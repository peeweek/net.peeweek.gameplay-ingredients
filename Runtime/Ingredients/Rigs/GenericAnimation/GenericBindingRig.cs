using NaughtyAttributes;
using System;
using UnityEngine;


namespace GameplayIngredients.Rigs
{

    public class GenericBindingRig : Rig
    {
        public enum BindingType
        {
            Float,
            Vector3
        }
        [SerializeField]
        BindingType bindingType = BindingType.Float;

        [InfoBox("Reads the value of SOURCE property and stores it into TARGET property")]

        [SerializeField, ReflectedMember("typeForBinding")]
        ReflectedMember source;
        [SerializeField, ReflectedMember("typeForBinding")]
        ReflectedMember target;

        Type typeForBinding
        {
            get 
            {
                switch (bindingType)
                {
                    case BindingType.Float:
                        return typeof(float);
                    case BindingType.Vector3:
                        return typeof(Vector3);
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public override UpdateMode defaultUpdateMode => UpdateMode.Update;

        public override int defaultPriority => 0;

        public override void UpdateRig(float deltaTime)
        {
            target.SetValue(source.GetValue());
        }
    }
}

