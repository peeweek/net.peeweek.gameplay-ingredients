using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayIngredients.Controllers
{
    public abstract class PlayerInput : MonoBehaviour
    {
        public enum ButtonState
        {
            Released = 0,
            JustPressed = 1,
            Pressed = 2,
            JustReleased = 3
        }

        public abstract Vector2 Look { get; }
        public abstract Vector2 Movement { get; }
        public abstract ButtonState Pause { get; }
        public abstract ButtonState Jump { get; }
        public abstract ButtonState Shoot { get; }
        public abstract ButtonState Action { get; }

        public abstract void UpdateInput();
    }
}
