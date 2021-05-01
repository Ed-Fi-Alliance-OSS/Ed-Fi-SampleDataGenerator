using System;
using System.Collections;
using System.Collections.Generic;
using CloneExtensions;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges
{
    public interface ISdgEntityOutputCollection
    {
        object ConvertToEdFiInterchange();
        void Clear();
        void CopyToCollection(object item);
        void CopyToCollection(IEnumerable<object> items);
        Type SdgEntityType { get; }
    }

    public interface ISdgEntityOutputCollection<in TSdgEntity> : ISdgEntityOutputCollection
    {
        void CopyToCollection(TSdgEntity item);
        void CopyToCollection(IEnumerable<TSdgEntity> items);
    }

    public abstract class SdgEntityCollectionBase<TSdgEntity> : ISdgEntityOutputCollection<TSdgEntity>, IEnumerable
        where TSdgEntity: class, new()
    {
        private readonly IList _buffer;

        protected SdgEntityCollectionBase()
        {
            var interchangeOutputInfo = typeof(TSdgEntity).GetInterchangeOutputInfo();
            if (interchangeOutputInfo == null)
            {
                throw new InvalidOperationException($"Type {typeof(TSdgEntity).Name} must have an [{nameof(InterchangeOutputAttribute)}] attribute applied to be used with this class");
            }

            var bufferListType = typeof(List<>).MakeGenericType(interchangeOutputInfo.InterchangeItemType);
            _buffer = (IList)Activator.CreateInstance(bufferListType);
        }

        public Type SdgEntityType => typeof(TSdgEntity);

        private Array ConvertToBaseObjects(TSdgEntity items)
        {
            return items.ConvertToBaseObjects();
        }

        public object ConvertToEdFiInterchange()
        {
            return _buffer.Count > 0
                ? TypeConversionHelpers.ConvertToEdFiInterchange(this)
                : null;
        }

        public void CopyToCollection(TSdgEntity item)
        {
            var itemClone = item.GetClone();
            var baseObjects = ConvertToBaseObjects(itemClone);
            foreach (var baseObject in baseObjects)
            {
                _buffer.Add(baseObject);
            }
        }

        public void CopyToCollection(object item)
        {
            CopyToCollection(item as TSdgEntity);
        }

        public void CopyToCollection(IEnumerable<TSdgEntity> items)
        {
            foreach (var item in items)
            {
                CopyToCollection(item);
            }
        }

        public void CopyToCollection(IEnumerable<object> items)
        {
            foreach (var item in items)
            {
                CopyToCollection(item as TSdgEntity);
            }
        }

        public void Clear()
        {
            _buffer.Clear();
        }

        public IEnumerator GetEnumerator()
        {
            return _buffer.GetEnumerator();
        }
    }
}