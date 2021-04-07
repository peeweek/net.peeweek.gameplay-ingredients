using UnityEngine;
using UnityEngine.UI;

namespace GameplayIngredients.Actions
{
    [AddComponentMenu(ComponentMenu.actionsPath + "Focus UI Action")]
    [Callable("UI", "Actions/ic-action-ui.png")]
    public class FocusUIAction : ActionBase
    {
        public Selectable UIObjectToFocus;

        public override void Execute(GameObject instigator = null)
        {
            if (UIObjectToFocus != null)
            {
                // Workaround : Before selecting, we ensure that there's no selection in the EventSystem
                Manager.Get<UIEventManager>().eventSystem.SetSelectedGameObject(null);
                UIObjectToFocus.Select();
            }
        }
        public override string GetDefaultName()
        {
            return $"Focus UI : '{UIObjectToFocus?.name}'";
        }
    }
}
