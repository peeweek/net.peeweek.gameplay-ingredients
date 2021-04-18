#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace GameplayIngredients.Events
{
#if !ENABLE_INPUT_SYSTEM
    [WarnDisabledModule("New Input System")]
#endif
    public class OnInputSystemEvent : EventBase
    {
#if ENABLE_INPUT_SYSTEM
        [RegisteredInputActions]
        public InputActionAsset inputAction;
#endif


    }
}


