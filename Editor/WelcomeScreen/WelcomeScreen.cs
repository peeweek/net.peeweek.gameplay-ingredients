using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GameplayIngredients.Editor
{
    class WelcomeScreen : EditorWindow
    {
        const string kShowOnStartupPreference = "GameplayIngredients.Welcome.ShowAtStartup";
        const int WindowWidth = 640;
        const int WindowHeight = 520;

        static bool showOnStartup
        {
            get { return EditorPrefs.GetBool(kShowOnStartupPreference, true); }
            set { if (value != showOnStartup) EditorPrefs.SetBool(kShowOnStartupPreference, value); }
        }

        static readonly Texture2D header = (Texture2D)EditorGUIUtility.Load("Packages/net.peeweek.gameplay-ingredients/Editor/WelcomeScreen/welcome-title.png");

        [InitializeOnLoadMethod]
        static void InitShowAtStartup()
        {
            if (showOnStartup)
                EditorApplication.update += ShowAtStartup;
                
        }

        static void ShowAtStartup()
        {
            if(!Application.isPlaying)
            {
                ShowFromMenu();
            }
            EditorApplication.update -= ShowAtStartup;
        }

        [MenuItem("Window/Gameplay Ingredients/Startup Wizard")]
        static void ShowFromMenu()
        {
            GetWindow<WelcomeScreen>(true, "Gameplay Ingredients");
        }

        private void OnEnable()
        {
            this.position = new Rect((Screen.width / 2.0f) - WindowWidth/2, (Screen.height / 2.0f) - WindowHeight/2, WindowWidth, WindowHeight);
            this.minSize = new Vector2(WindowWidth, WindowHeight);
            this.maxSize = new Vector2(WindowWidth, WindowHeight);

            if (!GameplayIngredientsSettings.hasSettingAsset)
                wizardMode = WizardMode.FirstTimeSetup;
        }

        private void OnDestroy()
        {
            EditorApplication.update -= ShowAtStartup;
        }

        private enum WizardMode
        {
            TipOfTheDay = 0,
            FirstTimeSetup = 1,
            Configuration = 2,
        }

        [SerializeField]
        private WizardMode wizardMode = WizardMode.TipOfTheDay;

        private void OnGUI()
        {
            Rect headerRect = GUILayoutUtility.GetRect(640, 215);
            GUI.DrawTexture(headerRect, header);
            RectOffset headerButtonsOffset = new RectOffset(200, 200, 160, 16);
            using (new GUILayout.AreaScope(new Rect(160, 180, 320, 32)))
            {
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("  Tips  ", Styles.buttonLeft)) wizardMode = WizardMode.TipOfTheDay;
                    if (GUILayout.Button("  Setup  ", Styles.buttonMid)) wizardMode = WizardMode.FirstTimeSetup;
                    EditorGUI.BeginDisabledGroup(true);
                    if (GUILayout.Button("  Configuration  ", Styles.buttonRight)) wizardMode = WizardMode.Configuration;
                    EditorGUI.EndDisabledGroup();
                    GUILayout.FlexibleSpace();
                }
            }
            GUILayout.Space(8);

            switch (wizardMode)
            {
                case WizardMode.TipOfTheDay: TipOfTheDayGUI();
                    break;
                case WizardMode.FirstTimeSetup: FirstTimeSetupGUI();
                    break;
                case WizardMode.Configuration: ConfigurationGUI();
                    break;
            }

            Rect line = GUILayoutUtility.GetRect(640, 1);
            EditorGUI.DrawRect(line, Color.black);
            using (new GUILayout.HorizontalScope())
            {
                showOnStartup = GUILayout.Toggle(showOnStartup, " Show this window on startup");
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Close"))
                {
                    this.Close();
                }
            }
        }

        int tipIndex = 0;

        void TipOfTheDayGUI()
        {
            GUILayout.Label("Tip of the Day", EditorStyles.boldLabel);
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                var tip = tips[tipIndex];
                GUILayout.Label(tip.Title, Styles.title);
                GUILayout.Space(12);
                GUILayout.Label(tip.Body, Styles.body);
                GUILayout.FlexibleSpace();
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.FlexibleSpace();
                    if(GUILayout.Button("<<"))
                    {
                        tipIndex--;
                        if (tipIndex < 0)
                            tipIndex = tips.Count - 1;
                    }
                    if(GUILayout.Button(">>"))
                    {
                        tipIndex++;
                        if (tipIndex == tips.Count)
                            tipIndex = 0;
                    }
                }
            }
        }
        void FirstTimeSetupGUI()
        {
            GUILayout.Label("First Time Setup", EditorStyles.boldLabel);

            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                GUILayout.Label("Welcome to Gameplay Ingredients !", Styles.title);
                GUILayout.Space(12);
                GUILayout.Label(@"This wizard will help you set up your project so you can use and customize scripts.

GameplayIngredients is a framework that comes with a variety of features : these can be configured in a <b>GameplayIngredientsSettings</b> asset.

This asset needs to be stored in a Resources folder.
While this is not mandatory we advise you to create it in order to be able to modify it.
", Styles.body);
                GUILayout.Space(8);
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Create GameplayIngredientsSettings Asset"))
                    {
                        GameplayIngredientsSettings asset = Instantiate<GameplayIngredientsSettings>(GameplayIngredientsSettings.defaultSettings);
                        AssetDatabase.CreateAsset(asset, "Assets/Resources/GameplayIngredientsSettings.asset");
                        Selection.activeObject = asset;
                    }
                }
                GUILayout.FlexibleSpace();
            }
        }
        void ConfigurationGUI()
        {
            GUILayout.Label("Configuration", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
        }

        struct Tip
        {
            public string Title;
            public string Body;
        }

        static List<Tip> tips = new List<Tip>()
        {
            new Tip(){ Title = "Show Gizmos", Body = "You can toggle Gizmos on and off by using the Ctrl + , key shortcut."},
            new Tip(){ Title = "Editor Scene Setups", Body = "You can save your current multi-scene setup to an asset by using the File/Save Current Scene Setup As... or create an Editor Scene Setup asset from the Project View. These assets can be double-clicked to restore all scenes at once."},
            new Tip(){ Title = "Find And Replace", Body = "You can use the Find and Replace window to select, add, and/or refine list of objects in your scene based on different criteria (name, components, etc.). You can then turn this search result into a selection, or replace every object from this list by a prefab or a copy of another game object."},
            new Tip(){ Title = "Game View Link", Body = "Game View link enables you to link your scene view and your game view. To enable it use Ctrl + . or click the 'Game' button in the additional Scene View toolbar. You can define a prefab (named LinkGameViewCamera) containing a camera to setup this camera (for instance if you use HD Render Pipeline and Postprocessing)"},
            new Tip(){ Title = "Hierarchy Window Hints", Body = "You can toggle Hierarchy Window hints by selecting them in the Edit menu. These hints will display an icon for most common components on your game object."},
            new Tip(){ Title = "Scene View POVs", Body = "Scene View POVs enable storing custom point of views in your scenes. To use it, select the POV dropdown in the additional custom toolbar."},
            new Tip(){ Title = "Selection History", Body = "Selection History keeps track of your previously selected objects. You can also star/unstar objects in order to go back at them more easily"},
            new Tip(){ Title = "Play From Here", Body = "Play From Here enables a custom callback when starting your Editor Play Session. Implement the callback and use the scene view camera position and forward vector to generate your custom start function."},
            new Tip(){ Title = "Events, Logic and Actions", Body = "Gameplay Ingredients ships with many Actions, Logic and Events in order to set-up your scenes. Actions perform various actions on your scene objects, Logic trigger actions based on conditions, Events trigger Actions and Logic based on scene interaction (eg: On Trigger Enter)"},
            new Tip(){ Title = "Managers", Body = "Managers are Monobehaviors that instantiate themselves automatically upon startup. You can define a []"},
        };
    }

    static class Styles
    {
        public static GUIStyle buttonLeft;
        public static GUIStyle buttonMid;
        public static GUIStyle buttonRight;
        public static GUIStyle title;
        public static GUIStyle body;

        static Styles()
        {
            buttonLeft = new GUIStyle(EditorStyles.miniButtonLeft);
            buttonMid = new GUIStyle(EditorStyles.miniButtonMid);
            buttonRight = new GUIStyle(EditorStyles.miniButtonRight);
            buttonLeft.fontSize = 12;
            buttonMid.fontSize = 12;
            buttonRight.fontSize = 12;

            title = new GUIStyle(EditorStyles.label);    
            title.fontSize = 22;

            body = new GUIStyle(EditorStyles.label);
            body.fontSize = 12;
            body.wordWrap = true;
            body.richText = true;
        }
    }
}
