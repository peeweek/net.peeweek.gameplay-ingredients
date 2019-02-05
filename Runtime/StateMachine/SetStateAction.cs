using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameplayIngredients.StateMachines;

namespace GameplayIngredients.Actions
{
    public class SetStateAction : ActionBase
    {
        [NonNullCheck]
        public StateMachine StateMachine;
        public string State = "State";

        public override void Execute(GameObject instigator = null)
        {
            if(StateMachine != null)
                StateMachine.SetState(State);
        }
    }
}
