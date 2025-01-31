using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Reflectless
{
    internal class MethodAccess
    {
        internal static TFuncOrAction GetMethodAccessor<TFuncOrAction>(string name)
        {
            var funcType = typeof(TFuncOrAction);
            var typeArguments = funcType.GetGenericArguments();
            var objectType = typeArguments[0];
            var parameters = typeArguments.Skip(1).Take(typeArguments.Length - 2).ToArray();

            return GetMethodAccessor<TFuncOrAction>(objectType, name, parameters);
        }


        internal static TFuncOrAction GetMethodAccessor<TFuncOrAction>(Type type, string name, params Type[]? methodParameterTypes)
        {
            var method = methodParameterTypes != null && methodParameterTypes.Length > 0
                ? type.GetMethod(name, methodParameterTypes)
                : type.GetMethod(name);

            if (method == null)
            {
                var methodDetails = methodParameterTypes == null || methodParameterTypes.Length == 0
                    ? null
                    : $"method with name {name} and parameters [{string.Join(",", methodParameterTypes.Select(t => t.Name))}]";
                methodDetails ??= $"method with name {name}";
                throw new Exception($"The {methodDetails} in type {type.FullName} does not exists.");
            }

            var methodParameters = method.GetParameters();
            var types = methodParameters.Select(mp => mp.ParameterType).ToList();
            types.Insert(0, type);

            var lambdaInputParameterExprList = new List<ParameterExpression>();
            var callInputParameterExprList = new List<Expression>();

            var funcType = typeof(TFuncOrAction);
            var typeArguments = funcType.GetGenericArguments();
            var returnTypeParameter = method.ReturnType == typeof(void) ? typeof(void) : typeArguments[^1];
            
            for (var index = 0; index < types.Count; index++)
            {
                var methodParameterType = types[index];

                var parameterExpression = Expression.Parameter(typeof(object), $"inputType{index}");
                lambdaInputParameterExprList.Add(parameterExpression);
                if (index > 0)
                {
                    callInputParameterExprList.Add(Expression.Convert(parameterExpression, methodParameterType));
                }
            }

            var callExpr = Expression.Call(Expression.Convert(lambdaInputParameterExprList[0], type) , method, callInputParameterExprList);
            var lambdaExpr = Expression.Lambda<TFuncOrAction>(Expression.Convert(callExpr, returnTypeParameter), lambdaInputParameterExprList);
            
            return lambdaExpr.Compile();
        }
    }
}
