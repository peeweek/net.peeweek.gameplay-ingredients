using GameplayIngredients;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers.Implementations
{
    [AddComponentMenu(ComponentMenu.managersPath + "Rumble Manager")]
    public class RumbleManager : Manager
    {
        private float _lowFreq;
        private float _highFreq;
        private float _duration;
        private RumblePattern _pattern = RumblePattern.None;
        private Gamepad _gamepad;

        public void Rumble(Gamepad gamepad, float lowFreq, float highFreq, float duration, RumblePattern pattern)
        {
            if (gamepad == null)
            {
                _gamepad = Gamepad.current;
            }
            _lowFreq = lowFreq;
            _highFreq = highFreq;
            _duration = Time.time + duration;
            _pattern = pattern;
        }

        public void StopRumble()
        {
            _pattern = RumblePattern.None;
            _gamepad?.SetMotorSpeeds(0, 0);
        }

        private void Update()
        {
            if (_gamepad == null) return;
            if (_pattern == RumblePattern.None) return;
            if (Time.time > _duration) return;

            switch (_pattern)
            {
                case RumblePattern.Constant:
                    _gamepad.SetMotorSpeeds(_lowFreq, _highFreq);
                    break;
                case RumblePattern.Pulse:
                    break;
                case RumblePattern.Linear:
                    break;
            }
        }

        private void OnDestroy()
        {
            StopRumble();
        }
    }

    public enum RumblePattern
    {
        None,
        Constant,
        Pulse,
        Linear
    }
}