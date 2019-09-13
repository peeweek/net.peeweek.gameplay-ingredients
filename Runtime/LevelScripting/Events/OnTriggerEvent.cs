using NaughtyAttributes;
using UnityEngine;

namespace GameplayIngredients.Events
{
    public class OnTriggerEvent : EventBase
    {
        [ReorderableList]
        public Callable[] onTriggerEnter;

        [ReorderableList]
        public Callable[] onTriggerExit;

        [ReorderableList]
        public Callable[] onTriggerStay;

        public bool OnlyInteractWithTag = true;
        [EnableIf("OnlyInteractWithTag")]
        public string Tag = "Player";

        private void OnTriggerEnter(Collider other)
        {
            if (OnlyInteractWithTag && other.tag == Tag )
            {
                Callable.Call(onTriggerEnter, other.gameObject);
            }
            if (!OnlyInteractWithTag)
            {
                Callable.Call(onTriggerEnter, other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (OnlyInteractWithTag && other.tag == Tag )
            {
                Callable.Call(onTriggerExit, other.gameObject);
            }
            if (!OnlyInteractWithTag)
            {
                Callable.Call(onTriggerExit, other.gameObject);
            }
        }

        private void OnTriggerStaý(Collider other)
        {
            if (OnlyInteractWithTag && other.tag == Tag)
            {
                Callable.Call(onTriggerStay, other.gameObject);
            }
            if (!OnlyInteractWithTag)
            {
                Callable.Call(onTriggerStay, other.gameObject);
            }
        }
    }
}
