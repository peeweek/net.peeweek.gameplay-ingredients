using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

namespace GameplayIngredients.Actions
{

    public class FullScreenFadeAction : ActionBase
    {
        public FullScreenFadeManager.FadeMode Fading = FullScreenFadeManager.FadeMode.ToBlack;
        public float Duration = 2.0f;
        public FullScreenFadeManager.FadeTimingMode fadeTimingMode = FullScreenFadeManager.FadeTimingMode.UnscaledGameTime;

        [ReorderableList]
        public Callable[] OnComplete;


        public override void Execute(GameObject instigator = null)
        {
            Manager.Get<FullScreenFadeManager>().Fade(Duration, Fading, fadeTimingMode, OnComplete, instigator);
        }
    }

}
