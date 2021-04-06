using NaughtyAttributes;
using UnityEngine;

namespace GameplayIngredients.Events
{
    public class OnDestroyEvent : EventBase
    {
        public Callable[] onDestroy;

        private void OnDestroy()
        {
            Callable.Call(onDestroy, gameObject);
        }
        
    }
}


