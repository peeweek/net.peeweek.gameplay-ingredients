using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayIngredients.Interactive
{
    public class InteractiveManager : Manager
    {
        List<Interactive> m_Interactives;

        private void Awake()
        {
            m_Interactives = new List<Interactive>();
        }

        public void RegisterInteractive(Interactive interactive)
        {
            m_Interactives.Add(interactive);
        }

        public void RemoveInteractive(Interactive interactive)
        {
            m_Interactives.Remove(interactive);
        }

        public void Interact(InteractiveUser user)
        {
            foreach(var interactive in GetCandidates(user))
            {
                if (interactive.Interact(user))
                    break;
            }
        }

        private Interactive[] GetCandidates(InteractiveUser user)
        {
            List<Interactive> candidates = new List<Interactive>();
            foreach(var interactive in m_Interactives)
            {
                if (user.CanInteract(interactive))
                    candidates.Add(interactive);
            }
            return candidates.ToArray();
        }
    }
}
