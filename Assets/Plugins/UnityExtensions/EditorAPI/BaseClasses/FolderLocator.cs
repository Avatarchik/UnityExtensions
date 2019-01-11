#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace UnityExtensions.Editor
{
    /// <summary>
    /// 继承 FolderLocator, 用来获取脚本所在的目录
    /// </summary>
    /// <typeparam name="Locator"> FolderLocator 的子类 </typeparam>
    public class FolderLocator<Locator> : ScriptableAsset
        where Locator : FolderLocator<Locator>
    {
        static MonoScript _script;
        static string _directory;


        /// <summary>
        /// 脚本文件所在的目录, 从 Assets 目录开始
        /// </summary>
        public static string directory
        {
            get
            {
                if (_script == null)
                {
                    var instance = CreateInstance<Locator>();
                    _script = MonoScript.FromScriptableObject(instance);
                    DestroyImmediate(instance, false);

                    _directory = AssetDatabase.GetAssetPath(_script);
                    _directory = _directory.Substring(0, _directory.LastIndexOf('/'));
                }
                return _directory;
            }
        }


        /// <summary>
        /// 加载资源, 如果已经加载过则直接返回
        /// </summary>
        /// <param name="reference"> 用来保存资源对象的引用 </param>
        /// <param name="subpath"> 资源相对于当前目录的子路径 </param>
        public static T LoadAsset<T>(ref T reference, string subpath)
            where T : Object
        {
            if (reference == null)
            {
                reference = AssetDatabase.LoadAssetAtPath<T>(directory + '/' + subpath);
            }
            return reference;
        }

    } // class FolderLocator

} // namespace UnityExtensions.Editor

#endif // UNITY_EDITOR