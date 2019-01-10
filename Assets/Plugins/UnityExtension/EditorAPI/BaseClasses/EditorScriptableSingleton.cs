#if UNITY_EDITOR

namespace UnityExtension
{
    /// <summary>
    /// 仅用于编辑器的单例类型，可以用来暂存编辑器数据
    /// </summary>
    public class EditorScriptableSingleton<T> : ScriptableAsset where T : EditorScriptableSingleton<T>
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
                    _instance = FindObjectOfType<T>();
                    if (!_instance)
                    {
                        _instance = CreateInstance<T>();
                    }
                }
                return _instance;
            }
        }

    } // class EditorScriptableSingleton

} // namespace UnityExtension

#endif