using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityExtension.Editor;
#endif

namespace UnityExtension
{
    /// <summary>
    /// ScriptableAssetSingleton
    /// 在编辑器和运行时提供访问资源单例的方法
    /// 如果未曾创建资源，或 Build 后不包含资源，访问时会创建一个临时对象
    /// 注意：没有被场景引用, 且 没有加入 Preloaded Assets 列表 且 不在 Resources 里的资源在 Build 时会被忽略
    /// </summary>
    public class ScriptableAssetSingleton<T> : ScriptableAsset where T : ScriptableAssetSingleton<T>
    {
        static T _instance;


        /// <summary>
        /// 访问单例
        /// </summary>
        public static T instance
        {
            get
            {
                if (!_instance)
                {
#if UNITY_EDITOR
                    _instance = EditorUtilities.FindAsset<T>();
                    if (!_instance)
                    {
                        _instance = CreateInstance<T>();
                        Debug.LogWarning(string.Format("No asset of {0} loaded, a temporary instance was created. Use {0}.CreateOrSelectAsset to create an asset.", typeof(T).Name));
                    }
#else
                    _instance = CreateInstance<T>();
                    Debug.LogWarning(string.Format("No asset of {0} loaded, a temporary instance was created. Do you forget to add the asset to \"Preloaded Assets\" list?", typeof(T).Name));
#endif
                }
                return _instance;
            }
        }


        protected ScriptableAssetSingleton()
        {
            _instance = this as T;
        }


#if UNITY_EDITOR

        /// <summary>
        /// 创建单例资源, 如果已经存在则选中该资源
        /// </summary>
        public static void CreateOrSelectAsset()
        {
            bool needCreate = false;

            if (!_instance)
            {
                _instance = EditorUtilities.FindAsset<T>();
                if (!_instance)
                {
                    _instance = CreateInstance<T>();
                    needCreate = true;
                }
            }
            else needCreate = !AssetDatabase.IsNativeAsset(_instance);

            if (needCreate)
            {
                EditorUtilities.CreateAsset(_instance);
            }

            Selection.activeObject = instance;
        }

#endif

    } // class ScriptableAssetSingleton

} // namespace UnityExtension