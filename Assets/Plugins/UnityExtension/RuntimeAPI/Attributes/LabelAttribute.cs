using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityExtension.Editor;
#endif

namespace UnityExtension
{
    /// <summary>
    /// 标记在一个字段上, 可以替换显示的 Label
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class LabelAttribute : PropertyAttribute
    {
        string _label;

        public LabelAttribute(string label)
        {
            _label = label;
        }

#if UNITY_EDITOR

        [CustomPropertyDrawer(typeof(LabelAttribute))]
        class LabelDrawer : BasePropertyDrawer<LabelAttribute>
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                label.text = attribute._label;
                base.OnGUI(position, property, label);
            }
        }

#endif // UNITY_EDITOR

    } // class LabelAttribute

} // namespace UnityExtension