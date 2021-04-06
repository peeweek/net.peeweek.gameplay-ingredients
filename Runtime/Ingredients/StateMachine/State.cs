using NaughtyAttributes;
using UnityEngine;

namespace GameplayIngredients.StateMachines
{
    [HelpURL(Help.URL + "state-machines")]
    [AdvancedHierarchyIcon("Packages/net.peeweek.gameplay-ingredients/Icons/Misc/ic-State.png")]
    public class State : MonoBehaviour
    {
        public string StateName { get { return gameObject.name; } }

        public Callable[] OnStateEnter;
        public Callable[] OnStateExit;
        [ShowIf("AllowUpdateCalls")]
        public Callable[] OnStateUpdate;

        private bool AllowUpdateCalls()
        {
            return GameplayIngredientsSettings.currentSettings.allowUpdateCalls;
        }
    }
}
