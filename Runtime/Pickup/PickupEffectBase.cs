using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayIngredients.Pickup
{
    public abstract class PickupEffectBase : ScriptableObject
    {
        public float Duration = 0.0f;

        public void ApplyPickupEffect(PickupOwnerBase owner)
        {
            owner.ApplyEffect(this);
        }
    }
}
