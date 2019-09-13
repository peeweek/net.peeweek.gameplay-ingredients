using NaughtyAttributes;
using UnityEngine;

namespace GameplayIngredients.Events
{
    [RequireComponent(typeof(Joint))]
    public class OnJointBreakEvent : EventBase
    {
        [ReorderableList]
        public Callable[] onJointBreak;

        private void OnJointBreak(Collider other)
        {
            Callable.Call(onJointBreak, other.gameObject);
        }
    }
}