using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace GameplayIngredients.Logic
{
    public class NextFrameLogic : LogicBase
    {
        [ReorderableList]
        public Callable[] OnComplete;
        IEnumerator m_Coroutine;

        public override void Execute(GameObject instigator = null)
        {
            m_Coroutine = RunDelay();
            StartCoroutine(m_Coroutine);
        }

        IEnumerator RunDelay()
        {
            yield return new WaitForEndOfFrame();
            Callable.Call(OnComplete);
            m_Coroutine = null;
        }
    }
}

