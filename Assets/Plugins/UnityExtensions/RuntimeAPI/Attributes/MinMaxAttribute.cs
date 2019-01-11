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
    public sealed class MinMaxAttribute : PropertyAttribute
    {
        float _min;
        float _max;

        /// <summary>
        /// 让 int 或 float 字段的值限制在指定范围
        /// </summary>
        public MinMaxAttribute(float min, float max)
        {
            _min = min;
            _max = max;
        }

#if UNITY_EDITOR

        [CustomPropertyDrawer(typeof(MinMaxAttribute))]
        class MinMaxDrawer : BasePropertyDrawer<MinMaxAttribute>
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                switch (property.propertyType)
                {
                    case SerializedPropertyType.Float:
                        {
                            property.floatValue = Mathf.Clamp(
                                EditorGUI.FloatField(position, label, property.floatValue),
                                attribute._min,
                                attribute._max);
                            break;
                        }
                    case SerializedPropertyType.Integer:
                        {
                            property.intValue = Mathf.Clamp(
                                EditorGUI.IntField(position, label, property.intValue),
                                (int)attribute._min,
                                (int)attribute._max);
                            break;
                        }
                    default:
                        {
                            EditorGUI.LabelField(position, label.text, "Use MinMax with float or int.");
                            break;
                        }
                }
            }

        } // class MinMaxDrawer

#endif

    } // class MinMaxAttribute

} // namespace UnityExtensions