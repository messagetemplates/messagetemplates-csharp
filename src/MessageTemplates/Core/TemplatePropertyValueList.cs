using System;
using System.Collections.Generic;
using System.Linq;
using MessageTemplates.Structure;

namespace MessageTemplates.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class TemplatePropertyValueList
    {
#if RESHAPED_REFLECTION
    // Net40
#else
        // everything else
#endif
        private readonly TemplatePropertyValue[] _elements;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elements"></param>
        public TemplatePropertyValueList(IEnumerable<TemplatePropertyValue> elements)
        {
            this._elements = elements.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        public int Length => _elements.Length;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TemplatePropertyValue this[int index]
        {
            get { throw new NotImplementedException(); }
        }
    }
}