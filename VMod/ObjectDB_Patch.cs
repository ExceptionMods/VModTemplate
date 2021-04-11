using HarmonyLib;

namespace VMod
{
    [HarmonyPatch(typeof(ObjectDB), "CopyOtherDB")]
    public static class ObjectDB_CopyOtherDB_Patch
    {
        public static void Postfix()
        {
            VModPlugin.TryRegisterItems();
            VModPlugin.TryRegisterStatusEffects();
        }
    }

    [HarmonyPatch(typeof(ObjectDB), "Awake")]
    public static class ObjectDB_Awake_Patch
    {
        public static void Postfix()
        {
            VModPlugin.TryRegisterItems();
            VModPlugin.TryRegisterStatusEffects();
        }
    }
}
