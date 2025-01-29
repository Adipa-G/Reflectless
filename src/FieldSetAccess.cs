using System;
using System.Linq.Expressions;

namespace Reflectless
{
    internal class FieldSetAccess
    {
        internal static Action<object, object> GetFieldSetAccessor(Type type, string name)
        {
            var field = type.GetField(name);
            if (field == null)
            {
                throw new Exception($"The field {name} in type {type.FullName} does not exists.");
            }

            var classTypeExpr = Expression.Parameter(typeof(object), "clsType");
            var inputParamTypeExpr = Expression.Parameter(typeof(object), "inputType");
            var fieldExpr = Expression.Field(Expression.Convert(classTypeExpr, type), field);
            var assignExpr = Expression.Assign(fieldExpr, Expression.Convert(inputParamTypeExpr, field.FieldType));

            var lambdaExpr = Expression.Lambda<Action<object, object>>(assignExpr, classTypeExpr, inputParamTypeExpr);

            return lambdaExpr.Compile();
        }

        internal static Action<TClass, TMember> GetFieldSetAccessor<TClass, TMember>(string name)
        {
            var field = typeof(TClass).GetField(name);
            if (field == null)
            {
                throw new Exception($"The field {name} in type {typeof(TClass).FullName} does not exists.");
            }

            var classTypeExpr = Expression.Parameter(typeof(TClass), "clsType");
            var inputParamTypeExpr = Expression.Parameter(typeof(TMember), "inputType");
            var fieldExpr = Expression.Field(classTypeExpr, field);
            var assignExpr = Expression.Assign(fieldExpr, inputParamTypeExpr);

            var lambdaExpr = Expression.Lambda<Action<TClass, TMember>>(assignExpr, classTypeExpr, inputParamTypeExpr);

            return lambdaExpr.Compile();
        }
    }
}
