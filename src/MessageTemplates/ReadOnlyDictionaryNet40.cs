using System.Linq;

namespace System.Collections.Generic
{
#pragma warning disable 1591
#if USE_READONLY_COLLECTION_40
    public interface IReadOnlyCollection<out T> : IEnumerable<T>, IEnumerable
    {
        int Count { get; }
    }

    public interface IReadOnlyDictionary<TKey, TValue> :
        IReadOnlyCollection<KeyValuePair<TKey, TValue>>,
        IEnumerable<KeyValuePair<TKey, TValue>>,
        IEnumerable
    {
        TValue this[TKey key] { get; }
        IEnumerable<TKey> Keys { get; }
        IEnumerable<TValue> Values { get; }
        bool ContainsKey(TKey key);
        bool TryGetValue(TKey key, out TValue value);
    }

    public interface IReadOnlyList<out T> :
        IReadOnlyCollection<T>, IEnumerable<T>, IEnumerable
    {
        T this[int index] { get; }
    }

    public class ReadOnlyDictionary<TKey, TValue> :
        IReadOnlyDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> dictionary;

        public ReadOnlyDictionary(Dictionary<TKey, TValue> dictionary)
        {
            this.dictionary = dictionary;
        }

        public TValue this[TKey key] { get { return this.dictionary[key]; } }
        public int Count { get { return this.dictionary.Count; } }
        public IEnumerable<TKey> Keys { get { return this.dictionary.Keys; } }
        public IEnumerable<TValue> Values { get { return this.dictionary.Values; } }
        public bool ContainsKey(TKey key) { return this.dictionary.ContainsKey(key); }
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this.dictionary.GetEnumerator();
        }
        public bool TryGetValue(TKey key, out TValue value)
        {
            return this.dictionary.TryGetValue(key, out value);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.dictionary).GetEnumerator();
        }
    }

    public class ReadOnlyList<T> : IReadOnlyList<T>
    {
        private readonly T[] elements;

        public ReadOnlyList(T[] elements)
        {
            this.elements = elements;
        }
        public T this[int index] { get { return this.elements[index]; } }
        public int Count { get { return this.elements.Length; } }
        public IEnumerator<T> GetEnumerator()
        {
            return this.elements.AsEnumerable().GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.elements.GetEnumerator();
        }
    }
#endif

    /// <summary>
    /// Allows support both .NET 4.0 (which does not have IReadOnly* collection
    /// interfaces) and .NET 4.5+ (which does have them).
    /// </summary>
    public static class Net40ReadOnlyDictionaryExtensions
    {
        public static IReadOnlyList<T> ToListNet40<T>(
            this T[] source)
        {
#if USE_READONLY_COLLECTION_40
            return new ReadOnlyList<T>(source.ToArray());
#else
            return source;
#endif
        }

        public static IReadOnlyDictionary<TKey, TElement> ToDictionary40<TSource, TKey, TElement>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector)
        {
#if USE_READONLY_COLLECTION_40
            return new ReadOnlyDictionary<TKey, TElement>(
                source.ToDictionary(keySelector, elementSelector));
#else
            return source.ToDictionary(keySelector, elementSelector);
#endif
        }
    }
#pragma warning restore 1591
}
