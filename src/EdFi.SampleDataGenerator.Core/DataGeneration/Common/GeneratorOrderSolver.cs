using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Attributes;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.Sorting;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common
{
    public static class GeneratorOrderSolver
    {
        public static List<IEntityAttributeGenerator<TContext, TConfiguration>> SortByDependencies<TContext, TConfiguration>(this IEnumerable<IEntityAttributeGenerator<TContext, TConfiguration>> generators) 
        {
            var groupedGenerators = generators.GroupBy(g => g.FullyQualifiedFieldName);
            
            var orderedGeneratorGroups = TopologicalSort.Sort(groupedGenerators,
                FlattenFieldNamesList, x => x.Key);

            var sortedList = new List<IEntityAttributeGenerator<TContext, TConfiguration>>();
            sortedList.AddRange(orderedGeneratorGroups.SelectMany(x => x));
            
            return sortedList;
        }

        private static List<string> FlattenFieldNamesList<TContext, TConfiguration>(IGrouping<string, IEntityAttributeGenerator<TContext, TConfiguration>> generatorGroup)
        {
            return generatorGroup.SelectMany(g => g.DependsOnFields.Select(f => f.FullyQualifiedFieldName)).Distinct().ToList();
        }

        public static List<IInterchangeEntityGenerator<TContext, TConfiguration>> SortByDependencies<TContext, TConfiguration>(this IEnumerable<IInterchangeEntityGenerator<TContext, TConfiguration>> generators)
        {
            var orderedGenerators = TopologicalSort.Sort(generators, g => g.InternalEntityDependencies.Select(e => e.ClassName), k => k.GeneratesEntity.ClassName);
            return orderedGenerators.ToList();
        }

        public static List<IInterchangeDataGenerator<TContext, TConfiguration>> SortByDependencies<TContext, TConfiguration>(this IEnumerable<IInterchangeDataGenerator<TContext, TConfiguration>> generators)
        {
            var orderedGenerators = TopologicalSort.Sort(generators, g => g.DependsOnInterchanges.Select(i => i.Name).Distinct(), k => k.InterchangeEntity.Interchange.Name);
            return orderedGenerators.ToList();
        }
    }
}
