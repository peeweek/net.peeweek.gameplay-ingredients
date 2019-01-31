using NaughtyAttributes;
using UnityEngine;

namespace GameplayIngredients.Hooks
{
    public class OnDestroyHook : MonoBehaviour
    {
        [ReorderableList]
        public Callable[] onDestroy;

        private void OnDestroy()
        {
            Callable.Call(onDestroy);
        }
        
    }
}


