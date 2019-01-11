using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityExtensions.Editor;
#endif

namespace UnityExtensions
{
    /// <summary>
    /// 标记在一个 int 字段上, 使其作为 Layer 显示
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class LayerAttribute : PropertyAttribute
    {

#if UNITY_EDITOR

        [CustomPropertyDrawer(typeof(LayerAttribute))]
        class LayerDrawer : BasePropertyDrawer<LayerAttribute>
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                if (property.propertyType == SerializedPropertyType.Integer)
                {
                    property.intValue = EditorGUI.LayerField(position, label, property.intValue);
                }
                else EditorGUI.LabelField(position, label, "Use LayerAttribute with int.");
            }

        } // class LayerDrawer

#endif // UNITY_EDITOR

    } // class LayerAttribute

} // namespace UnityExtensions