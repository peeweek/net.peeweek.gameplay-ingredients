using NaughtyAttributes;
using UnityEngine;

namespace GameplayIngredients.Events
{
    [RequireComponent(typeof(Collider))]
    public class OnColliderEvent : EventBase
    {
        [ReorderableList]
        public Callable[] onCollisionEnter;

        [ReorderableList]
        public Callable[] onCollisionExit;

        [ReorderableList]
        public Callable[] onCollisionStay;

        public bool OnlyInteractWithTag = false;
        [EnableIf("OnlyInteractWithTag")]
        public string Tag = "Player";

        private void OnCollisionEnter(Collider other)
        {
            if (OnlyInteractWithTag && other.tag == Tag)
            {
                Callable.Call(onCollisionEnter, other.gameObject);
            }
            if (!OnlyInteractWithTag)
            {
                Callable.Call(onCollisionEnter, other.gameObject);
            }
        }

        private void OnCollisionExit(Collider other)
        {
            if (OnlyInteractWithTag && other.tag == Tag)
            {
                Callable.Call(onCollisionExit, other.gameObject);
            }
            if (!OnlyInteractWithTag)
            {
                Callable.Call(onCollisionExit, other.gameObject);
            }
        }

        private void OnCollisionStay(Collider other)
        {
            if (OnlyInteractWithTag && other.tag == Tag)
            {
                Callable.Call(onCollisionStay, other.gameObject);
            }
            if (!OnlyInteractWithTag)
            {
                Callable.Call(onCollisionStay, other.gameObject);
            }
        }
    }
}
