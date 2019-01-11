using UnityEngine;
using UnityEditor;

namespace UnityExtensions.Editor
{
    struct ScriptGeneratingCommands
    {
        [MenuItem("Assets/Create/Unity Extensions/Layers Script", priority = 1000)]
        static void CreateLayersScript()
        {
            ScriptGeneratingKit.CreateMaskScript("UnityExtensions", "Layers", "Layer", LayerMask.LayerToName);
        }


        [MenuItem("Assets/Create/Unity Extensions/Scenes Script (File Name)", priority = 1000)]
        static void CreateScenesScriptFileName()
        {
            ScriptGeneratingKit.CreateSceneScript(0);
        }


        [MenuItem("Assets/Create/Unity Extensions/Scenes Script (Folder + File Name)", priority = 1000)]
        static void CreateScenesScriptFolderFileName()
        {
            ScriptGeneratingKit.CreateSceneScript(1);
        }

    } // struct ScriptGeneratingCommands

} // namespace UnityExtensions.Editor
