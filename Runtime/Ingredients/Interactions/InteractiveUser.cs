using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayIngredients.Interactions
{
    [HelpURL(Help.URL + "interactive")]
    public abstract class InteractiveUser : MonoBehaviour
    {
        public abstract bool CanInteract(Interactive interactive);

        public abstract Interactive[] SortCandidates(IEnumerable<Interactive> candidates);

        public void Interact()
        {
            InteractionManager.Interact(this);
        }
    }
}
