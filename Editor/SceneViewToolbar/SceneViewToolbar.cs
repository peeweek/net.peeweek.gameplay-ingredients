using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using GameplayIngredients.Comments.Editor;
using UnityEditor.Overlays;
using UnityEditor.Toolbars;
using UnityEngine.UIElements;

namespace GameplayIngredients.Editor
{
    public static partial class SceneViewToolbar
    {

#if !UNITY_2021_2_OR_NEWER
        public delegate void SceneViewToolbarDelegate(SceneView sceneView);

        public static event SceneViewToolbarDelegate OnSceneViewToolbarGUI;

        [InitializeOnLoadMethod]
        static void Initialize()
        {

            SceneView.duringSceneGui += OnSceneGUI;

        }

        private static void OnSceneGUI(SceneView sceneView)
        {
            if (!Preferences.showToolbar)
                return;

            var r = new Rect(Vector2.zero, new Vector2(sceneView.position.width,24));
            Handles.BeginGUI();
            using (new GUILayout.AreaScope(r))
            {
                using (new GUILayout.HorizontalScope(EditorStyles.toolbar))
                {

                    if(Preferences.showPlayFromHere)
                    {
                        using(new EditorGUI.DisabledGroupScope(!PlayFromHere.IsReady))
                        {
                            bool play = GUILayout.Toggle(EditorApplication.isPlaying, Contents.playFromHere, EditorStyles.toolbarButton);

                            if (GUI.changed)
                            {
                                if (play)
                                    PlayFromHere.Play(sceneView);
                                else
                                    EditorApplication.isPlaying = false;
                            }

                        }
                        GUILayout.Space(24);
                    }

                    if(Preferences.showLinkGameView)
                    {

                        Color backup = GUI.color;

                        bool isLinked = LinkGameView.Active;
                        bool isLocked = LinkGameView.LockedSceneView == sceneView;


                        if (isLinked && isLocked)
                        {
                            GUI.color = Styles.lockedLinkColor * 2;
                        }
                        else if (isLinked && LinkGameView.CinemachineActive)
                        {
                            GUI.color = Styles.cineColor * 2;
                        }

                        isLinked = GUILayout.Toggle(isLinked, LinkGameView.CinemachineActive ? Contents.linkGameViewCinemachine : Contents.linkGameView, EditorStyles.toolbarButton, GUILayout.Width(64));

                        if (GUI.changed)
                        {
                            if (Event.current.shift)
                            {
                                if (!LinkGameView.Active)
                                    LinkGameView.Active = true;

                                LinkGameView.CinemachineActive = !LinkGameView.CinemachineActive;
                            }
                            else
                            {
                                LinkGameView.Active = isLinked;
                                LinkGameView.CinemachineActive = false;
                            }
                        }

                        isLocked = GUILayout.Toggle(isLocked, Contents.lockLinkGameView, EditorStyles.toolbarButton);

                        if (GUI.changed)
                        {
                            if (isLocked)
                            {
                                LinkGameView.CinemachineActive = false;
                                LinkGameView.LockedSceneView = sceneView;
                            }
                            else
                            {
                                LinkGameView.LockedSceneView = null;
                            }
                        }

                        GUI.color = backup;
                        GUILayout.Space(16);
                    }

                    if(Preferences.showPOV)
                    {
                        if (GUILayout.Button("POV", EditorStyles.toolbarDropDown))
                        {
                            Rect btnrect = GUILayoutUtility.GetLastRect();
                            btnrect.yMax += 17;
                            SceneViewPOV.ShowPopup(btnrect, sceneView);
                        }
                        GUILayout.Space(16);
                    }

                    if(Preferences.showCheck)
                    {
                        if (GUILayout.Button(Contents.checkWindow, EditorStyles.toolbarButton))
                        {
                            EditorWindow.GetWindow<CheckWindow>();
                        }
                        GUILayout.Space(16);
                    }

                    if(Preferences.showComments)
                    {
                        if (GUILayout.Button(Contents.commentsWindow, EditorStyles.toolbarButton))
                        {
                            CommentsWindow.Open();
                        }
                        if (GUILayout.Button(Contents.addComment, EditorStyles.toolbarButton))
                        {
                            SceneCommentEditor.CreateComment();
                        }
                    }

                    if(Preferences.showCustom)
                    {
                        // Custom Callback here
                        if (OnSceneViewToolbarGUI != null)
                            OnSceneViewToolbarGUI.Invoke(sceneView);
                    }

                    GUILayout.FlexibleSpace();
                    // Saving Space not to overlap view controls
                    GUILayout.Space(96);
                }
            }

            if (LinkGameView.CinemachineActive)
            {
                DisplayText("CINEMACHINE PREVIEW", Styles.cineColor);
            }
            else if (LinkGameView.Active)
            {
                if (LinkGameView.LockedSceneView == sceneView)
                {
                    DisplayText("GAME VIEW LINKED (LOCKED)", Styles.lockedLinkColor);
                }
                else if(LinkGameView.LockedSceneView == null && SceneView.lastActiveSceneView == sceneView)
                {
                    DisplayText("GAME VIEW LINKED", Color.white);
                }
            }

            Handles.EndGUI();
        }

