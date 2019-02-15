using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayIngredients.Pickup
{
    [RequireComponent(typeof(Collider))]
    public class PickupItem : MonoBehaviour
    {
        public PickupEffectBase[] effects { get { return GetComponents<PickupEffectBase>();  } }

        private void OnTriggerEnter(Collider other)
        {
            var owner = other.gameObject.GetComponent<PickupOwnerBase>();
            if(owner != null)
            {
                if(owner.PickUp(this))
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }
}

