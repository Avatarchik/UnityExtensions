using System;
using System.Reflection;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityExtensions.Editor;
#endif

namespace UnityExtensions
{
    /// <summary>
    /// 标记在一个字段上, 根据指定的 Property 或 Field 决定是否禁用编辑
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class DisableAttribute : PropertyAttribute
    {
#if UNITY_EDITOR

        string _name;
        bool _value;

        object _fieldOrProp;


        public DisableAttribute(string fieldOrProperty, bool disableValue)
        {
            _name = fieldOrProperty;
            _value = disableValue;
        }


        [CustomPropertyDrawer(typeof(DisableAttribute))]
        class DisableDrawer : BasePropertyDrawer<DisableAttribute>
        {
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                if (attribute._fieldOrProp == null)
                {
                    var field = ReflectionKit.GetFieldInfo(property.serializedObject.targetObject, attribute._name);
                    if (field?.FieldType == typeof(bool))
                    {
                        attribute._fieldOrProp = field;
                    }
                    else
                    {
                        var prop = ReflectionKit.GetPropertyInfo(property.serializedObject.targetObject, attribute._name);
                        if (prop?.PropertyType == typeof(bool) && prop.CanRead)
                        {
                            attribute._fieldOrProp = prop;
                        }
                    }
                }

                return attribute._fieldOrProp == null ? EditorGUIUtility.singleLineHeight : base.GetPropertyHeight(property, label);
            }


            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                object result = (attribute._fieldOrProp as FieldInfo)?.GetValue(property.serializedObject.targetObject);
                if (result == null) result = (attribute._fieldOrProp as PropertyInfo)?.GetValue(property.serializedObject.targetObject, null);

                if (result != null)
                {
                    using (new DisabledScope((bool)result == attribute._value))
                    {
                        base.OnGUI(position, property, label);
                    }
                }
                else
                {
                    EditorGUI.LabelField(position, label.text, "Field or Property has error!");
                }
            }

        } // class DisableDrawer

#else

        public DisableAttribute(string fieldOrProperty, bool disableValue) { }

#endif // UNITY_EDITOR

    } // class DisableAttribute

} // namespace UnityExtensions