using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayIngredients.Pickup
{
    public class PickupItem : MonoBehaviour
    {
        public PickupEffectBase[] effects { get { return GetComponents<PickupEffectBase>();  } }
    }
}

