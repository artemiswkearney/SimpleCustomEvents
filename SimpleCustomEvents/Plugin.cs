using HarmonyLib;
using IPA;
using IPA.Config;
using IPA.Utilities;
using System;
using System.Reflection;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;

namespace CustomEvents
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        public static event Action<CustomEventCallbackController> callbackControllerAwake;
        internal static void invokeCallbackControllerAwake(CustomEventCallbackController callbackController) => callbackControllerAwake?.Invoke(callbackController);
        public static Harmony harmony;

        [Init]
        public void Init(IPALogger logger)
        {
            harmony = new Harmony("com.arti.BeatSaber.CustomEvents");
            Logger.log = logger;
        }

        [OnEnable]
        public void OnEnable()
        {
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        [OnDisable]
        public void OnDisable()
        {
            harmony.UnpatchAll();
        }
    }
}
