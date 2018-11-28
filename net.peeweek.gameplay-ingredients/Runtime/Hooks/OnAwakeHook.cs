using NaughtyAttributes;
using UnityEngine;

namespace GameplayIngredients.Hooks
{
    public class OnAwakeHook : MonoBehaviour
    {
        [ReorderableList]
        public Callable[] onAwake;

        private void Awake()
        {
            Callable.Call(onAwake);
        }
    }
}


