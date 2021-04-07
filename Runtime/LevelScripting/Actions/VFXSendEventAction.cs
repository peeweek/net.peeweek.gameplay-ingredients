using UnityEngine;
using UnityEngine.VFX;

namespace GameplayIngredients.Actions
{
    [AddComponentMenu(ComponentMenu.actionsPath + "VFX Send Event Action")]
    [Callable("Game", "Misc/ic-vfx.png")]
    public class VFXSendEventAction : ActionBase
    {
        public VisualEffect visualEffect;

        public string eventName = "Event";

        public override void Execute(GameObject instigator = null)
        {
            int id = Shader.PropertyToID(eventName);
            var attrib = visualEffect.CreateVFXEventAttribute();
            visualEffect.SendEvent(eventName, attrib);
        }

        public override string GetDefaultName()
        {
            return $"Send VFX Event '{eventName}' to {visualEffect?.gameObject.name}";
        }
    }

}
