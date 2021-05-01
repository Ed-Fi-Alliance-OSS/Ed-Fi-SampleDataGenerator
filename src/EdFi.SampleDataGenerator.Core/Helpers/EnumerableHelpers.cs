using System.Collections.Generic;
using System.Linq;

namespace EdFi.SampleDataGenerator.Core.Helpers
{
    public static class EnumerableHelpers
    {
        public static IEnumerable<TOutput> SafeCast<TInput, TOutput>(this IEnumerable<TInput> list)
            where TInput: TOutput
        {
            return list?.Cast<TOutput>() ?? Enumerable.Empty<TOutput>();
        }

        public static IEnumerable<TOutput> SafeConcat<TOutput>(this IEnumerable<TOutput> result, IEnumerable<TOutput> input)
        {
            return Enumerable.Concat(result ?? Enumerable.Empty<TOutput>(), input ?? Enumerable.Empty<TOutput>());
        } 

        public static IEnumerable<TOutput> Concat<TInput, TOutput>(this IEnumerable<TOutput> result, IEnumerable<TInput> input)
            where TInput: TOutput
        {
            return result == null 
                ? input.SafeCast<TInput, TOutput>()
                : input == null
                    ? result
                    : Enumerable.Concat(result, input.SafeCast<TInput, TOutput>());
        }

        public static IEnumerable<T> Yield<T>(this T item)
        {
            yield return item;
        } 
    }
}