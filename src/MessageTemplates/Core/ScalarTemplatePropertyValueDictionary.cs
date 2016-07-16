using System;
using System.Collections;
using System.Collections.Generic;
using MessageTemplates.Structure;
using System.Linq;

namespace MessageTemplates.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class ScalarTemplatePropertyValueDictionary
        : IEnumerable<KeyValuePair<ScalarValue, TemplatePropertyValue>>
    {
        private IDictionary<ScalarValue, TemplatePropertyValue> elements;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elements"></param>
        public ScalarTemplatePropertyValueDictionary(
            IEnumerable<KeyValuePair<ScalarValue, TemplatePropertyValue>> elements)
        {
            this.elements = elements.ToDictionary(e => e.Key, e => e.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<ScalarValue, TemplatePropertyValue>> GetEnumerator()
        {
            return elements.GetEnumerator();
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