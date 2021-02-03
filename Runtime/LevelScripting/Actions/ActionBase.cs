using UnityEngine;

namespace GameplayIngredients.Actions
{
    [HelpURL("https://peeweek.readthedocs.io/en/latest/gameplay-ingredients/events-logic-actions/")]
    public abstract class ActionBase : Callable
    {
        public override sealed string ToString()
        {
            return "Action : " + Name;
        }
    }
}
