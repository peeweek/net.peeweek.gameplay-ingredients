using NaughtyAttributes;
using UnityEngine;

namespace GameplayIngredients.Events
{
    public class OnDestroyEvent : MonoBehaviour
    {
        [ReorderableList]
        public Callable[] onDestroy;

        private void OnDestroy()
        {
            Callable.Call(onDestroy, gameObject);
        }
        
    }
}


