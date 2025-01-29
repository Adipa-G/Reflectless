using System;
using System.Linq.Expressions;

namespace Reflectless
{
    internal class FieldGetAccess
    {
        internal static Func<object, object> GetFieldGetAccessor(Type type, string name)
        {
            var field = type.GetField(name);
            if (field == null)
            {
                throw new Exception($"The field {name} in type {type.FullName} does not exists.");
            }

            var classTypeExpr = Expression.Parameter(typeof(object), "clsType");
            var fieldExpr = Expression.Field(Expression.Convert(classTypeExpr, type), name);
            var lambdaExpr = Expression.Lambda<Func<object, object>>(Expression.Convert(fieldExpr, typeof(object)),
                classTypeExpr);

            return lambdaExpr.Compile();
        }

        internal static Func<TClass, TMember> GetFieldGetAccessor<TClass, TMember>(string name)
        {
            var field = typeof(TClass).GetField(name);
            if (field == null)
            {
                throw new Exception($"The field {name} in type {typeof(TClass).FullName} does not exists.");
            }

            var classTypeExpr = Expression.Parameter(typeof(TClass), "clsType");
            var fieldExpr = Expression.Field(classTypeExpr, name);

            var lambdaExpr = Expression.Lambda<Func<TClass, TMember>>(Expression.Convert(fieldExpr, field.FieldType),
                classTypeExpr);

            return lambdaExpr.Compile();
        }
    }
}
