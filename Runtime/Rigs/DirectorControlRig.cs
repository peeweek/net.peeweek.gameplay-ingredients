using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace GameplayIngredients.Rigs
{
    public class DirectorControlRig : MonoBehaviour
    {
        [NonNullCheck]
        public PlayableDirector director;

        public bool Playing = false;
        public bool Reverse = false;
        public bool UnscaledGameTime = false;

        public DirectorWrapMode wrapMode = DirectorWrapMode.Hold;
        
        float nextPauseTime = -1.0f;

        private void OnEnable()
        {
            if (director != null)
                director.timeUpdateMode = DirectorUpdateMode.Manual;
        }

        public void Play(PlayableAsset asset = null, bool reverse = false, float time = 0.0f)
        {
            Playing = true;
            Reverse = reverse;

            director.time = time;

            if(asset != null)
                director.playableAsset = asset;
        }

        public void Play(PlayableAsset asset = null)
        {
            Play(asset, Reverse, (float)director.time);
        }

        public void Pause()
        {
            director.Pause();
        }

        public void SetTime(float time, bool Reverse = false)
        {
            director.time = time;
            director.Evaluate();
        }

        public void Update()
        {
            if(Playing)
            {
                float dt = UnscaledGameTime? Time.unscaledDeltaTime : Time.deltaTime;

                director.time += Reverse ? -1.0f : 1.0f * dt;
                director.Evaluate();
            }
        }
    }
}
