namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common.Mutators
{
    public class MutationResult
    {
        public bool Mutated { get; protected set; }
        public object OldValue { get; protected set; }
        public object NewValue { get; protected set; }

        protected MutationResult()
        {
        }

        public static MutationResult NoMutation => new MutationResult {Mutated = false, OldValue = null, NewValue = null};

        public static MutationResult NewMutation(object oldValue, object newValue)
        {
            return new MutationResult {Mutated = true, OldValue = oldValue, NewValue = newValue};
        }
    }
}