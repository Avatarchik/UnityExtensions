using System;
using System.Reflection;

namespace UnityExtension
{
    /// <summary>
    /// Reflection 实用工具
    /// </summary>
    public partial struct Utilities
    {
        /// <summary>
        /// 获取对象的成员字段信息。从最终类型开始向上查找，忽略字段的可见性。
        /// </summary>
        public static FieldInfo GetFieldInfo(object instance, string fieldName)
        {
            Type type = instance.GetType();
            FieldInfo info = null;
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

            while(type != null)
            {
                info = type.GetField(fieldName, flags);
                if (info != null) return info;
                type = type.BaseType;
            }

            return null;
        }


        /// <summary>
        /// 获取类型的静态字段信息。从最终类型开始向上查找，忽略字段的可见性。
        /// </summary>
        public static FieldInfo GetFieldInfo<T>(string fieldName)
        {
            Type type = typeof(T);
            FieldInfo info = null;
            BindingFlags flags = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;

            while (type != null)
            {
                info = type.GetField(fieldName, flags);
                if (info != null) return info;
                type = type.BaseType;
            }

            return null;
        }


        /// <summary>
        /// 获取对象的成员属性信息。从最终类型开始向上查找，忽略属性的可见性。
        /// </summary>
        public static PropertyInfo GetPropertyInfo(object instance, string propertyName)
        {
            Type type = instance.GetType();
            PropertyInfo info = null;
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

            while (type != null)
            {
                info = type.GetProperty(propertyName, flags);
                if (info != null) return info;
                type = type.BaseType;
            }

            return null;
        }


        /// <summary>
        /// 获取类型的静态属性信息。从最终类型开始向上查找，忽略属性的可见性。
        /// </summary>
        public static PropertyInfo GetPropertyInfo<T>(string propertyName)
        {
            Type type = typeof(T);
            PropertyInfo info = null;
            BindingFlags flags = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;

            while (type != null)
            {
                info = type.GetProperty(propertyName, flags);
                if (info != null) return info;
                type = type.BaseType;
            }

            return null;
        }


        /// <summary>
        /// 获取对象的成员方法信息。从最终类型开始向上查找，忽略方法的可见性。
        /// </summary>
        public static MethodInfo GetMethodInfo(object instance, string methodName)
        {
            Type type = instance.GetType();
            MethodInfo info = null;
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

            while (type != null)
            {
                info = type.GetMethod(methodName, flags);
                if (info != null) return info;
                type = type.BaseType;
            }

            return null;
        }


        /// <summary>
        /// 获取类型的静态方法信息。从最终类型开始向上查找，忽略方法的可见性。
        /// </summary>
        public static MethodInfo GetMethodInfo<T>(string methodName)
        {
            Type type = typeof(T);
            MethodInfo info = null;
            BindingFlags flags = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;

            while (type != null)
            {
                info = type.GetMethod(methodName, flags);
                if (info != null) return info;
                type = type.BaseType;
            }

            return null;
        }


        /// <summary>
        /// 获取对象的成员字段值。从最终类型开始向上查找，忽略字段的可见性。
        /// </summary>
        public static object GetFieldValue(object instance, string fieldName)
        {
            return GetFieldInfo(instance, fieldName).GetValue(instance);
        }


        /// <summary>
        /// 获取类型的静态字段值。从最终类型开始向上查找，忽略字段的可见性。
        /// </summary>
        public static object GetFieldValue<T>(string fieldName)
        {
            return GetFieldInfo<T>(fieldName).GetValue(null);
        }


        /// <summary>
        /// 设置对象的成员字段值。从最终类型开始向上查找，忽略字段的可见性。
        /// </summary>
        public static void SetFieldValue(object instance, string fieldName, object value)
        {
            GetFieldInfo(instance, fieldName).SetValue(instance, value);
        }


        /// <summary>
        /// 设置类型的静态字段值。从最终类型开始向上查找，忽略字段的可见性。
        /// </summary>
        public static void SetFieldValue<T>(string fieldName, object value)
        {
            GetFieldInfo<T>(fieldName).SetValue(null, value);
        }


        /// <summary>
        /// 获取对象的成员属性值。从最终类型开始向上查找，忽略属性的可见性。
        /// </summary>
        public static object GetPropertyValue(object instance, string propertyName)
        {
            return GetPropertyInfo(instance, propertyName).GetValue(instance, null); 
        }


        /// <summary>
        /// 获取类型的静态属性值。从最终类型开始向上查找，忽略属性的可见性。
        /// </summary>
        public static object GetPropertyValue<T>(string propertyName)
        {
            return GetPropertyInfo<T>(propertyName).GetValue(null, null);
        }


        /// <summary>
        /// 设置对象的成员属性值。从最终类型开始向上查找，忽略属性的可见性。
        /// </summary>
        public static void SetPropertyValue(object instance, string propertyName, object value)
        {
            GetPropertyInfo(instance, propertyName).SetValue(instance, value, null);
        }


        /// <summary>
        /// 设置类型的静态属性值。从最终类型开始向上查找，忽略属性的可见性。
        /// </summary>
        public static void SetPropertyValue<T>(string propertyName, object value)
        {
            GetPropertyInfo<T>(propertyName).SetValue(null, value, null);
        }


        /// <summary>
        /// 调用对象的成员方法。从最终类型开始向上查找，忽略方法的可见性。
        /// </summary>
        public static object InvokeMethod(object instance, string methodName, params object[] param)
        {
            return GetMethodInfo(instance, methodName).Invoke(instance, param);
        }


        /// <summary>
        /// 调用类型的静态方法。从最终类型开始向上查找，忽略方法的可见性。
        /// </summary>
        public static object InvokeMethod<T>(string methodName, params object[] param)
        {
            return GetMethodInfo<T>(methodName).Invoke(null, param);
        }

    } // struct Utilities

} // namespace UnityExtension