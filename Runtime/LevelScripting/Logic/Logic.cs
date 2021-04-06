using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace GameplayIngredients.Logic
{
    [Callable("Logic", "Logic/ic-generic-logic.png")]
    public class Logic : LogicBase
    {
        public Callable[] Calls;

        public override void Execute(GameObject instigator = null)
        {
            Callable.Call(Calls, instigator);
        }
    }
}

