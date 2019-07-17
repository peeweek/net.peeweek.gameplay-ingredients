using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GameplayIngredients.Editor
{
    partial class WelcomeScreen : EditorWindow
    {
        void OnSetupGUI()
        {
            GUILayout.Label("First Time Setup", EditorStyles.boldLabel);

            using (new GUILayout.VerticalScope(Styles.helpBox))
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
    }
}

