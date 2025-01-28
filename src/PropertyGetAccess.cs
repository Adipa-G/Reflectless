using System;
using System.Linq.Expressions;

namespace Reflectless
{
    internal class PropertyGetAccess
    {
        internal static Func<object, object> GetPropertyGetAccessor(Type type, string name)
        {
            var property = type.GetProperty(name);
            if (property == null)
            {
                throw new Exception($"The property {name} in type {type.FullName} does not exists.");
            }

            var classTypeExpr = Expression.Parameter(typeof(object), "clsType");

            var expr = Expression.Lambda<Func<object, object>>(
                Expression.Convert(Expression.Call(Expression.Convert(classTypeExpr, type), property.GetMethod),
                    typeof(object)), classTypeExpr);

            return expr.Compile();
        }

        internal static Func<TClass, TMember> GetPropertyGetAccessor<TClass, TMember>(string name)
        {
            var property = typeof(TClass).GetProperty(name);
            if (property == null)
            {
                throw new Exception($"The property {name} in type {typeof(TClass).FullName} does not exists.");
            }

            var classTypeExpr = Expression.Parameter(typeof(TClass), "clsType");

            var expr = Expression.Lambda<Func<TClass, TMember>>(
                Expression.Convert(Expression.Call(classTypeExpr, property.GetMethod), typeof(TMember)), classTypeExpr);
            
            return expr.Compile();
        }
    }
}
