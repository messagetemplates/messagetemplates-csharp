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

using System.Reflection;
using MessageTemplates;

class EnumScalarConversionPolicy : IScalarConversionPolicy
{
    public bool TryConvertToScalar(object value, IMessageTemplatePropertyValueFactory propertyValueFactory, out ScalarValue result)
    {
        bool isEnum = false;
#if !REFLECTION_API_EVOLVED // https://blogs.msdn.microsoft.com/dotnet/2012/08/28/evolving-the-reflection-api/
        isEnum = value.GetType().IsEnum;
#else
        isEnum = value.GetType().GetTypeInfo().IsEnum;
#endif

        if (isEnum)
        {
            result = new ScalarValue(value);
            return true;
        }

        result = null;
        return false;
    }
}