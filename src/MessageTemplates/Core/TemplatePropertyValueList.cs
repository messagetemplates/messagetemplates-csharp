using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MessageTemplates
{
    /// <summary>
    /// 
    /// </summary>
    public class TemplatePropertyValueList : IEnumerable<TemplatePropertyValue>
    {
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
        public int Count => _elements.Length;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TemplatePropertyValue this[int index] => _elements[index];

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<TemplatePropertyValue> GetEnumerator()
        {
            for (int index = 0; index < _elements.Length; index++)
            {
                var templateProperty = _elements[index];
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