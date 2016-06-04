﻿// Copyright 2013-2016 Serilog Contributors
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
using MessageTemplates.Structure;

namespace MessageTemplates.Parsing
{
    /// <summary>
    /// An element parsed from a message template string.
    /// </summary>
    public abstract class MessageTemplateToken
    {
        readonly int _startIndex;

        /// <summary>
        /// Construct a <see cref="MessageTemplateToken"/>.
        /// </summary>
        /// <param name="startIndex">The token's start index in the template.</param>
        protected MessageTemplateToken(int startIndex)
        {
            _startIndex = startIndex;
        }

        /// <summary>
        /// The token's start index in the template.
        /// </summary>
        public int StartIndex
        {
            get { return _startIndex; }
        }

        /// <summary>
        /// The token's length.
        /// </summary>
        public abstract int Length { get; }

        /// <summary>
        /// Render the token to the output.
        /// </summary>
        /// <param name="properties">Properties that may be represented by the token.</param>
        /// <param name="output">Output for the rendered string.</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        public abstract void Render(IReadOnlyDictionary<string, TemplatePropertyValue> properties, TextWriter output, IFormatProvider formatProvider = null);
    }
}