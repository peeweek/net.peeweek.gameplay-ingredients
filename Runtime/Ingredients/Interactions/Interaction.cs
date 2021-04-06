using GameplayIngredients.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayIngredients.Interactions
{
    [HelpURL(Help.URL + "interactive")]
    [Callable("Game", "Misc/ic-interaction.png")]
    public class Interaction : ActionBase
    {
        public InteractiveUser InteractiveUser;

        public override void Execute(GameObject instigator = null)
        {
            if (InteractiveUser != null)
                InteractiveUser.Interact();
        }
    }
}
