using System.Collections.Generic;
using System.Reflection;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common.Mutators
{
    public static class MutatorFactory
    {
        public static IEnumerable<IGlobalMutator> GlobalMutatorFactory(IRandomNumberGenerator randomNumberGenerator)
        {
            return Assembly.GetExecutingAssembly().CreateAll<IGlobalMutator>(randomNumberGenerator);
        }

        public static IEnumerable<IStudentMutator> StudentMutatorFactory(IRandomNumberGenerator randomNumberGenerator)
        {
            return Assembly.GetExecutingAssembly().CreateAll<IStudentMutator>(randomNumberGenerator);
        }
    }
}
