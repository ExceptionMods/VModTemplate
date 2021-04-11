using HarmonyLib;

namespace VMod
{
    [HarmonyPatch(typeof(ZNetScene), "Awake")]
    public static class ZNetScene_Awake_Patch
    {
        public static bool Prefix(ZNetScene __instance)
        {
            VModPlugin.TryRegisterPrefabs(__instance);
            return true;
        }
    }
}
