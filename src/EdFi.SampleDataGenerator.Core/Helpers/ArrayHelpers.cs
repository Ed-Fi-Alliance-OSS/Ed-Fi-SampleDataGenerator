namespace EdFi.SampleDataGenerator.Core.Helpers
{
    public static class ArrayHelpers
    {
        public static T[] CreateCopyAndAppend<T>(this T[] array, T newItem)
        {
            if (array == null)
            {
                return new[] {newItem};
            }

            var result = new T[array.Length + 1];
            array.CopyTo(result, 0);
            result[result.Length - 1] = newItem;
            
            return result;
        }
    }
}
