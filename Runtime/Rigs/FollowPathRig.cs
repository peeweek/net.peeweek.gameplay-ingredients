using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayIngredients.Rigs
{
    public class FollowPathRig : MonoBehaviour
    {
        public enum PlayMode
        {
            Playing,
            Stopped,
            Reverse
        }

        public enum LoopMode
        {
            Hold,
            Loop,
            PingPong
        }

        public LoopMode loopMode = LoopMode.PingPong;
        public bool StopOnSteps = false;
        public float Speed = 2.0f;

        [ReorderableList, SerializeField]
        protected GameObject[] Path;

        public PlayMode initialPlayMode = PlayMode.Playing;
        PlayMode m_PlayMode;
        [SerializeField]
        float progress;

        private void Start()
        {
            m_PlayMode = initialPlayMode;
            progress = 0.0f;
        }

        private void Update()
        {
            if(m_PlayMode != PlayMode.Stopped)
            {
                // Process loopMode and boundary reach
                switch(loopMode)
                {
                    case LoopMode.Hold:
                        if((m_PlayMode == PlayMode.Playing && progress == Path.Length - 1) || (m_PlayMode == PlayMode.Reverse && progress == 0.0f))
                        {
                            m_PlayMode = PlayMode.Stopped;
                            return;
                        }
                        break;
                    case LoopMode.Loop:
                        if (m_PlayMode == PlayMode.Playing && progress == Path.Length -1)
                        {
                            progress = 0.0f;
                        }
                        else if (m_PlayMode == PlayMode.Reverse && progress == 0.0f)
                        {
                            progress = Path.Length -1;
                        }
                        break;
                    case LoopMode.PingPong:
                        if (m_PlayMode == PlayMode.Playing && progress == Path.Length -1)
                        {
                            m_PlayMode = PlayMode.Reverse;
                        }
                        else if (m_PlayMode == PlayMode.Reverse && progress == 0.0f)
                        {
                            m_PlayMode = PlayMode.Playing;
                        }
                        break;
                }

                // Process move on path

                float sign = 1.0f;

                if (m_PlayMode == PlayMode.Reverse)
                    sign = -1.0f;

                int idx = (int)Mathf.Floor(progress);
                int nextidx = idx + (int)sign;

                Vector3 inPos = Path[idx].transform.position;
                Vector3 outPos = Path[nextidx].transform.position;

                Vector3 dir = ( outPos - inPos ).normalized;
                Vector3 pos = Vector3.Lerp(inPos, outPos, (sign > 0)? progress % 1.0f : 1.0f-(progress % 1.0f));
                Vector3 move = dir * Speed * Time.deltaTime;
                float moveT = move.magnitude / (outPos - inPos).magnitude * sign;

                progress = Mathf.Clamp(progress + moveT * sign, idx, nextidx);

                if(progress == nextidx)
                    transform.position = outPos;
                else
                    transform.position = Vector3.Lerp(inPos, outPos, (sign > 0) ? progress % 1.0f : 1.0f - (progress % 1.0f));

            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1.0f, 1.0f, 1.0f, 0.1f);
            DrawGizmosPath();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            DrawGizmosPath();
        }

        void DrawGizmosPath()
        {
            if(Path != null && Path.Length > 1)
            {
                for(int i = 0; i < Path.Length -1; i++)
                {
                    Gizmos.DrawLine(Path[i].transform.position, Path[i + 1].transform.position);
                }
            }
        }

    }
}

