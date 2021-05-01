using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common.Attributes
{
    public interface IEntityField
    {
        IEntity Entity { get; }
        string FieldName { get; }
        string FullyQualifiedFieldName { get; }
        /// <summary>
        /// If true, this Field isn't actually a member of Entity, but rather a "virtual" field
        /// added for dependency tracking
        /// </summary>
        bool IsVirtual { get; }
    }
}