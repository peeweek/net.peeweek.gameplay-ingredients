using NaughtyAttributes;
using UnityEngine.Events;

namespace GameplayIngredients.Hooks
{
    public class OnEnableDisableHook : HookBase
    {
        [ReorderableList]
        public Callable[] OnEnableEvent;
        [ReorderableList]
        public Callable[] OnDisableEvent;

        private void OnEnable()
        {
            Callable.Call(OnEnableEvent, gameObject);
        }

        private void OnDisable()
        {
            Callable.Call(OnDisableEvent, gameObject);
        }
    }
}

