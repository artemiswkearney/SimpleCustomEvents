using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCustomEvents.HarmonyPatches
{
    [HarmonyPatch(typeof(BeatmapObjectCallbackController), "Init")]
    class BeatmapObjectCallbackControllerInit
    {
        public static void Postfix(BeatmapObjectCallbackController __instance)
        {
            //Logger.log.Info("Creating CustomEventCallbackController");
            __instance.gameObject.AddComponent<CustomEventCallbackController>()/*
                .customEventDidTriggerEvent +=
                e =>
                {
                    Logger.log.Info(e.type + " (" + e.time + ")");
                }*/;
        }
    }
}
