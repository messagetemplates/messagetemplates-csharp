using System;
using System.Collections;
using System.Collections.Generic;
using MessageTemplates.Structure;

namespace MessageTemplates.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class ScalarTemplatePropertyValueDictionary
        : IEnumerable<KeyValuePair<ScalarValue, TemplatePropertyValue>>
    {
        private IEnumerable<KeyValuePair<ScalarValue, TemplatePropertyValue>> elements;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elements"></param>
        public ScalarTemplatePropertyValueDictionary(IEnumerable<KeyValuePair<ScalarValue, TemplatePropertyValue>> elements)
        {
            this.elements = elements;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<ScalarValue, TemplatePropertyValue>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}