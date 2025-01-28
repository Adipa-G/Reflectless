using System;

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
    }
}
