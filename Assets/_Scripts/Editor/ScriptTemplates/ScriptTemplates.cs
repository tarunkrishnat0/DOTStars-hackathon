using System.Reflection;
using UnityEditor;

namespace WaynGroup.Mgm.Ability.Editor
{
    internal class ScriptTemplates
    {
        [MenuItem("Assets/Create/DOTS/Unmanaged System", priority = -1)]
        public static void CreateUnmanagedSystem()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
                $"Assets/_Scripts/Editor/ScriptTemplates/UnmanagedSystem.txt",
                "UnmanagedSystem.cs");
        }

        [MenuItem("Assets/Create/DOTS/Authoring Component")]
        public static void CreateAuthoringComponent()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
                $"Assets/_Scripts/Editor/ScriptTemplates/AuthoringComponent.txt",
                "AuthoringComponent.cs");
        }

        [MenuItem("Assets/Create/DOTS/IComponentData")]
        public static void CreateIComponentData()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
                $"Assets/_Scripts/Editor/ScriptTemplates/IComponentData.txt",
                "IComponentData.cs");
        }
        [MenuItem("Assets/Create/DOTS/IBufferElementData")]
        public static void CreateIBufferElementData()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
                $"Assets/_Scripts/Editor/ScriptTemplates/IBufferElementData.txt",
                "IBufferElementData.cs");
        }
        [MenuItem("Assets/Create/DOTS/Hybrid Component")]
        public static void CreateHybridComponent()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
                $"Assets/_Scripts/Editor/ScriptTemplates/HybridComponent.txt",
                "HybridComponent.cs");
        }

    }
}