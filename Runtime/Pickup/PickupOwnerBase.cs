using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayIngredients.Pickup
{
    public abstract class PickupOwnerBase : MonoBehaviour
    {
        public abstract void ApplyEffect<T>(T effect) where T : PickupEffectBase;
    }
}

