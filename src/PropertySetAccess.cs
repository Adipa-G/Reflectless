using System;
using System.Linq.Expressions;

namespace Reflectless
{
    internal class PropertySetAccess
    {
        internal static Action<object, object> GetPropertySetAccessor(Type type, string name)
        {
            var property = type.GetProperty(name);
            if (property == null)
            {
                throw new Exception($"The property {name} in type {type.FullName} does not exists.");
            }

            var classTypeExpr = Expression.Parameter(typeof(object), "clsType");
            var inputParamTypeExpr = Expression.Parameter(typeof(object), "inputType");
            var callExpr = Expression.Call(Expression.Convert(classTypeExpr, type), property.SetMethod,
                Expression.Convert(inputParamTypeExpr, property.PropertyType));
            var lambdaExpr = Expression.Lambda<Action<object, object>>(Expression.Convert(callExpr, typeof(void)),
                classTypeExpr, inputParamTypeExpr);

            return lambdaExpr.Compile();
        }

        internal static Action<TClass, TMember> GetPropertySetAccessor<TClass, TMember>(string name)
        {
            var property = typeof(TClass).GetProperty(name);
            if (property == null)
            {
                throw new Exception($"The property {name} in type {typeof(TClass).FullName} does not exists.");
            }

            var classTypeExpr = Expression.Parameter(typeof(TClass), "clsType");
            var inputParamTypeExpr = Expression.Parameter(typeof(TMember), "inputType");
            var callExpr = Expression.Call(classTypeExpr, property.SetMethod, inputParamTypeExpr);
            var lambdaExpr = Expression.Lambda<Action<TClass, TMember>>(
                Expression.Convert(callExpr,
                    typeof(void)), classTypeExpr, inputParamTypeExpr);

            return lambdaExpr.Compile();
        }
    }
}
