using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;

namespace NeedForSpeed;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInProcess("Ages of Conflict.exe")]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;

    private void Awake()
    {
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        new Harmony("NeedForSpeed").PatchAll();
    }

    [HarmonyPatch(typeof(SpeedAdjust), "Start")]
    private static class SpeedAdjustPatch
    {
        private static void Postfix(SpeedAdjust __instance)
        {
            // still just the arrows- maybe a UI box to enter your speed as well in the future??????
            // TODO: these speeds are rounded in unfun ways because of other stuff that isn't yet patched in this method
            typeof(SpeedAdjust)
                .GetField("speedOptions", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.SetValue(__instance, new float[] {
                    0.1f, 0.25f, 0.5f, 0.75f, 1f, 1.5f, 2f, 3f, 5f, 10f, 15f, 20f, 25f, 30f, 35f, 40f, 45f, 50f, 55f, 60f, 65f, 70f, 75f, 80f, 85f, 90f, 95f, 100f, 150f, 200f, 250f, 500f, 1000f, 2000f
                });
        }
    }
}
