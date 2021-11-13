using System;
using GameplayIngredients.Utils;
using UnityEngine;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace GameplayIngredients
{
    [HelpURL(Help.URL + "settings")]
    public class GameplayIngredientsSettings : ScriptableObject
    {
	    public string[] includedManagers { get { return m_IncludedManagers;  } }
        public string[] excludedManagers { get { return m_ExcludedManagers; } }
        public bool verboseCalls { get { return m_VerboseCalls; } }
        public bool allowUpdateCalls { get { return m_AllowUpdateCalls; } }

        public bool disableWelcomeScreenAutoStart { get { return m_DisableWelcomeScreenAutoStart; } }

        [BoxGroup("Editor")]
        [SerializeField]
        protected bool m_DisableWelcomeScreenAutoStart;

        [BoxGroup("Managers")] 
        [SerializeField, ReorderableList, IncludedManager]
        protected string[] m_IncludedManagers;
        
        [BoxGroup("Managers")] 
        [SerializeField, ReorderableList, ExcludedManager]
        protected string[] m_ExcludedManagers;

        [BoxGroup("Callables")]
        [SerializeField, InfoBox("Verbose Calls enable logging at runtime, this can lead to performance drop, use only when debugging.", EInfoBoxType.Warning)]
        private bool m_VerboseCalls = false;

        [BoxGroup("Callables")]
        [SerializeField, InfoBox("Per-update calls should be avoided due to high performance impact. Enable and use with care, only if strictly necessary.", EInfoBoxType.Warning)]
        private bool m_AllowUpdateCalls = false;

        const string kAssetName = "GameplayIngredientsSettings.asset";

        private static string GetSettingsPath()
        {
            string settingsPath;
            string sceneResourcePath = PathUtils.GetCurrentSceneResourcesPath();
            if (sceneResourcePath != null)
            {
                settingsPath = sceneResourcePath + "/" + kAssetName;
            }
            else
            {
                settingsPath = "Assets/Resources/" + kAssetName;
            }

            return settingsPath;
        }
        
        public static GameplayIngredientsSettings currentSettings
        {
            get
            {
                if (hasSettingAsset)
                {
                    string settingsPath = GetSettingsPath();
                    return AssetDatabase.LoadAssetAtPath<GameplayIngredientsSettings>(settingsPath);
                }
                else
                {
                    return defaultSettings;
                }
            }
        }

        public static bool hasSettingAsset
        {
            get
            {
                string settingsPath = GetSettingsPath();
                return AssetDatabase.LoadAssetAtPath<GameplayIngredientsSettings>(settingsPath) != null;
            }
        }

        public static GameplayIngredientsSettings defaultSettings
        {
            get
            {
                if (s_DefaultSettings == null)
                    s_DefaultSettings = CreateDefaultSettings();
                return s_DefaultSettings;
            }
        }

        static GameplayIngredientsSettings s_DefaultSettings;

        static GameplayIngredientsSettings CreateDefaultSettings()
        {
            var defaultAsset = CreateInstance<GameplayIngredientsSettings>();
            defaultAsset.m_VerboseCalls = false;
            defaultAsset.m_IncludedManagers = Array.Empty<string>();
            defaultAsset.m_ExcludedManagers = Array.Empty<string>();
            defaultAsset.m_DisableWelcomeScreenAutoStart = false;
            return defaultAsset;
        }
    }
}
