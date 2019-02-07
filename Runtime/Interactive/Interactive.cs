using GameplayIngredients.Events;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayIngredients.Interactive
{
    public abstract class Interactive : EventBase
    {
        [Header("Events")]
        [SerializeField, ReorderableList]
        protected Callable OnInteract;

        protected virtual void OnEnable()
        {
            Manager.Get<InteractiveManager>().RegisterInteractive(this);
        }

        protected virtual void OnDisable()
        {
            Manager.Get<InteractiveManager>().RemoveInteractive(this);
        }

        public bool Interact(InteractiveUser user)
        {
            if (user.CanInteract(this) && CanInteract(user))
            {
                Callable.Call(OnInteract);
                return true;
            }
            else
                return false;
        }

        protected abstract bool CanInteract(InteractiveUser user);

    }
}
