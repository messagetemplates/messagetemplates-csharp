// Copyright 2013-2015 Serilog Contributors
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

using MessageTemplates;

partial class PropertyValueConverter
{
    class DepthLimiter : IMessageTemplatePropertyValueFactory
    {
        readonly int _maximumDestructuringDepth;
        readonly int _currentDepth;
        readonly PropertyValueConverter _propertyValueConverter;

        public DepthLimiter(int currentDepth, int maximumDepth, PropertyValueConverter propertyValueConverter)
        {
            _maximumDestructuringDepth = maximumDepth;
            _currentDepth = currentDepth;
            _propertyValueConverter = propertyValueConverter;
        }

        public TemplatePropertyValue CreatePropertyValue(object value, Destructuring destructuring)
        {
            return DefaultIfMaximumDepth() ??
                _propertyValueConverter.CreatePropertyValue(value, destructuring, _currentDepth + 1);
        }

        public TemplatePropertyValue CreatePropertyValue(object value, bool destructureObjects = false)
        {
            return DefaultIfMaximumDepth() ??
                _propertyValueConverter.CreatePropertyValue(value, destructureObjects, _currentDepth + 1);
        }

        TemplatePropertyValue DefaultIfMaximumDepth()
        {
            if (_currentDepth == _maximumDestructuringDepth)
            {
                SelfLog.WriteLine("Maximum destructuring depth reached.");
                return new ScalarValue(null);
            }

            return null;
        }
    }
}