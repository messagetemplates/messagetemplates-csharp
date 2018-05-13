// Copyright 2014 Serilog Contributors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;

namespace MessageTemplates
{
    /// <summary>
    /// Represents a message template passed to a log method. The template
    /// can subsequently render the template in textual form given the list
    /// of properties.
    /// </summary>
    public class MessageTemplate
    {
        readonly string _text;
        readonly MessageTemplateToken[] _tokens;

        // Optimisation for when the template is bound to
        // property values.
        readonly PropertyToken[] _positionalProperties;
        readonly PropertyToken[] _namedProperties;

        /// <summary>
        /// Construct a message template using manually-defined text and property tokens.
        /// </summary>
        /// <param name="text">The full text of the template; used by MessageTemplates internally to avoid unneeded
        /// string concatenation.</param>
        /// <param name="tokens">The text and property tokens defining the template.</param>
        public MessageTemplate(string text, IEnumerable<MessageTemplateToken> tokens)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (tokens == null) throw new ArgumentNullException(nameof(tokens));

            _text = text;
            _tokens = tokens.ToArray();

            var propertyTokens = _tokens.OfType<PropertyToken>().ToArray();
            if (propertyTokens.Length != 0)
            {
                var allPositional = true;
                var anyPositional = false;
                foreach (var propertyToken in propertyTokens)
                {
                    if (propertyToken.IsPositional)
                        anyPositional = true;
                    else
                        allPositional = false;
                }

                if (allPositional)
                {
                    _positionalProperties = propertyTokens;
                }
                else
                {
                    if (anyPositional)
                        SelfLog.WriteLine("Message template is malformed: {0}", text);

                    _namedProperties = propertyTokens;
                }
            }
        }

        /// <summary>
        /// The raw text describing the template.
        /// </summary>
        public string Text
        {
            get { return _text; }
        }

        /// <summary>
        /// Render the template as a string.
        /// </summary>
        /// <returns>The string representation of the template.</returns>
        public override string ToString()
        {
            return Text;
        }

        /// <summary>
        /// The tokens parsed from the template.
        /// </summary>
        public IEnumerable<MessageTemplateToken> Tokens => _tokens;

        internal PropertyToken[] NamedProperties => _namedProperties;

        internal PropertyToken[] PositionalProperties => _positionalProperties;

        /// <summary>
        /// Convert the message template into a textual message, given the
        /// properties matching the tokens in the message template.
        /// </summary>
        /// <param name="properties">Properties matching template tokens.</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <returns>The message created from the template and properties. If the
        /// properties are mismatched with the template, the template will be
        /// returned with incomplete substitution.</returns>
        public string Render(TemplatePropertyValueDictionary properties, IFormatProvider formatProvider = null)
        {
            var writer = new StringWriter(formatProvider);
            Render(properties, writer, formatProvider);
            return writer.ToString();
        }

        /// <summary>
        /// Convert the message template into a textual message, given the
        /// properties matching the tokens in the message template.
        /// </summary>
        /// <param name="properties">Properties matching template tokens.</param>
        /// <param name="output">The message created from the template and properties. If the
        /// properties are mismatched with the template, the template will be
        /// returned with incomplete substitution.</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        public void Render(TemplatePropertyValueDictionary properties, TextWriter output, IFormatProvider formatProvider = null)
        {
            foreach (var token in _tokens)
            {
                token.Render(properties, output, formatProvider);
            }
        }

        /// <summary>
        /// Parses a message template (e.g. "hello, {name}") into a
        /// <see cref="MessageTemplate"/> structure.
        /// </summary>
        /// <param name="templateMessage">A message template (e.g. "hello, {name}")</param>
        /// <returns>The parsed message template.</returns>
        public static MessageTemplate Parse(string templateMessage)
        {
            return new MessageTemplateParser().Parse(templateMessage);
        }

        /// <summary>
        /// Render
        /// </summary>
        public void Format(IFormatProvider formatProvider, TextWriter output, params object[] values)
        {
            var props = Capture(this, values);
            this.Render(new TemplatePropertyValueDictionary(props), output, formatProvider);
        }

        /// <summary>
        /// Render
        /// </summary>
        public string Format(IFormatProvider formatProvider, params object[] values)
        {
            var sw = new StringWriter(formatProvider);
            Format(formatProvider, sw, values);
            sw.Flush();
            return sw.ToString();
        }

        /// <summary>
        /// Captures properties from the given template message and the provided values.
        /// </summary>
        public static TemplatePropertyList Capture(
            string templateMessage, params object[] values)
        {
            var template = Parse(templateMessage);
            return Capture(template, values);
        }

        /// <summary>
        /// Captures properties from the given message template and
        /// provided values.
        /// </summary>
        public static TemplatePropertyList CaptureWith(
            int maximumDepth, IEnumerable<Type> additionalScalarTypes,
            IEnumerable<IDestructuringPolicy> additionalDestructuringPolicies,
            MessageTemplate template, params object[] values)
        {
            var binder = new PropertyBinder(
                new PropertyValueConverter(
                    maximumDepth,
                    additionalScalarTypes ?? Enumerable.Empty<Type>(),
                    additionalDestructuringPolicies ?? Enumerable.Empty<IDestructuringPolicy>()));

            return binder.ConstructProperties(template, values);
        }

        /// <summary>
        /// Captures properties from the given message template and
        /// provided values.
        /// </summary>
        public static TemplatePropertyList Capture(
            MessageTemplate template, params object[] values)
        {
            var binder = new PropertyBinder(
                new PropertyValueConverter(
                    10,
                    Enumerable.Empty<Type>(),
                    Enumerable.Empty<IDestructuringPolicy>()));

            return binder.ConstructProperties(template, values);
        }

        /// <summary>
        /// Formats the message template as a string, written into the text
        /// writer.
        /// </summary>
        public static void Format(
            IFormatProvider formatProvider,
            TextWriter output,
            string templateMessage,
            params object[] values)
        {
            var template = Parse(templateMessage);
            template.Format(formatProvider, output, values);
        }

        /// <summary>
        /// Formats the message template as a string using the provided
        /// format provider and values.
        /// </summary>
        public static string Format(
            IFormatProvider formatProvider,
            string templateMessage,
            params object[] values)
        {
            var sw = new StringWriter(formatProvider);
            Format(formatProvider, sw, templateMessage, values);
            sw.Flush();
            return sw.ToString();
        }

        /// <summary>
        /// Formats the message template as a string using the provided values.
        /// </summary>
        public static string Format(
            string templateMessage,
            params object[] values)
        {
            return Format(CultureInfo.InvariantCulture, templateMessage, values);
        }
    }
}