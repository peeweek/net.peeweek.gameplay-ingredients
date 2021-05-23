using System;
using UnityEngine;

namespace GameplayIngredients.Rigs
{
    [Serializable]
    public abstract class FloatAnimationHandler : AnimationHandler<float> { }

    [Serializable, AnimationHandler("Continuous Float", typeof(float))]
    public class FloatContinuousAnimationHandler : FloatAnimationHandler
    {
        [SerializeField]
        float Rate = 1.0f;

        float m_Base;
        float m_Time;

        public override void OnStart(float defaultValue)
        {
            m_Base = defaultValue;
            m_Time = 0;
        }

        public override float OnUpdate(float deltaTime)
        {
            m_Time += deltaTime;
            return m_Base + m_Time * Rate;
        }
    }

    [Serializable, AnimationHandler("Sine Float", typeof(float))]
    public class FloatSineAnimationHandler : FloatAnimationHandler
    {
        [SerializeField]
        float frequency = 1.0f;
        [SerializeField]
        public float amplitude = 1.0f;

        float m_Base;
        float m_Time;

        public override void OnStart(float defaultValue)
        {
            m_Base = defaultValue;
            m_Time = 0;
        }

        public override float OnUpdate(float deltaTime)
        {
            m_Time += deltaTime;
            return m_Base + Mathf.Sin(m_Time * frequency * Mathf.PI) * amplitude;
        }
    }


}
