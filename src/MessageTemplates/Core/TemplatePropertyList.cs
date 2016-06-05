using System;
using MessageTemplates.Structure;

namespace MessageTemplates.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class TemplatePropertyList
    {
#if RESHAPED_REFLECTION
    // Net40
#else
        // everything else
#endif
        private TemplateProperty[] result;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        public TemplatePropertyList(TemplateProperty[] result)
        {
            this.result = result;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string this[int index]
        {
            get { throw new NotImplementedException(); }
        }
    }
}