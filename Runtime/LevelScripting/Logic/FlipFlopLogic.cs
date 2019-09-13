using UnityEngine;
using NaughtyAttributes;

namespace GameplayIngredients.Logic
{
    public class FlipFlopLogic : LogicBase
    {
        [ReorderableList]
        public Callable[] OnConditionValid;

        [ReorderableList]
        public Callable[] OnConditionInvalid;

        private bool condition = true;

        public override void Execute(GameObject instigator = null)
        {
            if (condition)
            {
                Callable.Call(OnConditionValid, instigator);
                condition = false;
            }
            else
            {
                Callable.Call(OnConditionInvalid, instigator);
                condition = true;
            }
        }
    }
}
