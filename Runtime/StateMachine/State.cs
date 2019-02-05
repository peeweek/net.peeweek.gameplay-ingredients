using NaughtyAttributes;
using UnityEngine;

namespace GameplayIngredients.StateMachines
{
    public class State : MonoBehaviour
    {
        public string StateName = "State";

        [ReorderableList]
        public Callable[] OnStateEnter;
        [ReorderableList]
        public Callable[] OnStateExit;
        [ReorderableList]
        public Callable[] OnStateUpdate;
    }
}
