using UnityEngine;

namespace GameplayIngredients.Events
{
    [HelpURL("https://peeweek.readthedocs.io/en/latest/gameplay-ingredients/events-logic-actions/")]
    public abstract class EventBase : MonoBehaviour
    {
        protected bool AllowUpdateCalls()
        {
            return GameplayIngredientsSettings.currentSettings.allowUpdateCalls;
        }
        protected bool ForbidUpdateCalls()
        {
            return !GameplayIngredientsSettings.currentSettings.allowUpdateCalls;
        }
    }
}


