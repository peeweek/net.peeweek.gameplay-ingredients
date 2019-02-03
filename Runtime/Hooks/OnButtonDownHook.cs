using NaughtyAttributes;
using UnityEngine;

namespace GameplayIngredients.Hooks
{
    public class OnButtonDownHook : HookBase
    {
        public string Button = "Fire1";

        [ReorderableList]
        public Callable[] OnButtonDown;

        [ReorderableList]
        public Callable[] OnButtonUp;

        void Update()
        {
            if (Input.GetButtonDown(Button))
                Callable.Call(OnButtonDown, gameObject);

            if (Input.GetButtonUp(Button))
                Callable.Call(OnButtonUp, gameObject);
        }
    }
}


