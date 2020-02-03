using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomEvents.HarmonyPatches
{
    [HarmonyPatch(typeof(BeatmapObjectCallbackController), "SetNewBeatmapData")]
    class BeatmapObjectCallbackControllerSetNewBeatmapData
    {
        public static void Postfix(BeatmapObjectCallbackController __instance, BeatmapData beatmapData)
        {
            __instance.GetComponent<CustomEventCallbackController>()?.SetNewBeatmapData(beatmapData);
        }
    }
}
