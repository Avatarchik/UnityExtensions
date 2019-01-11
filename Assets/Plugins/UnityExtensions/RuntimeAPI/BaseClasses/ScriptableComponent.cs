using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityExtensions.Editor;
#endif

namespace UnityExtensions
{
    /// <summary>
    /// ScriptableComponent
    /// </summary>
    public class ScriptableComponent : MonoBehaviour
    {

#if UNITY_EDITOR

        /// <summary>
        /// 自定义 Inspector 示例
        /// </summary>
        [CustomEditor(typeof(ScriptableComponent))]
        [CanEditMultipleObjects]
        class ScriptableComponentEditor : BaseEditor<ScriptableComponent>
        {
            SerializedProperty _someProp;


            void OnEnable()
            {
                _someProp = serializedObject.FindProperty("fieldName");
            }


            public override void OnInspectorGUI()
            {
                serializedObject.Update();

                EditorGUILayout.PropertyField(_someProp);
                if (!_someProp.hasMultipleDifferentValues)
                {
                }

                serializedObject.ApplyModifiedProperties();

                using (var scope = new ChangeCheckScope(target))
                {
                    bool enabled = EditorGUILayout.Toggle("Enabled", target.enabled);
                    if (scope.changed) target.enabled = enabled;
                }
            }


            void OnSceneGUI()
            {
            }
        }

#endif // UNITY_EDITOR

    } // class ScriptableComponent

} // namespace UnityExtensions