using System;
using System.Threading.Tasks;
using MessageTemplates.Structure;

namespace MessageTemplates.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class TemplatePropertyValueDictionary
    {
        private TemplatePropertyList props;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="props"></param>
        public TemplatePropertyValueDictionary(TemplatePropertyList props)
        {
            this.props = props;
        }
#if RESHAPED_REFLECTION
        // Net40
#else
        // everything else
#endif

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        public bool TryGetValue(string propertyName, out TemplatePropertyValue propertyValue)
        {
            throw new NotImplementedException();
        }
    }
}
