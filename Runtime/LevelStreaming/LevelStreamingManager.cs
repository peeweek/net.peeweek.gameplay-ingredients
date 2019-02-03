using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

namespace GameplayIngredients.LevelStreaming
{
    [ManagerDefaultPrefab("LevelStreamingManager")]
    public class LevelStreamingManager : Manager
    {
        public enum StreamingAction
        {
            Load,
            Unload
        }

        [Header("UI Configuration")]
        public GameObject LoadingRoot;
        public GameObject ProgressBar;
        public GameObject ProgressBarContainer;
        public Text LoadingText;
        public Text PercentageText;

        [Header("Behavior")]
        [Min(0.0f)]
        public float DelayBeforeLoad = 0.0f;
        [Min(0.0f)]
        public float DelayAfterLoad = 0.0f;

        [Header("Debug")]
        public Text DebugText;
        public bool EnableDebug = false;

        private float[] percentages;
        private AsyncOperation[] asyncOperations;


        public void LoadScenes(StreamingAction action, string[] scenes, string sceneToActivate, bool showUI, Callable[] onLoadComplete)
        {
            if (EnableDebug)
                DebugText.gameObject.SetActive(true);

            List<string> requiredScenes = new List<string>();

            foreach (string scene in scenes)
            {
                if (
                        (SceneManager.GetSceneByName(scene).isLoaded && action == StreamingAction.Unload)
                    || (!SceneManager.GetSceneByName(scene).isLoaded && action == StreamingAction.Load)
                    )
                {
                    requiredScenes.Add(scene);
                }
            }

            int count = requiredScenes.Count;
            percentages = new float[count];
            asyncOperations = new AsyncOperation[count];

            if (showUI)
                LoadingRoot.SetActive(true);

            if (count > 0)
                StartCoroutine(LoadScenesCoroutine(action, requiredScenes, sceneToActivate, showUI, onLoadComplete));
            else
            {
                Debug.LogWarning("Did not find any candidates to load or unload...");

                if (onLoadComplete != null)
                    Callable.Call(onLoadComplete);

                if (showUI)
                    LoadingRoot.SetActive(false);

                if (EnableDebug)
                    DebugText.gameObject.SetActive(false);
            }
        }

        IEnumerator LoadScenesCoroutine(StreamingAction action, List<string> scenes, string sceneToActivate, bool showUI, Callable[] onLoadComplete)
        {
            LogDebugInformation("START LOAD/UNLOAD FOR LEVELS...");
            SetProgressBar(0.0f, true);

            if(DelayBeforeLoad >= 0.0f)
                yield return new WaitForSeconds(DelayBeforeLoad);

            switch (action)
            {
                case StreamingAction.Load:
                    LogDebugInformation("[*] ASYNC LOAD FOR: " + scenes);
                    StartCoroutine(LoadLevelCoroutine(scenes));
                    break;
                case StreamingAction.Unload:
                    LogDebugInformation("[*] ASYNC UNLOAD FOR: " + scenes);
                    StartCoroutine(UnloadLevelCoroutine(scenes));
                    break;
                default: throw new NotImplementedException("LoadScenesCoroutine does not handle mode " + action.ToString());
            }

            // Wait for all scenes to be loaded
            while (asyncOperations.Any(a => !a.isDone))
                yield return new WaitForEndOfFrame();

            // Then change active scene
            if (sceneToActivate != "")
            {
                var newActive = SceneManager.GetSceneByName(sceneToActivate);
                SceneManager.SetActiveScene(newActive);
                yield return new WaitForEndOfFrame();
            }

            if(DelayAfterLoad >= 0.0f)
                yield return new WaitForSeconds(DelayAfterLoad);

            if (onLoadComplete != null)
                Callable.Call(onLoadComplete);

            if (showUI)
                LoadingRoot.SetActive(false);

            if (EnableDebug)
                DebugText.gameObject.SetActive(false);
        }

        void UpdatePercentage()
        {
            for (int i = 0; i < asyncOperations.Length; i++)
                percentages[i] = asyncOperations[i].progress;
            float percentage = percentages.Sum() / percentages.Length;
            SetProgressBar(percentage);
        }

        private IEnumerator LoadLevelCoroutine(List<string> sceneNames)
        {
            for (int i = 0; i < sceneNames.Count; i++)
            {
                asyncOperations[i] = SceneManager.LoadSceneAsync(sceneNames[i], LoadSceneMode.Additive);
                asyncOperations[i].allowSceneActivation = false;
            }

            LoadingText.text = "Loading...";

            while (asyncOperations.Any(a => a.progress < 0.9f))
            {
                UpdatePercentage();
                yield return new WaitForEndOfFrame();
            }

            // Activate scenes
            foreach (var a in asyncOperations)
                a.allowSceneActivation = true;
            LoadingText.text = "Starting...";
            LogDebugInformation("All scenes loaded");

            while (asyncOperations.Any(a => !a.isDone))
            {
                foreach (var a in asyncOperations)
                    Debug.Log("allowSceneActivation: " + a.allowSceneActivation);
                UpdatePercentage();
                yield return new WaitForEndOfFrame();
            }

            LogDebugInformation("All scenes activated");
        }

        private IEnumerator UnloadLevelCoroutine(List<string> sceneNames)
        {
            for (int i = 0; i < sceneNames.Count; i++)
            {
                if (!SceneManager.GetSceneByName(sceneNames[i]).isLoaded)
                {
                    LogDebugInformation("SKIP UNLOAD: " + sceneNames + " (NOT LOADED)");
                    continue;
                }
                asyncOperations[i] = SceneManager.UnloadSceneAsync(sceneNames[i]);
                asyncOperations[i].allowSceneActivation = false;
            }

            // Remove nulls
            asyncOperations = asyncOperations.Where(a => a != null).ToArray();

            LoadingText.text = "Unloading...";

            while (asyncOperations.Any(a => a.progress < 1.0f))
            {
                UpdatePercentage();
                yield return new WaitForEndOfFrame();
            }

            LogDebugInformation("Unloaded all scenes");
        }

        private float m_CurrentPercentage = 0.0f;
        private float m_TargetPercentage = 0.0f;

        private void SetProgressBar(float percentage, bool direct = false)
        {
            m_TargetPercentage = percentage;
            if (direct)
                m_CurrentPercentage = m_TargetPercentage;
        }

        public void Update()
        {
            // Smoothen bar
            m_CurrentPercentage = Mathf.Lerp(m_CurrentPercentage, m_TargetPercentage, 10 * Time.deltaTime);

            // Update UI
            PercentageText.text = ((int)(m_CurrentPercentage * 100)) + "%";
            Vector2 size = ProgressBar.GetComponent<RectTransform>().sizeDelta;
            size.x = m_CurrentPercentage * ProgressBarContainer.GetComponent<RectTransform>().sizeDelta.x;
            ProgressBar.GetComponent<RectTransform>().sizeDelta = size;

            //Debug
            if (EnableDebug)
            {
                var sb = new System.Text.StringBuilder();
                int count = SceneManager.sceneCount;

                for (int i = 0; i < count; i++)
                {
                    var s = SceneManager.GetSceneAt(i);
                    sb.AppendLine(string.Format("{0} : {1}", s.name, s.isLoaded ? "loaded" : "unloaded"));
                }

                DebugText.text = sb.ToString();
            }

        }

        private void LogDebugInformation(string text)
        {
            if(EnableDebug)
                Debug.Log(text);
        }

    }
}
