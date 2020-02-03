using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomEvents.HarmonyPatches
{
    [HarmonyPatch(typeof(BeatmapObjectCallbackController), "Start")]
    class BeatmapObjectCallbackControllerStart
    {
        public static void Postfix(BeatmapObjectCallbackController __instance)
        {
            //Logger.log.Info("Creating CustomEventCallbackController");
            var controller = __instance.gameObject.AddComponent<CustomEventCallbackController>();
            /*
            controller.customEventDidTriggerEvent +=
                e =>
                {
                    Logger.log.Info(e.type + " (" + e.time + ")");
                };
            */
        }
    }
}
