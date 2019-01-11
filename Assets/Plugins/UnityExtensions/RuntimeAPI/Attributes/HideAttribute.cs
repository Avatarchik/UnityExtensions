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
    /// 标记在一个字段上, 根据指定的 Property 或 Field 决定是否隐藏编辑
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class HideAttribute : PropertyAttribute
    {
#if UNITY_EDITOR

        string _name;
        bool _value;

        object _fieldOrProp;
        int _result; // 0-hide, 1-show, -1-error


        public HideAttribute(string fieldOrProperty, bool hideValue)
        {
            _name = fieldOrProperty;
            _value = hideValue;
        }


        [CustomPropertyDrawer(typeof(HideAttribute))]
        class HideDrawer : BasePropertyDrawer<HideAttribute>
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

                object result = (attribute._fieldOrProp as FieldInfo)?.GetValue(property.serializedObject.targetObject);
                if (result == null) result = (attribute._fieldOrProp as PropertyInfo)?.GetValue(property.serializedObject.targetObject, null);

                if (result != null)
                {
                    if ((bool)result == attribute._value)
                    {
                        attribute._result = 0;
                        return -2f;
                    }
                    else
                    {
                        attribute._result = 1;
                        return base.GetPropertyHeight(property, label);
                    }
                }
                else
                {
                    attribute._result = -1;
                    return EditorGUIUtility.singleLineHeight;
                }
            }


            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                if (attribute._result == 1)
                {
                    base.OnGUI(position, property, label);
                }
                else if(attribute._result == -1)
                {
                    EditorGUI.LabelField(position, label.text, "Field or Property has error!");
                }
            }

        } // class HideDrawer

#else

        public HideAttribute(string fieldOrProperty, bool hideValue) { }

#endif // UNITY_EDITOR

    } // class HideAttribute

} // namespace UnityExtensions