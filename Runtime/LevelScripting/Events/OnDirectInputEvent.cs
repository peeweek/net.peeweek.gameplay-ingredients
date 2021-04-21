using GameplayIngredients.Managers;
using NaughtyAttributes;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
#endif

namespace GameplayIngredients.Events
{
#if !ENABLE_INPUT_SYSTEM
    [WarnDisabledModule("New Input System")]
#endif
    [AddComponentMenu(ComponentMenu.eventsPath + "On Direct Input Event (New Input System)")]
    public class OnDirectInputEvent : EventBase
    {
        [SerializeField]
        Device device = Device.Keyboard;
        [SerializeField, ShowIf("IsMouse")]
        MouseButton mouseButton = MouseButton.Left;
        [SerializeField, ShowIf("IsGamepad")]
        GamepadButton gamepadButton = GamepadButton.A;
        [SerializeField, ShowIf("IsKeyboard")]
        Key keyboardKey = Key.Space;

        public Callable[] onPressed;
        public Callable[] onReleased;

        bool IsMouse() => device == Device.Mouse;
        bool IsGamepad() => device == Device.Gamepad;
        bool IsKeyboard() => device == Device.Keyboard;

        ButtonControl button;
#if ENABLE_INPUT_SYSTEM

        private void OnEnable()
        {
            button = GetButton(device);
            Manager.Get<SingleUpdateManager>().Register(SingleUpdate);
        }

        private void OnDisable()
        {
            Manager.Get<SingleUpdateManager>().Remove(SingleUpdate);
        }

        private void SingleUpdate()
        {
            if (button == null)
                return;

            if (button.wasPressedThisFrame)
                Callable.Call(onPressed, null);

            if (button.wasReleasedThisFrame)
                Callable.Call(onReleased, null);
        }
        ButtonControl GetButton(Device d)
        {
            switch (d)
            {
                case Device.Gamepad:
                    return InputSystemManager.GetButton(gamepadButton);
                case Device.Keyboard:
                    return InputSystemManager.GetButton(keyboardKey);
                case Device.Mouse:
                    return InputSystemManager.GetButton(mouseButton);
                default:
                    throw new System.NotImplementedException();
            }
        }

#endif


    }
}


