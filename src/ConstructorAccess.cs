using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reflectless
{
    internal class ConstructorAccess
    {
        internal static Func<object> GetDefaultConstructorAccessor(Type type)
        {
            return GetConstructorAccessor<Func<object>>(type);
        }

        internal static TFunc GetConstructorAccessor<TFunc>()
        {
            var funcType = typeof(TFunc);
            var typeArguments = funcType.GetGenericArguments();
            var returnType = typeArguments[^1];
            var parameters = typeArguments.Take(typeArguments.Length - 1).ToArray();

            return GetConstructorAccessor<TFunc>(returnType, parameters);
        }

        internal static TFunc GetConstructorAccessor<TFunc>(Type type, params Type[]? constructorParameterTypes)
        {
            var constructor = type.GetConstructor(constructorParameterTypes);
            if (constructor == null)
            {
                var constructorDetails = constructorParameterTypes == null || constructorParameterTypes.Length == 0
                    ? null
                    : $"constructor with parameters [{string.Join(",", constructorParameterTypes.Select(t => t.Name))}]";
                constructorDetails ??= "default constructor";
                throw new Exception($"The {constructorDetails} in type {type.FullName} does not exists.");
            }

            var lambdaInputParameterExprList = new List<ParameterExpression>();
            var callInputParameterExprList = new List<Expression>();

            if (constructorParameterTypes != null)
            {
                for (var index = 0; index < constructorParameterTypes.Length; index++)
                {
                    var constructionParameterType = constructorParameterTypes[index];

                    var parameterExpression = Expression.Parameter(typeof(object), $"inputType{index}");
                    lambdaInputParameterExprList.Add(parameterExpression);
                    callInputParameterExprList.Add(Expression.Convert(parameterExpression, constructionParameterType));
                }
            }

            var newExpr = Expression.New(constructor, callInputParameterExprList);
            var lambdaExpr = Expression.Lambda<TFunc>(newExpr, lambdaInputParameterExprList.ToArray());

            return lambdaExpr.Compile();
        }
    }
}
