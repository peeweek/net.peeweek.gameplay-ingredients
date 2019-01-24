using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GameplayIngredients.Editor
{
    public static class PlayFromHere
    {
        public delegate void PlayFromHereDelegate(Vector3 position, Vector3 forward);

        public static event PlayFromHereDelegate OnPlayFromHere;

        [InitializeOnLoadMethod]
        public static void Initialize()
        {
            EditorApplication.playModeStateChanged += OnEnterPlayMode;
        }

        
        public static void Play()
        {
            EditorPrefs.SetBool("playFromHereNext",true);
            EditorApplication.isPlaying = true;
        }

        static void OnEnterPlayMode(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredPlayMode && EditorPrefs.GetBool("playFromHereNext"))
            {
                if (OnPlayFromHere != null)
                {
                    Vector3 position = Vector3.zero;
                    Vector3 forward = Vector3.forward;
                    if (SceneView.lastActiveSceneView != null)
                    {
                        // Let's choose a point 1m in front of the point of view
                        var camera = SceneView.lastActiveSceneView.camera;
                        position = camera.transform.position;
                        forward = camera.transform.forward;
                    }
                    else
                        Debug.LogWarning("Play From Here : Could not find the position of the last sceneview camera, playing at world's origin.");

                    OnPlayFromHere.Invoke(position,forward);

                }
                else
                {
                    Debug.LogWarning("Play From Here : No Actions to take. Please add events to PlayFromHere.OnPlayFromHere()");
                }

                EditorPrefs.SetBool("playFromHereNext", false);
            }

        }
    }
}
