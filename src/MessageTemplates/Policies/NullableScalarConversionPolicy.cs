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

using System;
using MessageTemplates;

class NullableScalarConversionPolicy : IScalarConversionPolicy
{
    public bool TryConvertToScalar(object value, IMessageTemplatePropertyValueFactory propertyValueFactory,
        out ScalarValue result)
    {
        var type = value.GetType();
#if !REFLECTION_API_EVOLVED
            if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(Nullable<>))
#else
        if (!type.IsConstructedGenericType || type.GetGenericTypeDefinition() != typeof(Nullable<>))
#endif
        {
            result = null;
            return false;
        }
#if USE_DYNAMIC
            var dynamicValue = (dynamic)value;
            var innerValue = dynamicValue.HasValue ? (object)dynamicValue.Value : null;
#elif !REFLECTION_API_EVOLVED
            var targetType = type.GetGenericArguments()[0];
            var innerValue = Convert.ChangeType(value, targetType, null);
#else
        var targetType = type.GenericTypeArguments[0];
        var innerValue = Convert.ChangeType(value, targetType);
#endif
        result = propertyValueFactory.CreatePropertyValue(innerValue) as ScalarValue;
        return result != null;
    }
}