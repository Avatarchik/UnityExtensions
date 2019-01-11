using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityExtensions.Editor;
#endif

namespace UnityExtensions
{
    /// <summary>
    /// 让 int 或 float 字段的值限制在指定范围
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class MinAttribute : PropertyAttribute
    {
        float _min;

        /// <summary>
        /// 让 int 或 float 字段的值限制在指定范围
        /// </summary>
        public MinAttribute(float min)
        {
            _min = min;
        }

#if UNITY_EDITOR

        [CustomPropertyDrawer(typeof(MinAttribute))]
        class MinDrawer : BasePropertyDrawer<MinAttribute>
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                switch (property.propertyType)
                {
                    case SerializedPropertyType.Float:
                        {
                            property.floatValue = Mathf.Max(
                                EditorGUI.FloatField(position, label, property.floatValue),
                                attribute._min);
                            break;
                        }
                    case SerializedPropertyType.Integer:
                        {
                            property.intValue = Mathf.Max(
                                EditorGUI.IntField(position, label, property.intValue),
                                (int)attribute._min);
                            break;
                        }
                    default:
                        {
                            EditorGUI.LabelField(position, label.text, "Use Min with float or int.");
                            break;
                        }
                }
            }

        } // class MinDrawer

#endif

    } // class MinAttribute

} // namespace UnityExtensions