using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityExtension.Editor;
#endif

namespace UnityExtension
{
    /// <summary>
    /// 标记在 Quaternion 字段上, 将其显示为欧拉角
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class EulerAnglesAttribute : PropertyAttribute
    {

#if UNITY_EDITOR

        Quaternion _quaternion = Quaternion.identity;
        Vector3 _eulerAngles = Vector3.zero;


        [CustomPropertyDrawer(typeof(EulerAnglesAttribute))]
        class EulerAnglesDrawer : BasePropertyDrawer<EulerAnglesAttribute>
        {
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                if (EditorGUIUtility.wideMode)
                {
                    return EditorGUIUtility.singleLineHeight;
                }
                else
                {
                    return EditorGUIUtility.singleLineHeight * 2;
                }
            }


            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                if (attribute._quaternion != property.quaternionValue)
                {
                    property.quaternionValue = attribute._quaternion = property.quaternionValue.normalized;
                    attribute._eulerAngles = attribute._quaternion.eulerAngles;
                }

                EditorGUI.BeginChangeCheck();
                attribute._eulerAngles = EditorGUI.Vector3Field(position, label, attribute._eulerAngles);
                if (EditorGUI.EndChangeCheck())
                {
                    property.quaternionValue = attribute._quaternion = Quaternion.Euler(attribute._eulerAngles);
                }
            }

        } // class EulerAnglesDrawer

#endif // UNITY_EDITOR

    } // class EulerAnglesAttribute

} // namespace UnityExtension