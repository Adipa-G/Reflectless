using System;
using System.Linq;

namespace Reflectless
{
    public class Reflectless
    {
        public static Func<object, object> GetPropertyGetAccessor(Type type, string name) =>
            PropertyGetAccess.GetPropertyGetAccessor(type, name);

        public static Func<TClass, TMember> GetPropertyGetAccessor<TClass, TMember>(string name) =>
            PropertyGetAccess.GetPropertyGetAccessor<TClass, TMember>(name);

        public static Action<object, object> GetPropertySetAccessor(Type type, string name) =>
            PropertySetAccess.GetPropertySetAccessor(type, name);

        public static Action<TClass, TMember> GetPropertySetAccessor<TClass, TMember>(string name) =>
            PropertySetAccess.GetPropertySetAccessor<TClass, TMember>(name);

        public static Func<object, object> GetFieldGetAccessor(Type type, string name) =>
            FieldGetAccess.GetFieldGetAccessor(type, name);

        public static Func<TClass, TMember> GetFieldGetAccessor<TClass, TMember>(string name) =>
            FieldGetAccess.GetFieldGetAccessor<TClass, TMember>(name);

        public static Action<object, object> GetFieldSetAccessor(Type type, string name) =>
            FieldSetAccess.GetFieldSetAccessor(type, name);

        public static Action<TClass, TMember> GetFieldSetAccessor<TClass, TMember>(string name) =>
            FieldSetAccess.GetFieldSetAccessor<TClass, TMember>(name);

        public static Func<object> GetDefaultConstructorAccessor(Type type) =>
            ConstructorAccess.GetDefaultConstructorAccessor(type);

        public static Func<TClass> GetDefaultConstructorAccessor<TClass>() =>
            ConstructorAccess.GetConstructorAccessor<Func<TClass>>(typeof(TClass));

        public static TFunc GetConstructorAccessor<TFunc>(Type type, params Type[] parameterTypes) =>
            ConstructorAccess.GetConstructorAccessor<TFunc>(type, parameterTypes);

        public static TFunc GetConstructorAccessor<TFunc>() => ConstructorAccess.GetConstructorAccessor<TFunc>();
    }
}
