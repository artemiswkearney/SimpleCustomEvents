using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomEvents.HarmonyPatches
{
    [HarmonyPatch(typeof(BeatmapObjectSpawnController), "EarlyEventsWereProcessed")]
    class BeatmapObjectCallbackControllerEarlyEventsWereProcessed
    {
        public static void Postfix(BeatmapObjectCallbackController __instance, BeatmapObjectSpawnMovementData ____beatmapObjectSpawnMovementData)
        {
            CustomEventCallbackController.spawnAheadTime = ____beatmapObjectSpawnMovementData.spawnAheadTime;
        }
    }
}
