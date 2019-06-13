using CustomJSONData.CustomBeatmap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static IPA.Utilities.ReflectionUtil;

namespace CustomEvents
{
    /// <summary>
    /// Some code based on code in the Beat Saber class BeatmapObjectController. No code directly copied.
    /// Beat Saber is copyright Beat Games.
    /// </summary>
    [RequireComponent(typeof(BeatmapObjectCallbackController))]
    public class CustomEventCallbackController : MonoBehaviour
    {
        public BeatmapObjectCallbackController beatmapObjectCallbackController;
        public BeatmapDataModel beatmapDataModel;
        public AudioTimeSyncController audioTimeSyncController;
        protected Dictionary<string, List<CustomEventCallbackData>> _callbackDatas;
        public Dictionary<string, List<CustomEventCallbackData>> callbackDatas
        {
            get
            {
                if (_callbackDatas == null) _callbackDatas = new Dictionary<string, List<CustomEventCallbackData>>();
                return _callbackDatas;
            }
            set { _callbackDatas = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, int> eventLoopbackCallbackIds;

        public class CustomEventCallbackData
        {
            public Action<CustomEventData> callback;
            public string eventType;
            public float aheadTime;
            public int nextEventIndex;
            public int id;
            protected static int nextId;

            public CustomEventCallbackData(Action<CustomEventData> callback, string eventType, float aheadTime)
            {
                id = nextId++;
                this.callback = callback;
                this.eventType = eventType;
                this.aheadTime = aheadTime;
                nextEventIndex = 0;
            }
        }

        /// <summary>
        /// Called whenever any custom event occurs, regardless of event type.
        /// </summary>
        public event Action<CustomEventData> customEventDidTriggerEvent;

        public void beatmapDataChanged()
        {
            foreach (var pair in callbackDatas)
            {
                foreach (var callback in pair.Value)
                {
                    callback.nextEventIndex = 0;
                }
            }
            if (beatmapDataModel.beatmapData is CustomBeatmapData beatmapData)
            {
                foreach (var pair in beatmapData.customEventData)
                {
                    if (!eventLoopbackCallbackIds.ContainsKey(pair.Key))
                    {
                        eventLoopbackCallbackIds[pair.Key] = AddCustomEventCallback(d => customEventDidTriggerEvent?.Invoke(d), pair.Key, 0);
                    }
                }
            }
        }

        public void Awake()
        {
            beatmapObjectCallbackController = GetComponent<BeatmapObjectCallbackController>();
            beatmapDataModel = beatmapObjectCallbackController.GetPrivateField<BeatmapDataModel>("_beatmapDataModel");
            audioTimeSyncController = beatmapObjectCallbackController.GetPrivateField<AudioTimeSyncController>("_audioTimeSyncController");
            beatmapDataModel.beatmapDataDidChangeEvent += beatmapDataChanged;
            eventLoopbackCallbackIds = new Dictionary<string, int>();
            beatmapDataChanged();
            Plugin.invokeCallbackControllerAwake(this);
        }

        public void OnDestroy()
        {
            if (beatmapDataModel != null) beatmapDataModel.beatmapDataDidChangeEvent -= beatmapDataChanged;
        }

        public void LateUpdate()
        {
            CustomBeatmapData beatmapData = beatmapDataModel.beatmapData as CustomBeatmapData;
            if (beatmapData == null) return;
            foreach (var pair in beatmapData.customEventData)
            {
                if (!callbackDatas.ContainsKey(pair.Key)) continue;
                foreach (var callbackData in callbackDatas[pair.Key])
                {
                    while (callbackData.nextEventIndex < pair.Value.Count)
                    {
                        CustomEventData eventData = pair.Value[callbackData.nextEventIndex];
                        if (eventData.time - callbackData.aheadTime >= audioTimeSyncController.songTime) break;
                        if (eventData.time >= beatmapObjectCallbackController.startSongTime) // skip events before song start
                        {
                            callbackData.callback(eventData);
                        }
                        callbackData.nextEventIndex++;
                    }
                }
            }
        }

        /// <summary>
        /// Registers a callback for a given custom event type.
        /// </summary>
        /// <param name="callback">Called when an event of type <paramref name="eventType"/> occurs.</param>
        /// <param name="eventType">The type of event to call the callback for.</param>
        /// <param name="aheadTime">How far in advance of the event's position in the song to call the callback.</param>
        /// <returns>An ID that can be used with <see cref="RemoveCustomEventCallback(int)"/> to unregister the callback later</returns>
        public int AddCustomEventCallback(Action<CustomEventData> callback, string eventType, float aheadTime)
        {
            var callbackData = new CustomEventCallbackData(callback, eventType, aheadTime);
            if (!callbackDatas.ContainsKey(eventType))
            {
                callbackDatas[eventType] = new List<CustomEventCallbackData>();
            }
            callbackDatas[eventType].Add(callbackData);
            return callbackData.id;
        }
        /// <summary>
        /// Unregisters a previously registered callback.
        /// </summary>
        /// <param name="callbackId">A callback ID returned by <see cref="AddCustomEventCallback(Action{CustomEventData}, string, float)"/></param>
        public void RemoveCustomEventCallback(int callbackId)
        {
            foreach (var pair in callbackDatas)
            {
                for (int i = 0; i < pair.Value.Count; i++)
                {
                    if (pair.Value[i].id == callbackId)
                    {
                        pair.Value.RemoveAt(i);
                        return;
                    }
                }
            }
        }
    }
}
