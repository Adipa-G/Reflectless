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

        internal static TFunc GetConstructorAccessor<TFunc>(Type type, params Type[]? constructionParameterTypes)
        {
            var constructor = type.GetConstructor(constructionParameterTypes);
            if (constructor == null)
            {
                var constructorDetails = constructionParameterTypes == null || constructionParameterTypes.Length == 0
                    ? null
                    : $"constructor with parameters [{string.Join(",", constructionParameterTypes.Select(t => t.Name))}]";
                constructorDetails ??= "default constructor";
                throw new Exception($"The {constructorDetails} in type {type.FullName} does not exists.");
            }

            var inputParameterTypeExprList = new List<ParameterExpression>();
            var constructorParameterTypeExprList = new List<Expression>();
            if (constructionParameterTypes != null)
            {
                for (var index = 0; index < constructionParameterTypes.Length; index++)
                {
                    var constructionParameterType = constructionParameterTypes[index];

                    var parameterExpression = Expression.Parameter(typeof(object), $"inputType{index}");
                    inputParameterTypeExprList.Add(parameterExpression);
                    constructorParameterTypeExprList.Add(Expression.Convert(parameterExpression, constructionParameterType));
                }
            }

            var newExpr = Expression.New(constructor, constructorParameterTypeExprList);
            var lambdaExpr = Expression.Lambda<TFunc>(newExpr, inputParameterTypeExprList.ToArray());

            return lambdaExpr.Compile();
        }
    }
}
