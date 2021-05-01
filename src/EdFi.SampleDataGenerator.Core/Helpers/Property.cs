using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace EdFi.SampleDataGenerator.Core.Helpers
{
    public static class Property
    {
        public static IReadOnlyList<PropertyInfo> Path<TModel>(Expression<Func<TModel, object>> expression)
        {
            var memberExpression = UnpackMemberExpression(expression); // x.Property.Chain

            var path = new List<PropertyInfo>();

            while (memberExpression != null)
            {
                var property = (PropertyInfo)memberExpression.Member;

                //We encounter each property in the chain from right to left.
                path.Insert(0, property);

                memberExpression = memberExpression.Expression as MemberExpression;
            }

            return path;
        }

        public static PropertyInfo From<TModel>(Expression<Func<TModel, object>> expression)
        {
            return (PropertyInfo)GetMember(expression);
        }

        private static MemberInfo GetMember<TModel>(Expression<Func<TModel, object>> expression)
        {
            var memberExpression = UnpackMemberExpression(expression); // x.Property

            return memberExpression.Member; // Property
        }

        private static MemberExpression UnpackMemberExpression<TModel>(Expression<Func<TModel, object>> expression)
        {
            // The calling code may specify a lambda to a specific property:
            //      x => x.Property

            // However, depending on the type of that property, the incoming expression
            // object may arrive here in one of two forms:
            //
            //      x => (object)x.Property
            //
            //      x => x.Property

            // If the expression includes the cast, then it will be a UnaryExpression,
            // and we will need to unpack the operand of that expression in order to
            // access the original lambda's body.

            var body = expression.Body; // (object)x.Property OR x.Property

            var castToObject = body as UnaryExpression;
            if (castToObject != null) // (object)x.Property
                body = castToObject.Operand; // x.Property

            return (MemberExpression)body; // x.Property
        }
    }
}