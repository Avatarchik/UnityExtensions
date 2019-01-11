using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityExtensions.Editor;
#endif

namespace UnityExtensions
{
    /// <summary>
    /// ScriptableAsset
    /// </summary>
    public class ScriptableAsset : ScriptableObject
    {

#if UNITY_EDITOR

        /// <summary>
        /// 自定义 Inspector 示例
        /// </summary>
        [CustomEditor(typeof(ScriptableAsset))]
        [CanEditMultipleObjects]
        class ScriptableAssetEditor : BaseEditor<ScriptableAsset>
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
                    string name = EditorGUILayout.TextField("Name", target.name);
                    if (scope.changed) target.name = name;
                }
            }
        }

#endif // UNITY_EDITOR

    } // class ScriptableAsset

} // namespace UnityExtensions