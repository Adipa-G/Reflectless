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
            var callExpr = Expression.Call(Expression.Convert(classTypeExpr, type), property.GetMethod);
            var lambdaExpr =
                Expression.Lambda<Func<object, object>>(Expression.Convert(callExpr, typeof(object)), classTypeExpr);

            return lambdaExpr.Compile();
        }

        internal static Func<TClass, TMember> GetPropertyGetAccessor<TClass, TMember>(string name)
        {
            var property = typeof(TClass).GetProperty(name);
            if (property == null)
            {
                throw new Exception($"The property {name} in type {typeof(TClass).FullName} does not exists.");
            }

            var classTypeExpr = Expression.Parameter(typeof(TClass), "clsType");
            var callExpr = Expression.Call(classTypeExpr, property.GetMethod);
            var lambdaExpr =
                Expression.Lambda<Func<TClass, TMember>>(Expression.Convert(callExpr, typeof(TMember)), classTypeExpr);
            
            return lambdaExpr.Compile();
        }
    }
}