        static void DisplayText(string text, Color color)
        {
            Rect r = new Rect(16, 24, 512, 32);
            GUI.color = Color.black;
            GUI.Label(r, text);
            r.x--;
            r.y--;
            GUI.color = color;
            GUI.Label(r, text);
            GUI.color = Color.white;
        }

        static class Preferences
        {
            // Public Getters
            public static bool showToolbar => Get(kShowToolbar);
            public static bool showPlayFromHere => Get(kShowPlayFromHere);
            public static bool showLinkGameView => Get(kShowLinkGameView);
            public static bool showPOV => Get(kShowPOV);
            public static bool showCheck => Get(kShowCheck);
            public static bool showComments => Get(kShowComments);
            public static bool showCustom => Get(kShowCustom);

            const string kShowToolbar = "showToolbar";
            const string kShowPlayFromHere = "showPlayFromHere";
            const string kShowLinkGameView = "showLinkGameView";
            const string kShowPOV = "showPOV";
            const string kShowCheck = "showCheck";
            const string kShowComments = "showComments";
            const string kShowCustom = "showCustom";

            const string kPrefPrefix = "GameplayIngredients.SceneViewToolbar";
            static readonly Dictionary<string, bool> defaults = new Dictionary<string, bool>
            {
                { kShowToolbar , true },
                { kShowPlayFromHere , true },
                { kShowLinkGameView , true },
                { kShowPOV , true },
                { kShowCheck , true },
                { kShowComments , true },
                { kShowCustom , true },
            };

            static bool Default(string name)
            {
                if (defaults.ContainsKey(name))
                    return defaults[name];
                else
                    return false;
            }

            static bool Get(string name)
            {
                return EditorPrefs.GetBool($"{kPrefPrefix}.{name}", Default(name));
            }

            static void Set(string name, bool value)
            {
                EditorPrefs.SetBool($"{kPrefPrefix}.{name}", value);
            }

            [SettingsProvider]
            public static SettingsProvider GetPreferences()
            {
                static void OnToggleAll(bool toggle)
                {
                    OnToggleLinkGameView(toggle);
                }

                static void OnToggleLinkGameView(bool toggle)
                {
                    if (!toggle)
                        LinkGameView.Active = false;
                }

                static void PreferenceItem(string label, string name, Action<bool> onToggle = null)
                {
                    EditorGUI.BeginChangeCheck();
                    bool val = EditorGUILayout.Toggle(label, Get(name));
                    if (EditorGUI.EndChangeCheck())
                    {
                        Set(name, val);
                        onToggle?.Invoke(val);
                        SceneView.RepaintAll();
                    }
                };

                return new SettingsProvider("Preferences/Gameplay Ingredients/Scene View", SettingsScope.User)
                {
                    label = "Scene View",
                    guiHandler = (searchContext) =>
                    {
                        PreferenceItem("Show Toolbar", "showToolbar", OnToggleAll);
                        using (new EditorGUI.IndentLevelScope(1))
                        {
                            PreferenceItem("Play From Here", kShowPlayFromHere);
                            PreferenceItem("Link Game View", kShowLinkGameView, OnToggleLinkGameView);
                            PreferenceItem("Point of View", kShowPOV);
                            PreferenceItem("Check Window", kShowCheck);
                            PreferenceItem("Comments Window", kShowComments);
                            PreferenceItem("Custom Toolbar Items", kShowCustom);
                        }
                    }
                };
            }


        }

        static class Contents
        {
            public static GUIContent playFromHere;
            public static GUIContent lockLinkGameView;
            public static GUIContent linkGameView;
            public static GUIContent linkGameViewCinemachine;
            public static GUIContent checkWindow;
            public static GUIContent commentsWindow;
            public static GUIContent addComment;

            static Contents()
            {
                lockLinkGameView = new GUIContent(EditorGUIUtility.IconContent("IN LockButton"));
                linkGameView = new GUIContent(EditorGUIUtility.Load("Packages/net.peeweek.gameplay-ingredients/Icons/GUI/Camera16x16.png") as Texture);
                linkGameView.text = " Game";

                linkGameViewCinemachine = new GUIContent(EditorGUIUtility.Load("Packages/net.peeweek.gameplay-ingredients/Icons/GUI/Camera16x16.png") as Texture);
                linkGameViewCinemachine.text = " Cine";

                playFromHere = new GUIContent(EditorGUIUtility.IconContent("Animation.Play"));
                playFromHere.text = "Here";

                checkWindow = new GUIContent(EditorGUIUtility.IconContent("Valid"));
                checkWindow.text = "Check";

                commentsWindow = new GUIContent(EditorGUIUtility.IconContent("console.infoicon.inactive.sml"));
                commentsWindow.text = "Comments";

                addComment = new GUIContent("+");
            }
        }

        static class Styles
        {
            public static GUIStyle toolbar;
            public static Color lockedLinkColor = new Color(0.5f, 1.0f, 0.1f, 1.0f);
            public static Color cineColor = new Color(1.0f, 0.5f, 0.1f, 1.0f);

            static Styles()
            {
                toolbar = new GUIStyle(EditorStyles.inspectorFullWidthMargins);                
            }
        }
#endif
    }
}

