#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace UnityExtension.Editor
{
    /// <summary>
    /// BasePropertyDrawer
    /// </summary>
    public class BasePropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, property.hasVisibleChildren);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label, property.hasVisibleChildren);
        }

    } // class BasePropertyDrawer


    /// <summary>
    /// BasePropertyDrawer<T>
    /// </summary>
    public class BasePropertyDrawer<T> : BasePropertyDrawer where T : PropertyAttribute
    {
        protected new T attribute => base.attribute as T;

    } // class BasePropertyDrawer<T>

} // namespace UnityExtension.Editor

#endif // UNITY_EDITOR