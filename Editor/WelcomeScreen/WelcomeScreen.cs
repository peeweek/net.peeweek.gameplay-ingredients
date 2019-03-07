using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GameplayIngredients.Editor
{
    class WelcomeScreen : EditorWindow
    {
        const string kShowOnStartupPreference = "GameplayIngredients.Welcome.ShowAtStartup";

        static bool showOnStartup
        {
            get { return EditorPrefs.GetBool(kShowOnStartupPreference, true); }
            set { if (value != showOnStartup) EditorPrefs.SetBool(kShowOnStartupPreference, value); }
        }

        static readonly Texture2D header = (Texture2D)EditorGUIUtility.Load("Packages/net.peeweek.gameplay-ingredients/Editor/WelcomeScreen/welcome-title.png");

        [InitializeOnLoadMethod]
        static void ShowOnStartup()
        {
            if (showOnStartup)
                GetWindow<WelcomeScreen>(true, "Gameplay Ingredients");
        }

        [MenuItem("Window/Gameplay Ingredients/Startup Wizard")]
        static void ShowFromMenu()
        {
            GetWindow<WelcomeScreen>(true, "Gameplay Ingredients");
        }

        private void OnEnable()
        {
            this.position = new Rect((Screen.width / 2.0f) - 320, (Screen.height / 2.0f) - 200, 640, 400);
            this.minSize = new Vector2(640, 400);
            this.maxSize = new Vector2(640, 400);
        }

        private enum WizardMode
        {
            TipOfTheDay = 0,
            FirstTimeSetup = 1,
            Configuration = 2,
        }

        private WizardMode wizardMode = WizardMode.TipOfTheDay;

        private void OnGUI()
        {
            Rect headerRect = GUILayoutUtility.GetRect(640, 128);
            GUI.DrawTexture(headerRect, header);
            GUILayout.Space(8);
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Tips", Styles.buttonLeft)) wizardMode = WizardMode.TipOfTheDay;
                EditorGUI.BeginDisabledGroup(true);
                if (GUILayout.Button("Setup", Styles.buttonMid)) wizardMode = WizardMode.FirstTimeSetup;
                if (GUILayout.Button("Configuration", Styles.buttonRight)) wizardMode = WizardMode.Configuration;
                EditorGUI.EndDisabledGroup();
                GUILayout.FlexibleSpace();
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
                showOnStartup = GUILayout.Toggle(showOnStartup, "Show this window on startup");
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
                GUILayout.Label(tip.Title, Styles.tipTitle);
                GUILayout.Space(12);
                GUILayout.Label(tip.Body, Styles.tipBody);
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
        }
        void ConfigurationGUI()
        {
            GUILayout.Label("Configuration", EditorStyles.boldLabel);
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
        public static GUIStyle tipTitle;
        public static GUIStyle tipBody;

        static Styles()
        {
            buttonLeft = new GUIStyle(EditorStyles.miniButtonLeft);
            buttonMid = new GUIStyle(EditorStyles.miniButtonMid);
            buttonRight = new GUIStyle(EditorStyles.miniButtonRight);
            buttonLeft.fontSize = 12;
            buttonMid.fontSize = 12;
            buttonRight.fontSize = 12;

            tipTitle = new GUIStyle(EditorStyles.label);    
            tipTitle.fontSize = 22;

            tipBody = new GUIStyle(EditorStyles.label);
            tipBody.fontSize = 12;
            tipBody.wordWrap = true;
        }
    }
}
