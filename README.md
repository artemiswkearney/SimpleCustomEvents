# SimpleCustomEvents
Simple plugin to broadcast CustomJSONData's custom events

The interface provided for subscribing to custom events is based on `BeatmapObjectCallbackController`.    
A `CustomEvents.CustomEventCallbackController` component can be found on the same gameObject as `BeatmapObjectCallbackController`. To subscribe to all custom events of a given type (`_type` in the event JSON object), use the method `AddCustomEventCallback`. As with subscribing to lighting events from `BeatmapObjectCallbackController`, you may request to receive events ahead of their position in the song using the `aheadTime` parameter. However, unlike subscribing to lighting events, your callback will only be invoked for a single type of event specified when the callback is added. The (C#) event `customEventDidTriggerEvent` will fire whenever a custom event of any type occurs in the song (as though registered with an `aheadTime` of 0).
