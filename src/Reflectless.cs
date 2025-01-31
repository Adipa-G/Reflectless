using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Reflectless
{
    public class Reflectless
    {
        private static Dictionary<string, object> _cache = new Dictionary<string, object>();

        public static Func<object, object> GetPropertyGetAccessor(Type type, string name)
        {
            var cacheKey = $"non_generic_get_property_{type.FullName}_{name}";
            return GetFromCacheOrAddIfNotExists(cacheKey, () => ReflectlessNoCache.GetPropertyGetAccessor(type, name));
        }

        public static Func<TClass, TMember> GetPropertyGetAccessor<TClass, TMember>(string name)
        {
            var cacheKey = $"generic_get_property_{typeof(TClass).FullName}_{name}";
            return GetFromCacheOrAddIfNotExists(cacheKey, () => ReflectlessNoCache.GetPropertyGetAccessor<TClass, TMember>(name));
        }

        public static Action<object, object> GetPropertySetAccessor(Type type, string name)
        {
            var cacheKey = $"non_generic_set_property_{type.FullName}_{name}";
            return GetFromCacheOrAddIfNotExists(cacheKey, () => ReflectlessNoCache.GetPropertySetAccessor(type, name));
        }

        public static Action<TClass, TMember> GetPropertySetAccessor<TClass, TMember>(string name)
        {
            var cacheKey = $"generic_set_property_{typeof(TClass).FullName}_{name}";
            return GetFromCacheOrAddIfNotExists(cacheKey, () => ReflectlessNoCache.GetPropertySetAccessor<TClass, TMember>(name));
        }

        public static Func<object, object> GetFieldGetAccessor(Type type, string name)
        {
            var cacheKey = $"non_generic_get_field_{type.FullName}_{name}";
            return GetFromCacheOrAddIfNotExists(cacheKey, () => ReflectlessNoCache.GetFieldGetAccessor(type, name));
        }

        public static Func<TClass, TMember> GetFieldGetAccessor<TClass, TMember>(string name)
        {
            var cacheKey = $"generic_get_field_{typeof(TClass).FullName}_{name}";
            return GetFromCacheOrAddIfNotExists(cacheKey, () => ReflectlessNoCache.GetFieldGetAccessor<TClass, TMember>(name));
        }

        public static Action<object, object> GetFieldSetAccessor(Type type, string name)
        {
            var cacheKey = $"non_generic_set_field_{type.FullName}_{name}";
            return GetFromCacheOrAddIfNotExists(cacheKey, () => ReflectlessNoCache.GetFieldSetAccessor(type, name));
        }

        public static Action<TClass, TMember> GetFieldSetAccessor<TClass, TMember>(string name)
        {
            var cacheKey = $"generic_set_field_{typeof(TClass).FullName}_{name}";
            return GetFromCacheOrAddIfNotExists(cacheKey, () => ReflectlessNoCache.GetFieldSetAccessor<TClass, TMember>(name));
        }

        public static Func<object> GetDefaultConstructorAccessor(Type type)
        {
            var cacheKey = $"non_generic_default_constructor_{typeof(Type).FullName}";
            return GetFromCacheOrAddIfNotExists(cacheKey, () => ReflectlessNoCache.GetDefaultConstructorAccessor(type));
        }

        public static Func<TClass> GetDefaultConstructorAccessor<TClass>()
        {
            var cacheKey = $"generic_default_constructor_{typeof(TClass).FullName}";
            return GetFromCacheOrAddIfNotExists(cacheKey, () => ReflectlessNoCache.GetConstructorAccessor<Func<TClass>>(typeof(TClass)));
        }

        public static TFunc GetConstructorAccessor<TFunc>(Type type, params Type[] parameterTypes)
        {
            var cacheKey = $"non_generic_constructor_{type.FullName}_{string.Join("_", parameterTypes.Select(t => t.FullName))}";
            return GetFromCacheOrAddIfNotExists(cacheKey, () => ReflectlessNoCache.GetConstructorAccessor<TFunc>(type, parameterTypes));
        }

        public static TFunc GetConstructorAccessor<TFunc>()
        {
            var cacheKey = $"generic_constructor_{GetTypeKey<TFunc>()}";
            return GetFromCacheOrAddIfNotExists(cacheKey, ReflectlessNoCache.GetConstructorAccessor<TFunc>);
        }

        public static TFuncOrAction GetMethodAccessor<TFuncOrAction>(Type type, string name,
            params Type[] parameterTypes)
        {
            var cacheKey = $"non_generic_method_{type.FullName}_{name}_{string.Join("_", parameterTypes.Select(t => t.FullName))}";
            return GetFromCacheOrAddIfNotExists(cacheKey, () => ReflectlessNoCache.GetMethodAccessor<TFuncOrAction>(type, name, parameterTypes));
        }

        public static TFuncOrAction GetMethodAccessor<TFuncOrAction>(string name)
        {
            var cacheKey = $"generic_method_{name}_{GetTypeKey<TFuncOrAction>()}";
            return GetFromCacheOrAddIfNotExists(cacheKey, () => ReflectlessNoCache.GetMethodAccessor<TFuncOrAction>(name));
        }

        private static T GetFromCacheOrAddIfNotExists<T>(string cacheKey, Func<T> createFunction)
        {
            if (_cache.TryGetValue(cacheKey, out var cacheHit))
            {
                return (T)cacheHit;
            }

            var value = createFunction();
            if (!_cache.TryAdd(cacheKey, value!))
            {
                throw new Exception($"Unable to add cache entry for key {cacheKey}");
            }
            return value;
        }

        private static string GetTypeKey<T>()
        {
            var funcType = typeof(T);
            var typeArguments = funcType.GetGenericArguments();
            return string.Join("_", typeArguments.Select(ta => ta.FullName));
        }
    }
}
