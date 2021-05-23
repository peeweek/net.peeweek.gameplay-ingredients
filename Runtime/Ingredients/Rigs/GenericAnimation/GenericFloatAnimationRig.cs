using NaughtyAttributes;
using System;
using UnityEngine;

namespace GameplayIngredients.Rigs
{
    public class GenericFloatAnimationRig : GenericAnimationRig
    {
        [Header("Base Value")]
        [SerializeField]
        bool useStoredValueAsBase = true;
        [SerializeField, DisableIf("useStoredValueAsBase")]
        float baseValue = 1.0f;

        [Header("Animation")]
        public float frequency = 1.0f;
        public float amplitude = 1.0f;

        public override Type animatedType => typeof(float);

        private void Awake()
        {
            if (useStoredValueAsBase)
                baseValue = (float)property.GetValue();
        }

        protected override object UpdateAndGetValue(float deltaTime)
        {
            return Mathf.Sin(Time.time * frequency * Mathf.PI) * amplitude + baseValue;
        }
    }
}

