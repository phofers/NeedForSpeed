using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using BepInEx.Configuration;
using System.Collections.Generic;
using UnityEngine.Windows.Speech;

namespace NeedForSpeed;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInProcess("Ages of Conflict.exe")]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
    public static ConfigEntry<bool> configUpToTwoThousandSpeedEnabled;

    // if anyone reading this so much as questions at ALL the list/array stuff happening here, I will have your head. 
    // it's great because... eventually this will be a config entry? I think? I don't want to do that right now
    public static List<float> speedOptionsList = new List<float>
    {
        0.1f, 0.25f, 0.5f, 0.75f, 1f, 1.5f, 2f, 3f, 5f, 10f, 15f, 20f, 25f, 30f, 35f, 40f, 45f, 50f, 55f, 60f, 65f, 70f, 75f, 80f, 85f, 90f, 95f, 100f
    };

    private void Awake()
    {
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        new Harmony("NeedForSpeed").PatchAll();

        configUpToTwoThousandSpeedEnabled = Config.Bind(
                "General", "UpToTwoThousandSpeedEnabled", false, "Are speeds greater than 100x, up to 2000x, enabled?");

        if (configUpToTwoThousandSpeedEnabled.Value == true)
        {
            speedOptionsList.AddRange(new List<float> { 200f, 250f, 300f, 500f, 1000f, 1500f, 2000f });
        }
    }

    


    [HarmonyPatch(typeof(SpeedAdjust), "Start")]
    private static class SpeedAdjustPatch
    {
        private static void Postfix(SpeedAdjust __instance)
        {
            float[] speedOptions = speedOptionsList.ToArray();
            // still just the arrows- maybe a UI box to enter your speed as well in the future??????
            // TODO: these speeds are rounded in unfun ways because of other stuff that isn't yet patched in this class
            typeof(SpeedAdjust)
                .GetField("speedOptions", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.SetValue(__instance, speedOptions);
            
                
        }
    }
}
