using BepInEx;
using BepInEx.Logging;
using System.Collections.Generic;
using System.IO;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace VMod
{
    // ExceptionMods.
    // https://github.com/ExceptionMods

    [BepInPlugin("org.bepinex.plugins.vmod", "VMod", version)]
    public class VModPlugin : BaseUnityPlugin
    {
        public const string PluginId = "mod.vmod";
        public const string version = "0.0.1";

        public static Harmony harmony = new Harmony(PluginId);

        public static string Repository = "https://github.com/VMod/VMod";
        public static string ApiRepository = "https://api.github.com/repos/VMod/VMod/tags";
        public static ManualLogSource VModLogger;

        private Harmony _harmony;
        public static readonly Dictionary<string, GameObject> Prefabs = new Dictionary<string, GameObject>();
        public static readonly Dictionary<string, StatusEffect> StatusEffects = new Dictionary<string, StatusEffect>();

        // Awake is called once when both the game and the plug-in are loaded
        void Awake()
        {
            VModLogger = Logger;

            _harmony = new Harmony(PluginId);
            _harmony.PatchAll();
        }
        
        private void OnGUI()
        {
        }
        private void OnDestroy()
        {
            _harmony?.UnpatchAll(PluginId);
            foreach (var prefab in Prefabs.Values)
            {
                Destroy(prefab);
            }
            foreach (var script in StatusEffects.Values)
            {
                Destroy(script);
            }

            Prefabs.Clear();
            StatusEffects.Clear();
        }

        public static void TryRegisterPrefabs(ZNetScene zNetScene)
        {
            if (zNetScene == null)
            {
                return;
            }

            foreach (var prefab in Prefabs.Values)
            {
                zNetScene.m_prefabs.Add(prefab);
            }
        }

        public static void TryRegisterItems()
        {
            if (ObjectDB.instance == null || ObjectDB.instance.m_items.Count == 0)
            {
                return;
            }

            foreach (var prefab in Prefabs.Values)
            {
                var itemDrop = prefab.GetComponent<ItemDrop>();
                if (itemDrop != null)
                {
                    if (ObjectDB.instance.GetItemPrefab(prefab.name.GetStableHashCode()) == null)
                    {
                        ObjectDB.instance.m_items.Add(prefab);
                    }
                }
            }
        }

        public static void TryRegisterStatusEffects()
        {
            if (ObjectDB.instance == null || ObjectDB.instance.m_items.Count == 0)
            {
                return;
            }

            foreach (var statusEffect in StatusEffects.Values)
            {
                if (ObjectDB.instance.GetStatusEffect(statusEffect.m_name) == null)
                {
                    ObjectDB.instance.m_StatusEffects.Add(statusEffect);
                }
            }
        }

        private static string GetAssetPath(string assetName)
        {
            var assetFileName = Path.Combine(Paths.PluginPath, assetName);
            if (!File.Exists(assetFileName))
            {
                Assembly assembly = typeof(VModPlugin).Assembly;
                assetFileName = Path.Combine(Path.GetDirectoryName(assembly.Location), assetName);
                if (!File.Exists(assetFileName))
                {
                    Debug.LogError($"Could not find asset ({assetName})");
                    return null;
                }
            }

            return assetFileName;
        }
    }
}
