using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayIngredients.Controllers
{
    public class SimplePlayerInput : PlayerInput
    {
        [Header("Movement")]
        public string MovementHorizontalAxis = "Horizontal";
        public string MovementVerticalAxis = "Vertical";

        [Header("Look")]
        public string LookHorizontalAxis = "Mouse X";
        public string LookVerticalAxis = "Mouse Y";

        [Header("Buttons")]
        public string JumpButton = "Jump";

        public override Vector2 Look => m_Look;
        public override Vector2 Movement => m_Movement;

        public override ButtonState Jump => m_Jump;

        Vector2 m_Movement;
        Vector2 m_Look;

        ButtonState m_Jump;

        public override void UpdateInput()
        {
            m_Movement = new Vector2(Input.GetAxis(MovementHorizontalAxis), Input.GetAxis(MovementVerticalAxis));
            m_Look = new Vector2(Input.GetAxis(LookHorizontalAxis), Input.GetAxis(LookVerticalAxis));
            m_Jump = GetButtonState(JumpButton);
        }
    }
}
