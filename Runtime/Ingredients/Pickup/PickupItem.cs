using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayIngredients.Pickup
{
    [HelpURL(Help.URL + "pickup")]
    [RequireComponent(typeof(Collider))]
    public class PickupItem : MonoBehaviour
    {
        public PickupEffectBase[] effects { get { return GetComponents<PickupEffectBase>();  } }

        public Callable[] OnPickup;

        private void OnTriggerEnter(Collider other)
        {
            var owner = other.gameObject.GetComponent<PickupOwnerBase>();
            if(owner != null)
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

