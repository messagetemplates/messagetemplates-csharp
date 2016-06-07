using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessageTemplates.Structure;

namespace MessageTemplates.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class TemplatePropertyValueDictionary
    {
        private Dictionary<string, TemplatePropertyValue> props;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="props"></param>
        public TemplatePropertyValueDictionary(TemplatePropertyList props)
        {
            this.props = props.ToDictionary(p => p.Name, p=> p.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        public bool TryGetValue(string propertyName, out TemplatePropertyValue propertyValue)
        {
            return props.TryGetValue(propertyName, out propertyValue);
        }
    }
}
