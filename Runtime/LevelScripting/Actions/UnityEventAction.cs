using UnityEngine;
using UnityEngine.Events;

namespace GameplayIngredients.Actions
{
    [Callable("Game", "Actions/ic-action-event.png")]
    public class UnityEventAction : ActionBase
    {
        public UnityEvent OnExecute;

        public override void Execute(GameObject instigator = null)
        {
            OnExecute.Invoke();
        }

        public override string GetDefaultName()
        {
            return $"Call UnityEvent(s)";
        }
    }
}
