using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges
{
    public static class TypeConversionHelpers
    {
        public static object ConvertPropertyToEdFiInterchange<TEntity>(this TEntity entity, PropertyInfo property)
        {
            if (entity == null) return null;
            if (!property.ShouldBeOutput()) return null;

            var entityType = entity.GetType();
            var interchangeOutputInfo = property.GetInterchangeOutputInfo();
            if (interchangeOutputInfo == null)
            {
                throw new NotSupportedException($"Member {property.Name} of type {entityType.FullName} must have the [{nameof(InterchangeOutputAttribute)}] attribute, be a collection of an item type that has the [{nameof(InterchangeOutputAttribute)}] attribute, or be decorated with the [{nameof(DoNotOutputToInterchangeAttribute)}] attribute");
            }

            var propertyValue = property.GetValue(entity);
            var interchangeItem = ConvertToEdFiInterchangeHelper(propertyValue, interchangeOutputInfo);

            return interchangeItem;
        }

        public static object ConvertToEdFiInterchange<TEntity>(this TEntity entity)
        {
            if (entity == null) return null;

            var entityType = entity.GetType();
            var outputInfo = entityType.GetInterchangeOutputInfo() ?? entityType.GetUnderlyingType().GetInterchangeOutputInfo();
            if (outputInfo == null)
            {
                throw new InvalidOperationException($"Type {entityType.FullName} must have an [{nameof(InterchangeOutputAttribute)}] attribute applied or be a collection of an item type that has the [{nameof(InterchangeOutputAttribute)}] attribute");
            }

            var interchangeItem = ConvertToEdFiInterchangeHelper(entity, outputInfo);
            return interchangeItem;
        }

        private static object ConvertToEdFiInterchangeHelper<TEntity>(TEntity entity, InterchangeOutputInfo outputInfo)
        {
            var result = Activator.CreateInstance(outputInfo.InterchangeOutputType);

            //by convention, all interchange types have a public Items array field
            //if this member is not found, we've gotten a bad type or this convention has been violated
            var itemsMember = outputInfo.InterchangeOutputType.GetProperty("Items");
            if (itemsMember == null || !itemsMember.PropertyType.IsArray)
                throw new InvalidOperationException(
                    $"'{outputInfo.InterchangeOutputType.FullName}' type does not have a public array property named 'Items'");

            var itemsMemberUnderlyingType = itemsMember.PropertyType.GetUnderlyingType();
            if (!itemsMemberUnderlyingType.IsAssignableFrom(outputInfo.InterchangeItemType))
                throw new InvalidOperationException(
                    $"'{outputInfo.InterchangeItemType.FullName}' is not convertible to type '{itemsMemberUnderlyingType.FullName}'");

            var baseObjects = ConvertToBaseObjectsHelper(entity, outputInfo);
            itemsMember.SetValue(result, baseObjects);
            return result;
        }

        public static Array ConvertToBaseObjects<TEntity>(this TEntity entity)
        {
            var entityType = entity.GetType();
            var interchangeOutputInfo = entityType.GetInterchangeOutputInfo();

            if (interchangeOutputInfo == null)
            {
                throw new NotSupportedException($"Entity must have the [{nameof(InterchangeOutputAttribute)}] attribute, be a collection of an item type that has the [{nameof(InterchangeOutputAttribute)}] attribute, or be decorated with the [{nameof(DoNotOutputToInterchangeAttribute)}] attribute");
            }

            return ConvertToBaseObjectsHelper(entity, interchangeOutputInfo);
        }

        /// <summary>
        /// Converts a given entity to underlying "base" objects to be output with an Interchange item
        /// 
        /// Note: When calling this method you are effectively certifying that <paramref name="entity"/> is convertible.
        /// You should validate conversion is possible before calling this method.
        /// </summary>
        private static Array ConvertToBaseObjectsHelper<TEntity>(TEntity entity, InterchangeOutputInfo outputInfo)
        {
            if (entity == null) return null;

            var entityType = entity.GetType();
            var itemsMemberType = outputInfo.InterchangeItemType;

            var itemsListType = typeof(List<>).MakeGenericType(itemsMemberType);
            IList itemsList = (IList)Activator.CreateInstance(itemsListType);

            if (entityType.IsSupportedInterchangeCollectionType())
            {
                var entityItems = entity as IEnumerable;
                if (entityItems != null)
                {
                    foreach (var entityItem in entityItems)
                    {
                        ConvertToBaseObjectHelper(entityItem, itemsMemberType, itemsList);
                    }
                }
            }

            else
            {
                ConvertToBaseObjectHelper(entity, itemsMemberType, itemsList);
            }

            var itemsArray = Array.CreateInstance(itemsMemberType, itemsList.Count);
            itemsList.CopyTo(itemsArray, 0);
            return itemsArray;
        }

        //Note: this is a private helper method so that we can make an assumption
        //about the entity being passed in.
        //
        //If the destination type is assignable from the type of the entity passed in,
        //we'll assume we're at the "correct" level to convert the entity
        //to an interchange item type.
        //
        //Otherwise, we assume that the current entity is parent object for items
        //that *are* convertible to base objects.
        private static void ConvertToBaseObjectHelper<TEntity>(TEntity entity, Type itemsMemberType, IList itemsList)
        {
            if (entity == null) return;

            var entityType = entity.GetType();

            //If the entity type has interchange output info, we're still one level too high,
            //so we need to skip the following conditional.  This guards against the scenario
            //where the itemsMemberType is object, for instance, and adds the current entity to the
            //output list when it's the entity's child objects that should instead be added.
            if (itemsMemberType.IsAssignableFrom(entityType) && !entityType.HasInterchangeOutputInfo())
            {
                itemsList.Add(entity);
                return;
            }

            var entityMembers = entityType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);

            foreach (var entityMember in entityMembers)
            {
                if (entityMember.ShouldBeOutput())
                {
                    if (entityMember.PropertyType.IsSupportedInterchangeCollectionType())
                    {
                        var list = (IEnumerable)entityMember.GetValue(entity);
                        if (list == null) continue;
                        foreach (var item in list)
                        {
                            if (item == null) continue;
                            if (!itemsMemberType.IsInstanceOfType(item))
                            {
                                throw new InvalidCastException($"Can't convert type '{item.GetType().FullName}' to type '{itemsMemberType.FullName}' for entity type '{entityType.FullName}' - did you forget to decorate a member with [{nameof(DoNotOutputToInterchangeAttribute)}]?");
                            }

                            itemsList.Add(item);
                        }
                    }

                    else
                    {
                        var item = entityMember.GetValue(entity);
                        if (item == null) continue;
                        if (!itemsMemberType.IsInstanceOfType(item))
                        {
                            throw new InvalidCastException($"Can't convert type '{item.GetType().FullName}' to type '{itemsMemberType.FullName}' for entity type '{entityType.FullName}' - did you forget to decorate a member with [{nameof(DoNotOutputToInterchangeAttribute)}]?");
                        }

                        itemsList.Add(item);
                    }
                }
            }
        }
    }
}
