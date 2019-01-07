using UnityEngine.Events;

namespace GameplayIngredients.Actions
{
    public class UnityEventAction : ActionBase
    {
        public UnityEvent OnExecute;

        public override void Execute()
        {
            OnExecute.Invoke();
        }
    }
}
