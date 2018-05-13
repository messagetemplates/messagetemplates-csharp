using System;
using System.Collections;
using System.Collections.Generic;

namespace MessageTemplates
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
            if (result == null) throw new ArgumentNullException(nameof(result));
            this.result = result;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Length => result.Length;

        /// <summary>
        /// 
        /// </summary>
        public int Count => result.Length;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TemplateProperty this[int index] => result[index];

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="IEnumerator{T}" /> object that can be used to iterate 
        /// through the collection.</returns>
        public IEnumerator<TemplateProperty> GetEnumerator()
        {
            return ((IEnumerable<TemplateProperty>) result).GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}