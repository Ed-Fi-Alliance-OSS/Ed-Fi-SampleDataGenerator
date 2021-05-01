using System;
using System.Collections.Generic;
using System.Linq;

namespace EdFi.SampleDataGenerator.Core.Sorting
{
    public static class TopologicalSort
    {
        public static IEnumerable<TValue> Sort<TValue, TKey>(IEnumerable<TValue> source, Func<TValue, IEnumerable<TKey>> getDependencyKeys, Func<TValue, TKey> getKey)
        {
            var sorted = new List<TValue>();
            var itemMap = source.ToDictionary(getKey);
            var visited = new Dictionary<TKey, bool>();

            foreach (var item in source)
            {
                Visit(item, getDependencyKeys, sorted, visited, getKey, itemMap);
            }

            return sorted;
        }

        private static void Visit<TValue, TKey>(TValue item, Func<TValue, IEnumerable<TKey>> getDependencyKeys, List<TValue> sorted, Dictionary<TKey, bool> visited, Func<TValue, TKey> getKey, Dictionary<TKey,TValue> itemMap)
        {
            var key = getKey(item);
            
            if (visited.ContainsKey(key))
            {
                if (visited[key])
                {
                    throw new TopologicalSortException($"Sort failed due to cyclic dependency at key '{key}'");
                }
            }

            else
            {
                visited[key] = true;

                var dependencyKeys = getDependencyKeys(item);
                if (dependencyKeys != null)
                {
                    foreach (var dependencyKey in dependencyKeys)
                    {
                        if (!itemMap.ContainsKey(dependencyKey))
                            throw new TopologicalSortException($"Missing dependency named '{dependencyKey}' in source list");

                        var dependency = itemMap[dependencyKey];
                        Visit(dependency, getDependencyKeys, sorted, visited, getKey, itemMap);
                    }
                }

                visited[key] = false;
                sorted.Add(item);
            }
        }
    }

    public class TopologicalSortException : Exception
    {
        public TopologicalSortException()
        {
        }

        public TopologicalSortException(string message) : base(message)
        {
        }

        public TopologicalSortException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
