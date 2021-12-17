using Managers.Implementations;
using UnityEngine;

namespace GameplayIngredients.Actions
{
    public class GamepadRumbleStopAction : ActionBase
    {
        public override void Execute(GameObject instigator = null)
        {
            if (Manager.TryGet<RumbleManager>(out RumbleManager manager))
            {
                manager.StopRumble();
            }
        }
    }
}