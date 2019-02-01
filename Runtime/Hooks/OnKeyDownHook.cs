using NaughtyAttributes;
using UnityEngine;

namespace GameplayIngredients.Hooks
{
    public class OnKeyDownHook : HookBase
    {
        public enum ActionType
        {
            KeyDown,
            KeyUp
        }

        public KeyCode Key = KeyCode.F5;

        [ReorderableList]
        public Callable[] OnKeyDownEvent;

        [ReorderableList]
        public Callable[] OnKeyUpEvent;

        void Update()
        {
            if (Input.GetKeyDown(Key))
                Callable.Call(OnKeyDownEvent, gameObject);

            if (Input.GetKeyUp(Key))
                Callable.Call(OnKeyUpEvent, gameObject);
        }
    }
}


