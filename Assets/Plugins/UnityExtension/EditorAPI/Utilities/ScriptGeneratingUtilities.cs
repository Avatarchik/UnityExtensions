#if UNITY_EDITOR

using System;
using System.IO;
using System.Collections.Generic;
using UnityEditor;

namespace UnityExtension.Editor
{
    public partial struct EditorUtilities
    {
        static void CreateScript(string className, Action<StreamWriter> write)
        {
            string path = string.Format("{0}/{1}.cs", activeDirectory, className);

            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                using (var writer = new StreamWriter(stream))
                {
                    write(writer);
                }
            }

            AssetDatabase.Refresh();
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<MonoScript>(path);
        }


        public static void CreateMaskScript(string namespaceName, string className, string contentName, Func<int, string> bitToName)
        {
            CreateScript(className, writer =>
            {
                writer.Write(string.Format(
@"
namespace {0}
{{
    /// <summary> Constants of {1}. </summary>
    public struct {2}
    {{", namespaceName, contentName, className));

                List<string> list = new List<string>(32);

                for (int i = 0; i < 32; i++)
                {
                    var name = bitToName(i);

                    writer.Write(string.Format(
        @"
        /// <summary> {0} {1}, Name: {2} </summary>
        public const int {3} = {1};
"                        , contentName, i, string.IsNullOrEmpty(name) ? "(none)" : name, GetVariableName(name, contentName+i, list)));
                }

                writer.Write(string.Format(
        @"

        /// <summary> Constants of Mask of {0}. </summary>
        public struct Masks
        {{", contentName));

                for (int i = 0; i < 32; i++)
                {
                    var name = bitToName(i);

                    writer.Write(string.Format(
            @"
            /// <summary> Mask of {0} {1}, Name: {2} </summary>
            public const int {3} = {4};
"                        , contentName, i, string.IsNullOrEmpty(name) ? "(none)" : name, list[i], 1 << i));
                }

                writer.Write(
@"        }
    }
}"                    );
            });
        }


        // 创建场景脚本, depth 指生成变量名时, 从场景文件开始向上保留的文件夹数量
        public static void CreateSceneScript(int depth)
        {
            CreateScript("Scenes", writer =>
            {
                writer.Write(
@"
namespace UnityExtension
{
    /// <summary> Constants of Scenes. </summary>
    public struct Scenes
    {");

                var scenes = EditorBuildSettings.scenes;
                List<string> list = new List<string>(scenes.Length);

                for (int i = 0; i < scenes.Length; i++)
                {
                    var name = GetSubpath(scenes[i].path, depth, true);

                    writer.Write(string.Format(
        @"
        /// <summary> Scene {0}, Path: {1} </summary>
        public const int {2} = {0};
"                        , i, scenes[i].path, GetVariableName(name, "Scene"+i, list)));
                }

                writer.Write(
@"    }
}"                    );
            });
        }


        // 字符串转化为有效的变量名
        // 仅支持下划线，英文字母 和 数字，其他字符被忽略, 并且在 list 中保持唯一
        // 如果无法获取有效的变量名，则使用 fallbackName 作为变量名
        static string GetVariableName(string originalName, string fallbackName, List<string> list)
        {
            var chars = new List<char>(originalName);
            bool invalid;
            bool upper = true;

            for (int i = 0; i < chars.Count; i++)
            {
                var c = chars[i];

                if (i == 0) invalid = (c != '_' && !Utilities.IsEnglishLetter(c));
                else invalid = (c != '_' && !Utilities.IsDigit(c) && !Utilities.IsEnglishLetter(c));

                if (invalid)
                {
                    chars.RemoveAt(i--);
                    upper = true;
                }
                else if (upper)
                {
                    if (Utilities.IsEnglishLower(c))
                    {
                        chars[i] = (char)(c + 'A' - 'a');
                    }
                    upper = false;
                }
            }

            originalName = new string(chars.ToArray());
            if (originalName.Length == 0 || list.Contains(originalName))
            {
                originalName = fallbackName;
            }

            list.Add(originalName);
            return originalName;
        }


        // 将一个有效路径从尾部开始向前保留限定的层级
        static string GetSubpath(string path, int depth, bool removeExtension)
        {
            if (removeExtension)
            {
                path = path.Substring(0, path.LastIndexOf('.'));
            }

            for (int i = path.Length - 1; i >= 0; i--)
            {
                if (path[i] == '/')
                {
                    if (depth <= 0) return path.Substring(i + 1);
                    else depth--;
                }
            }

            return path;
        }

    } // struct EditorUtilities

} // namespace UnityExtension.Editor

#endif // UNITY_EDITOR