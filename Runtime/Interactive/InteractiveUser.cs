using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayIngredients.Interactive
{
    public abstract class InteractiveUser : MonoBehaviour
    {
        public abstract bool CanInteract(Interactive interactive);

        protected void Interact()
        {
            Manager.Get<InteractiveManager>().Interact(this);
        }
    }
}
