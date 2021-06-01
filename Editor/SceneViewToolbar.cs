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
    public static class SceneViewToolbar
    {
#if UNITY_2021_2_OR_NEWER

        [Overlay(typeof(SceneView), "Gameplay Ingredients", true)]
        public class IngredientsToolbarOverlay : ToolbarOverlay
        {
            const string prefix = "IngredientsToolbarOverlay.";
            public IngredientsToolbarOverlay() : base(PlayFromHereButton.id, LinkGameViewButton.id, PointOfViewButton.id, CommentsButton.id, CheckResolveButton.id) { }

            protected override Layout supportedLayouts => Layout.HorizontalToolbar | Layout.VerticalToolbar;

            #region PLAY FROM HERE
            [EditorToolbarElement(id)]
            public class PlayFromHereButton : EditorToolbarButton, IAccessContainerWindow
            {
                public const string id = prefix + "PlayFromHereButton";
                public EditorWindow containerWindow { get; set; }

                public PlayFromHereButton() : base()
                {
                    this.SetEnabled(PlayFromHere.IsReady);
                    icon = EditorApplication.isPlaying ? Contents.playFromHere_Stop : Contents.playFromHere;
                    tooltip = "Play from Here";
                    clicked += OnClick;
                    EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;
                }

                private void EditorApplication_playModeStateChanged(PlayModeStateChange obj)
                {
                    if (obj == PlayModeStateChange.EnteredPlayMode)
                        icon = Contents.playFromHere_Stop;
                    else if (obj == PlayModeStateChange.EnteredEditMode)
                        icon = Contents.playFromHere;
                }

                public void OnClick()
                {
                    if (!EditorApplication.isPlaying)
                        PlayFromHere.Play(containerWindow as SceneView);
                    else
                        EditorApplication.isPlaying = false;
                }
            }
            #endregion

            #region LINK GAME VIEW
            [EditorToolbarElement(id)]
            public class LinkGameViewButton : EditorToolbarDropdownToggle, IAccessContainerWindow
            {
                public override bool value { 
                    get => base.value;
                    set 
                    { 
                        base.value = value;
                        OnValueChanged(value);
                    }
                }

                public const string id = prefix + "LinkGameViewButton";

                public EditorWindow containerWindow { get; set; }

                static List<LinkGameViewButton> buttons = new List<LinkGameViewButton>();

                public LinkGameViewButton()
                {
                    icon = Contents.linkGameView;
                    tooltip = "Link Game View";
                    dropdownClicked += OnClick;

                    if (LinkGameView.Active && LinkGameView.LockedSceneView == null)
                        LinkGameView.Active = false;

                    buttons.Add(this);
                }

                ~LinkGameViewButton()
                {
                    if (LinkGameView.LockedSceneView == containerWindow as SceneView)
                    {
                        LinkGameView.LockedSceneView = null;
                        LinkGameView.Active = false;
                    }
                    buttons.Remove(this);
                }

                public void OnClick()
                {

                }

                void OnValueChanged(bool newValue)
                {
                    LinkGameView.Active = newValue;

                    if (newValue)
                        LinkGameView.LockedSceneView = containerWindow as SceneView;
                    else
                        LinkGameView.LockedSceneView = null;

                    foreach(var button in buttons)
                    {
                        if(button.containerWindow != containerWindow)
                        {
                            button.SetValueWithoutNotify(false);
                        }
                    }
                }
            }
            #endregion

            #region POINT OF VIEW
            [EditorToolbarElement(id)]
            public class PointOfViewButton : EditorToolbarDropdown, IAccessContainerWindow
            {
                public const string id = prefix + "PointOfViewButton";

                public EditorWindow containerWindow { get; set; }

                public PointOfViewButton()
                {
                    icon = Contents.pointOfView;
                    tooltip = "Point of View";
                    clicked += OnClick;
                }

                public void OnClick()
                {

                }
            }
            #endregion

            #region COMMENTS
            [EditorToolbarElement(id)]
            public class CommentsButton : EditorToolbarButton, IAccessContainerWindow
            {
                public const string id = prefix + "CommentsButton";

                public EditorWindow containerWindow { get; set; }

                public CommentsButton()
                {
                    icon = Contents.commentsWindow;
                    tooltip = "Open Comments Window";
                    clicked += OnClick;
                }

                public void OnClick()
                {
                    CommentsWindow.Open();
                }
            }
            #endregion

            #region CHECK/RESOLVE
            [EditorToolbarElement(id)]
            public class CheckResolveButton : EditorToolbarButton, IAccessContainerWindow
            {
                public const string id = prefix + "CheckResolveButton";

                public EditorWindow containerWindow { get; set; }

                public CheckResolveButton()
                {
                    icon = Contents.checkWindow;
                    tooltip = "Open Check/Resolve Window";
                    clicked += OnClick;
                }

                public void OnClick()
                {
                    CheckWindow.OpenWindow();
                }
            }
            #endregion

            static class Contents
            {
                public static Texture2D playFromHere;
                public static Texture2D playFromHere_Stop;

                public static Texture2D pointOfView;

                public static Texture2D linkGameView;

                public static Texture2D checkWindow;
                public static Texture2D commentsWindow;

                static Contents()
                {
                    playFromHere = EditorGUIUtility.Load("Packages/net.peeweek.gameplay-ingredients/Icons/SceneViewToolbar/PlayFromHere.png") as Texture2D;
                    playFromHere_Stop = EditorGUIUtility.Load("Packages/net.peeweek.gameplay-ingredients/Icons/SceneViewToolbar/PlayFromHere_Stop.png") as Texture2D;

                    pointOfView = EditorGUIUtility.Load("Packages/net.peeweek.gameplay-ingredients/Icons/SceneViewToolbar/POV.png") as Texture2D;

                    linkGameView = EditorGUIUtility.Load("Packages/net.peeweek.gameplay-ingredients/Icons/GUI/Camera16x16.png") as Texture2D;

                    checkWindow = EditorGUIUtility.IconContent("Valid").image as Texture2D;

                    commentsWindow = EditorGUIUtility.IconContent("console.infoicon.inactive.sml").image as Texture2D;
                }
            }
        }
#else
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

