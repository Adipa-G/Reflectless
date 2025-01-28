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

            var expr = Expression.Lambda<Func<object, object>>(
                Expression.Convert(Expression.Field(Expression.Convert(classTypeExpr, type), name), typeof(object)),
                classTypeExpr);

            return expr.Compile();
        }

        internal static Func<TClass, TMember> GetFieldGetAccessor<TClass, TMember>(string name)
        {
            var field = typeof(TClass).GetField(name);
            if (field == null)
            {
                throw new Exception($"The field {name} in type {typeof(TClass).FullName} does not exists.");
            }

            var classTypeExpr = Expression.Parameter(typeof(TClass), "clsType");
            
            var expr = Expression.Lambda<Func<TClass, TMember>>(
                Expression.Convert(Expression.Field(classTypeExpr, name), field.FieldType), classTypeExpr);

            return expr.Compile();
        }
    }
}
