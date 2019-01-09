using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityExtension.Editor;
#endif

namespace UnityExtension
{
    /// <summary>
    /// 让 Vector3 代表方向向量, Inspector 上显示的是 XY 轴的欧拉角
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class DirectionAttribute : PropertyAttribute
    {
#if UNITY_EDITOR

        float _length;
        Vector3 _direction;
        Vector2 _eulerAngles;

        public DirectionAttribute(float length = 1f)
        {
            _length = Mathf.Clamp(length, Utilities.OneMillionth, Utilities.Million);
            _direction = new Vector3(0, 0, _length);
            _eulerAngles = new Vector2();
        }

        [CustomPropertyDrawer(typeof(DirectionAttribute))]
        class DirectionDrawer : BasePropertyDrawer<DirectionAttribute>
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                var vector3 = property.vector3Value;

                if (attribute._direction != vector3)
                {
                    if (vector3.magnitude < Utilities.OneMillionth)
                    {
                        property.vector3Value = vector3 = attribute._direction;
                    }
                    else
                    {
                        property.vector3Value = vector3 = vector3 / vector3.magnitude * attribute._length;
                        attribute._direction = vector3;
                        attribute._eulerAngles = Quaternion.LookRotation(vector3).eulerAngles;
                    }
                }

                EditorGUI.BeginChangeCheck();
                attribute._eulerAngles = EditorGUI.Vector2Field(position, label, attribute._eulerAngles);
                if (EditorGUI.EndChangeCheck())
                {
                    property.vector3Value = attribute._direction = Quaternion.Euler(attribute._eulerAngles) * new Vector3(0, 0, attribute._length);
                }
            }

        } // class DirectionDrawer
#else

        public DirectionAttribute(float length = 1f) { }

#endif

    } // class DirectionAttribute

} // namespace UnityExtension