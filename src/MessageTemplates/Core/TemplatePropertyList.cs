using System;
using System.Collections;
using System.Collections.Generic;
using MessageTemplates.Structure;

namespace MessageTemplates.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class TemplatePropertyList : IEnumerable<TemplateProperty>
    {
        private readonly TemplateProperty[] result;

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
        public int Length => result.Length;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TemplateProperty this[int index] => result[index];

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        public IEnumerator<TemplateProperty> GetEnumerator()
        {
            for (int index = 0; index < result.Length; index++)
            {
                var templateProperty = result[index];
                yield return templateProperty;
            }
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}