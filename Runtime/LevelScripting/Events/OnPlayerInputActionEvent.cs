using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace GameplayIngredients.Events
{
#if !ENABLE_INPUT_SYSTEM
    [WarnDisabledModule("New Input System")]
#endif
    [AddComponentMenu(ComponentMenu.eventsPath + "On Player Input Action Event (New Input System)")]
    public class OnPlayerInputActionEvent : EventBase
    {
#if ENABLE_INPUT_SYSTEM
        [SerializeField]
        public PlayerInput playerInput;

        [SerializeField, PlayerInputAction("playerInput")]
        InputAction inputAction;

        public Callable[] onButtonDown;

        private void OnEnable()
        {
            inputAction.performed += InputAction_performed;
        }
        private void OnDisable()
        {
            inputAction.performed -= InputAction_performed;
        }

        private void InputAction_performed(InputAction.CallbackContext obj)
        {
            Callable.Call(onButtonDown, gameObject);
        }
#endif
    }
}


