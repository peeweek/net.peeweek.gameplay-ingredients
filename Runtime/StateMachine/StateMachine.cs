using NaughtyAttributes;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayIngredients.StateMachines
{
    public class StateMachine : MonoBehaviour
    {
        public State DefaultState;

        [ReorderableList]
        public State[] States;

        State m_CurrentState;

        void Start()
        {
            SetState(DefaultState.StateName);
        }

        public void SetState(string stateName)
        {
            State newState = States.First(o => o.StateName == stateName);
            if(newState != null)
            {
                if (m_CurrentState != null)
                    Callable.Call(m_CurrentState.OnStateExit, gameObject);

                m_CurrentState = newState;
                Callable.Call(m_CurrentState.OnStateEnter, gameObject);
            }
        }

        public void Update()
        {
            if (m_CurrentState != null)
                Callable.Call(m_CurrentState.OnStateUpdate, this.gameObject);
        }

    }
}

