using UnityEngine;

namespace GameplayIngredients.Pickup
{
    [AddComponentMenu(ComponentMenu.pickupPath + "Pickup Item")]
    [HelpURL(Help.URL + "pickup")]
    [RequireComponent(typeof(Collider))]
    public class PickupItem : MonoBehaviour
    {
        public PickupEffectBase[] effects { get { return GetComponents<PickupEffectBase>();  } }

        public Callable[] OnPickup;

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.TryGetComponent(out PickupOwnerBase owner))
            {
                if(owner.PickUp(this))
                {
                    Callable.Call(OnPickup, owner.gameObject);
                    Destroy(this.gameObject);
                }
            }
        }
    }
}

