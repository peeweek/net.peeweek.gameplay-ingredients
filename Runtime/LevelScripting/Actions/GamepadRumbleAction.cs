using Managers.Implementations;
using UnityEngine;

namespace GameplayIngredients.Actions
{
    public class GamepadRumbleAction : ActionBase
    {
        public float lowFrequency = 0.2f;
        public float highFrequency = 0.2f;
        public RumblePattern pattern = RumblePattern.Constant;
        public float duration = 1.0f; 
        
        public override void Execute(GameObject instigator = null)
        {
            if (Manager.TryGet<RumbleManager>(out RumbleManager manager))
            {
                manager.Rumble(null, lowFrequency, highFrequency, duration, pattern);
            }
        }
    }
}