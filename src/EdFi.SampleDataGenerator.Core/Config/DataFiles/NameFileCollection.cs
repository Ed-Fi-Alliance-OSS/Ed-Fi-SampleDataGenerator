using System;
using System.Collections;
using System.Collections.Generic;

namespace EdFi.SampleDataGenerator.Core.Config.DataFiles
{
    public class NameFileCollection<TKey, TValue> : IEnumerable<TValue>
        where TKey : IEqualityComparer<TKey>
        where TValue : NameFile
    {
        private readonly Dictionary<TKey, TValue> _data = new Dictionary<TKey, TValue>();

        public TValue this[TKey key]
        {
            get { return _data[key]; }
            set { _data[key] = value; }
        }

        public int Count => _data.Count;

        public IEnumerator<TValue> GetEnumerator()
        {
            return _data.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class NameFileCollection<TKey1, TKey2, TValue> : IEnumerable<TValue>
        where TKey1 : IEqualityComparer<TKey1>
        where TValue : NameFile
    {
        private readonly Dictionary<Tuple<TKey1, TKey2>, TValue> _data = new Dictionary<Tuple<TKey1, TKey2>, TValue>();

        public TValue this[TKey1 key1, TKey2 key2]
        {
            get { return _data[Tuple.Create(key1, key2)]; }
            set { _data[Tuple.Create(key1, key2)] = value; }
        }

        public int Count => _data.Count;

        public IEnumerator<TValue> GetEnumerator()
        {
            return _data.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public static class NameFileCollectionExtensions
    {
        public static NameFileCollection<TKey, TValue> ToNameFileCollection<TKey, TValue>(this IEnumerable<TValue> values, Func<TValue, TKey> keyFunc)
            where TKey : IEqualityComparer<TKey>
            where TValue : NameFile
        {
            var result = new NameFileCollection<TKey, TValue>();
            foreach (var value in values)
            {
                result[keyFunc(value)] = value;
            }

            return result;
        }

        public static NameFileCollection<TKey1, TKey2, TValue> ToNameFileCollection<TKey1, TKey2, TValue>(this IEnumerable<TValue> values, Func<TValue, TKey1> key1Func, Func<TValue, TKey2> key2Func)
            where TKey1 : IEqualityComparer<TKey1>
            where TValue : NameFile
        {
            var result = new NameFileCollection<TKey1, TKey2, TValue>();
            foreach (var value in values)
            {
                result[key1Func(value), key2Func(value)] = value;
            }

            return result;
        }
    }
}