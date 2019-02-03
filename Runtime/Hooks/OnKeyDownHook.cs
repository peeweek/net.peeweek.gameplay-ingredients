using NaughtyAttributes;
using UnityEngine;

namespace GameplayIngredients.Hooks
{
    public class OnKeyDownHook : HookBase
    {
        public KeyCode Key = KeyCode.F5;

        [ReorderableList]
        public Callable[] OnKeyDown;

        [ReorderableList]
        public Callable[] OnKeyUp;

        void Update()
        {
            if (Input.GetKeyDown(Key))
                Callable.Call(OnKeyDown, gameObject);

            if (Input.GetKeyUp(Key))
                Callable.Call(OnKeyUp, gameObject);
        }
    }
}


