using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayIngredients.Controllers
{
    public class KeyboardGamepadPlayerInput : PlayerInput
    {
        [Header("Behaviour")]
        public float LookExponent = 2.0f;
        [Range(0.0f, 0.7f)]
        public float MovementDeadZone = 0.15f;
        [Range(0.0f, 0.7f)]
        public float LookDeadZone = 0.15f;

        [Header("Gamepad Axes")]
        public string MovementHorizontalAxis = "Horizontal";
        public string MovementVerticalAxis = "Vertical";
        public string LookHorizontalAxis = "Look X";
        public string LookVerticalAxis = "Look Y";

        [Header("Mouse Axes")]
        public string MouseHorizontalAxis = "Mouse X";
        public string MouseVerticalAxis = "Mouse Y";

        [Header("Buttons")]
        public string JumpButton = "Jump";
        public string PauseButton = "Pause";
        public string ActionButton = "Action";
        public string ShootButton = "Shoot";
        public string UpButton = "Up";
        public string DownButton = "Down";
        public string LeftButton = "Left";
        public string RightButton = "Right";

        public override Vector2 Look => m_Look;
        public override Vector2 Movement => m_Movement;

        public override ButtonState Pause => m_Pause;
        public override ButtonState Jump => m_Jump;
        public override ButtonState Shoot => m_Shoot;
        public override ButtonState Action => m_Action;

        Vector2 m_Movement;
        Vector2 m_Look;

        ButtonState m_Pause;
        ButtonState m_Jump;
        ButtonState m_Shoot;
        ButtonState m_Action;

        public override void UpdateInput()
        {
            m_Movement = new Vector2(Input.GetAxisRaw(MovementHorizontalAxis), Input.GetAxisRaw(MovementVerticalAxis));

            Vector2 l = new Vector2(Input.GetAxisRaw(LookHorizontalAxis), Input.GetAxisRaw(LookVerticalAxis));
            Vector2 ln = l.normalized;
            float lm = Mathf.Clamp01(l.magnitude);
            m_Look = ln * Mathf.Pow(Mathf.Clamp01(lm - LookDeadZone) / (1.0f - LookDeadZone), LookExponent);

            if (m_Movement.magnitude < MovementDeadZone) m_Movement = Vector2.zero;
            m_Look += new Vector2(Input.GetAxisRaw(MouseHorizontalAxis), Input.GetAxisRaw(MouseVerticalAxis));

            m_Pause = GetButtonState(PauseButton);
            m_Action = GetButtonState(ActionButton);
            m_Jump = GetButtonState(JumpButton);
            m_Shoot = GetButtonState(ShootButton);
        }

        ButtonState GetButtonState(string Button)
        {
            if (Input.GetButton(Button))
            {
                if (Input.GetButtonDown(Button))
                    return ButtonState.JustPressed;
                else
                    return ButtonState.Pressed;
            }
            else
            {
                if (Input.GetButtonUp(Button))
                    return ButtonState.JustReleased;
                else
                    return ButtonState.Released;

            }
        }
    }
}
