using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace GameplayIngredients.Events
{
#if !ENABLE_INPUT_SYSTEM
    [WarnDisabledModule("New Input System")]
#endif
    [AddComponentMenu(ComponentMenu.eventsPath + "On Input Action Event (New Input System)")]
    public class OnInputActionEvent : EventBase
    {
#if ENABLE_INPUT_SYSTEM
        public InputAction inputAction;
#endif
    }
}


