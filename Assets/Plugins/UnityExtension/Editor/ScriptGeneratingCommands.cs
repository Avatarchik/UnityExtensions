using UnityEngine;
using UnityEditor;

namespace UnityExtension.Editor
{
    struct ScriptGeneratingCommands
    {
        [MenuItem("Assets/Create/Unity Extension/Layers Script", priority = 1000)]
        static void CreateLayersScript()
        {
            EditorUtilities.CreateMaskScript("UnityExtension", "Layers", "Layer", LayerMask.LayerToName);
        }


        [MenuItem("Assets/Create/Unity Extension/Scenes Script (File Name)", priority = 1000)]
        static void CreateScenesScriptFileName()
        {
            EditorUtilities.CreateSceneScript(0);
        }


        [MenuItem("Assets/Create/Unity Extension/Scenes Script (Folder + File Name)", priority = 1000)]
        static void CreateScenesScriptFolderFileName()
        {
            EditorUtilities.CreateSceneScript(1);
        }

    } // struct ScriptGeneratingCommands

} // namespace UnityExtension.Editor
