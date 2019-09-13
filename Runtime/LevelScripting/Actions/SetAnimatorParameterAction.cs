using UnityEngine;
using NaughtyAttributes;

public enum AnimatorParameterType { Bool, Float, Int, Trigger };

namespace GameplayIngredients.Actions
{
    public class SetAnimatorParameterAction : ActionBase
    {
        public Animator animator;
        public string parameterName;
        [OnValueChanged("OnParameterTypeChanged")]
        public AnimatorParameterType parameterType;

        bool showFloat;
        bool showInt;
        bool showBool = true;
        [ShowIf("showFloat")]
        public float floatValue;
        [ShowIf("showInt")]
        public int intValue;
        [ShowIf("showBool")]
        public bool boolValue;

        public override void Execute(GameObject instigator = null)
        {
            if (animator == null)
                return;

            switch (parameterType)
            {
                case AnimatorParameterType.Bool:
                    animator.SetBool(parameterName, boolValue);
                    break;
                case AnimatorParameterType.Float:
                    animator.SetFloat(parameterName, floatValue);
                    break;
                case AnimatorParameterType.Int:
                    animator.SetInteger(parameterName, intValue);
                    break;
                case AnimatorParameterType.Trigger:
                    animator.SetTrigger(parameterName);
                    break;
            }       
        }

        private void OnParameterTypeChanged()
        {
            showBool = false;
            showFloat = false;
            showInt = false;

            switch (parameterType)
            {
                case AnimatorParameterType.Bool:
                    showBool = true;
                    break;
                case AnimatorParameterType.Float:
                    showFloat = true;
                    break;
                case AnimatorParameterType.Int:
                    showInt = true;
                    break;
            }
        }
    }
}
