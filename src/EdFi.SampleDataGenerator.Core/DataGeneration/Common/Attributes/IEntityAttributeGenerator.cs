namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common.Attributes
{
    public interface IEntityAttributeGenerator<in TContext, in TConfiguration> : IGenerator<TContext, TConfiguration> 
    {
        IEntityField GeneratesField { get; }
        IEntityField[] DependsOnFields { get; }
        string FullyQualifiedFieldName { get; }
        string FieldName { get; }
    }
}