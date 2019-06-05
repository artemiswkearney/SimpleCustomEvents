using Harmony;
using IPA;
using IPA.Config;
using IPA.Utilities;
using System.Reflection;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;

namespace CustomEvents
{
    public class Plugin : IBeatSaberPlugin
    {

        public void Init(IPALogger logger)
        {
            var harmony = HarmonyInstance.Create("com.arti.BeatSaber.CustomEvents");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Logger.log = logger;
        }

        public void OnApplicationStart()
        {
            //Logger.log.Debug("OnApplicationStart");
        }

        public void OnApplicationQuit()
        {
            //Logger.log.Debug("OnApplicationQuit");
        }

        public void OnFixedUpdate()
        {

        }

        public void OnUpdate()
        {

        }

        public void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
        {

        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {

        }

        public void OnSceneUnloaded(Scene scene)
        {

        }
    }
}
