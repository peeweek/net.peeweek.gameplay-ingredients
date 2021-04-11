using NaughtyAttributes;
using UnityEngine;

namespace GameplayIngredients.Events
{
    [AddComponentMenu(ComponentMenu.eventsPath + "On Collider 2D Event")]
    [RequireComponent(typeof(Collider2D))]
    public class OnCollider2DEvent : EventBase
    {
        public Callable[] onCollisionEnter;
        public Callable[] onCollisionExit;

        public bool OnlyInteractWithTag = false;
        [EnableIf("OnlyInteractWithTag")]
        public string Tag = "Player";

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (OnlyInteractWithTag && other.collider.tag == Tag)
            {
                Callable.Call(onCollisionEnter, other.collider.gameObject);
            }
            if (!OnlyInteractWithTag)
            {
                Callable.Call(onCollisionEnter, other.collider.gameObject);
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (OnlyInteractWithTag && other.collider.tag == Tag)
            {
                Callable.Call(onCollisionExit, other.collider.gameObject);
            }
            if (!OnlyInteractWithTag)
            {
                Callable.Call(onCollisionExit, other.collider.gameObject);
            }
        }
    }
}
