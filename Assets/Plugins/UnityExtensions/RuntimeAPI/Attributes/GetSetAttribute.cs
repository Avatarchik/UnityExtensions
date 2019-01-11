using System.Reflection;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityExtensions.Editor;
#endif

namespace UnityExtensions
{
    /// <summary>
    /// 使用属性替换字段显示和编辑
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class GetSetAttribute : PropertyAttribute
    {
#if UNITY_EDITOR

        string _propertyName;
        string _label;

        Object _target;
        PropertyInfo _propertyInfo;


        public GetSetAttribute(string property, string label = null)
        {
            _propertyName = property;
            _label = label;
        }


        [CustomPropertyDrawer(typeof(GetSetAttribute))]
        sealed class GetSetDrawer : BasePropertyDrawer<GetSetAttribute>
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                if (attribute._label != null) label.text = attribute._label;

                if (attribute._target == null)
                {
                    attribute._target = property.serializedObject.targetObject;
                    attribute._propertyInfo = ReflectionKit.GetPropertyInfo(attribute._target, attribute._propertyName);
                }

                if (attribute._propertyInfo == null)
                {
                    EditorGUI.LabelField(position, label.text, "Can't find property");
                    return;
                }

                if (fieldInfo.FieldType != attribute._propertyInfo.PropertyType)
                {
                    EditorGUI.LabelField(position, label.text, "Mismatching property type");
                    return;
                }

                if (!attribute._propertyInfo.CanRead || !attribute._propertyInfo.CanWrite)
                {
                    EditorGUI.LabelField(position, label.text, "Property can't read or write");
                    return;
                }

                using (var scope = new ChangeCheckScope(property.serializedObject.targetObject))
                {
                    object value = attribute._propertyInfo.GetValue(attribute._target, null);

                    switch (property.propertyType)
                    {
                        case SerializedPropertyType.AnimationCurve:
                            {
                                value = EditorGUI.CurveField(position, label, (AnimationCurve)value);
                                break;
                            }
                        case SerializedPropertyType.Boolean:
                            {
                                value = EditorGUI.Toggle(position, label, (bool)value);
                                break;
                            }
                        case SerializedPropertyType.Bounds:
                            {
                                value = EditorGUI.BoundsField(position, label, (Bounds)value);
                                break;
                            }
                        case SerializedPropertyType.Color:
                            {
                                value = EditorGUI.ColorField(position, label, (Color)value);
                                break;
                            }
                        case SerializedPropertyType.Enum:
                            {
                                value = EditorGUI.EnumPopup(position, label, (System.Enum)value);
                                break;
                            }
                        case SerializedPropertyType.Float:
                            {
                                value = EditorGUI.FloatField(position, label, (float)value);
                                break;
                            }
                        case SerializedPropertyType.Integer:
                            {
                                value = EditorGUI.IntField(position, label, (int)value);
                                break;
                            }
                        case SerializedPropertyType.ObjectReference:
                            {
                                value = EditorGUI.ObjectField(position, label, value as Object, fieldInfo.FieldType, !EditorUtility.IsPersistent(attribute._target));
                                break;
                            }
                        case SerializedPropertyType.Rect:
                            {
                                value = EditorGUI.RectField(position, label, (Rect)value);
                                break;
                            }
                        case SerializedPropertyType.String:
                            {
                                value = EditorGUI.TextField(position, label, (string)value);
                                break;
                            }
                        case SerializedPropertyType.Vector2:
                            {
                                value = EditorGUI.Vector2Field(position, label, (Vector2)value);
                                break;
                            }
                        case SerializedPropertyType.Vector3:
                            {
                                value = EditorGUI.Vector3Field(position, label, (Vector3)value);
                                break;
                            }
                        case SerializedPropertyType.Vector4:
                            {
                                value = EditorGUI.Vector4Field(position, label.text, (Vector4)value);
                                break;
                            }
                        default:
                            {
                                EditorGUI.LabelField(position, label.text, "Type is not supported");
                                break;
                            }
                    }

                    if (scope.changed) attribute._propertyInfo.SetValue(attribute._target, value, null);
                }

            } // OnGUI

        } // class GetSetDrawer

#else

        public GetSetAttribute(string propertyName, string label = null) { }

#endif // UNITY_EDITOR

    } // class GetSetAttribute

} // namespace UnityExtensions