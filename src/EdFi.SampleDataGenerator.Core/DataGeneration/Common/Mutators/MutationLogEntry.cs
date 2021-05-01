namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common.Mutators
{
    public class MutationLogEntry
    {
        public string MutatorName { get; set; }
        public MutationType MutationType { get; set; }
        public string Interchange { get; set; }
        public string Entity { get; set; }
        public string Attribute { get; set; }
        public string EntityKey { get; set; }
        public object OldValue { get; set; }
        public object NewValue { get; set; }
    }
}