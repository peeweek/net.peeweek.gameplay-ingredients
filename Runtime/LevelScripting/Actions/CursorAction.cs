using UnityEngine;

namespace GameplayIngredients.Actions
{
    [Callable("Input", "Actions/ic-action-mouse.png")]
    public class CursorAction : ActionBase
    {
        public CursorLockMode LockState = CursorLockMode.None;
        public bool CursorVisible = true;

        public override void Execute(GameObject instigator = null)
        {
            Cursor.lockState = LockState;
            Cursor.visible = CursorVisible;
        }
        public override string GetDefaultName()
        {
            return $"{(CursorVisible ? "Show" : "Hide")} Cursor / Lock: {LockState}";
        }
    }
}

