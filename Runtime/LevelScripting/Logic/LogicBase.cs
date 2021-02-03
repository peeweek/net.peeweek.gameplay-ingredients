using NaughtyAttributes;
using UnityEngine;

namespace GameplayIngredients.Logic
{
    [HelpURL("https://peeweek.readthedocs.io/en/latest/gameplay-ingredients/events-logic-actions/")]
    public abstract class LogicBase : Callable
    {
        public override sealed string ToString()
        {
            return "Logic : " + Name;
        }
    }
}
