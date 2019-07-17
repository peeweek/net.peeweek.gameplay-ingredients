using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GameplayIngredients.Editor
{
    partial class WelcomeScreen : EditorWindow
    {
        int tipIndex = 0;

        void InitTips()
        {
            tipIndex = Random.Range(0, tips.Count);
        }

        void OnTipsGUI()
        {
            GUILayout.Label("Tip of the Day", EditorStyles.boldLabel);

            using (new GUILayout.VerticalScope(Styles.helpBox))
            {
                var tip = tips[tipIndex];
                GUILayout.Label(tip.Title, Styles.title);
                GUILayout.Space(12);
                GUILayout.Label(tip.Body, Styles.body);
                GUILayout.FlexibleSpace();
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("<<"))
                    {
                        tipIndex--;
                        if (tipIndex < 0)
                            tipIndex = tips.Count - 1;
                    }
                    if (GUILayout.Button(">>"))
                    {
                        tipIndex++;
                        if (tipIndex == tips.Count)
                            tipIndex = 0;
                    }
                }
            }
        }

        struct Tip
        {
            public string Title;
            public string Body;
        }

        static List<Tip> tips = new List<Tip>()
        {
#if UNITY_2018_3
            new Tip(){ Title = "Show Gizmos", Body = "You can toggle Gizmos on and off by using the Ctrl + , key shortcut."},
#endif
            new Tip(){ Title = "Editor Scene Setups", Body = "You can save your current multi-scene setup to an asset by using the File/Save Current Scene Setup As... or create an Editor Scene Setup asset from the Project View. These assets can be double-clicked to restore all scenes at once."},
            new Tip(){ Title = "Find And Replace", Body = "You can use the Find and Replace window to select, add, and/or refine list of objects in your scene based on different criteria (name, components, etc.). You can then turn this search result into a selection, or replace every object from this list by a prefab or a copy of another game object."},
            new Tip(){ Title = "Game View Link", Body = "Game View link enables you to link your scene view and your game view. To enable it use Ctrl + . or click the 'Game' button in the additional Scene View toolbar. You can define a prefab (named LinkGameViewCamera) containing a camera to setup this camera (for instance if you use HD Render Pipeline and Postprocessing)"},
            new Tip(){ Title = "Hierarchy Window Hints", Body = "You can toggle Hierarchy Window hints by selecting them in the Edit menu. These hints will display an icon for most common components on your game object."},
            new Tip(){ Title = "Scene View POVs", Body = "Scene View POVs enable storing custom point of views in your scenes. To use it, select the POV dropdown in the additional custom toolbar."},
            new Tip(){ Title = "Selection History", Body = "Selection History keeps track of your previously selected objects. You can also star/unstar objects in order to go back at them more easily"},
            new Tip(){ Title = "Play From Here", Body = "Play From Here enables a custom callback when starting your Editor Play Session. Implement the callback and use the scene view camera position and forward vector to generate your custom start function."},
            new Tip(){ Title = "Events, Logic and Actions", Body = "Gameplay Ingredients ships with many Actions, Logic and Events in order to set-up your scenes. Actions perform various actions on your scene objects, Logic trigger actions based on conditions, Events trigger Actions and Logic based on scene interaction (eg: On Trigger Enter)"},
            new Tip(){ Title = "Managers", Body = "Managers are Monobehaviors that instantiate themselves automatically upon startup. You can define a [ManagerDefaultPrefab]"},
        };
    }
}
