using NaughtyAttributes;
using UnityEngine;

namespace GameplayIngredients.Events
{
    public class OnAwakeEvent : EventBase
    {
        public Callable[] onAwake;

        private void Awake()
        {
            Callable.Call(onAwake, gameObject);
        }
    }
}


