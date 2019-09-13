using UnityEngine;
using NaughtyAttributes;

namespace GameplayIngredients.Logic
{
    public class FlipFlopLogic : LogicBase
    {
        [ReorderableList]
        public Callable[] OnFlip;

        [ReorderableList]
        public Callable[] OnFlop;

        private bool condition = true;

        public override void Execute(GameObject instigator = null)
        {
            if (condition)
            {
                Callable.Call(OnFlip, instigator);
                condition = false;
            }
            else
            {
                Callable.Call(OnFlop, instigator);
                condition = true;
            }
        }
    }
}
