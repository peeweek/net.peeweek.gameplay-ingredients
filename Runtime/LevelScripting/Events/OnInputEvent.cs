using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using GameplayIngredients.Managers;
#endif

namespace GameplayIngredients.Events
{
#if !ENABLE_INPUT_SYSTEM
    [WarnDisabledModule("New Input System")]
#endif
    [AddComponentMenu(ComponentMenu.eventsPath + "On Input Event (New Input System)")]
    public class OnInputEvent : EventBase
    {
#if ENABLE_INPUT_SYSTEM
        public InputSystemManager.RegisteredInputAction inputAction;
#endif
    }
}


