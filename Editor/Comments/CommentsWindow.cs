using Boo.Lang;
using System.ComponentModel;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GameplayIngredients.Comments.Editor
{

    public class CommentsWindow : EditorWindow
    {
        [MenuItem("Window/Gameplay Ingredients/Comments")]
        public static void Open()
        {
            EditorWindow.GetWindow<CommentsWindow>();
        }

        private void OnEnable()
        {
            titleContent = EditorGUIUtility.IconContent("console.infoicon.inactive.sml");
            titleContent.text = "Comments";
            Refresh();
        }

        private void OnDisable()
        {

        }

        public enum UserFilter
        {
            MyComments,
            AllComments,
        }

        const string kUserFilterPreference = "GameplayIngredients.Comments.UserFilter";
        const string kUserPreference = "GameplayIngredients.Comments.User";

        static UserFilter userFilter
        {
            get { return (UserFilter)EditorPrefs.GetInt(kUserFilterPreference, 0); }
            set { EditorPrefs.SetInt(kUserFilterPreference, (int)value); }
        }

        static string user
        {
            get { return EditorPrefs.GetString(kUserPreference, "user"); }
            set { EditorPrefs.SetString(kUserPreference, value); }
        }

        bool showInfo = true;
        bool showBugs = true;
        bool showRequests = true;
        bool showTodo = true;

        int infoCount => sceneComments == null ? 0 : sceneComments.Count(o => o.comment.type == CommentType.Info);
        int bugCount => sceneComments == null ? 0 : sceneComments.Count(o => o.comment.type == CommentType.Bug);
        int requestCount => sceneComments == null ? 0 : sceneComments.Count(o => o.comment.type == CommentType.Request);
        int todoCount => sceneComments == null ? 0 : sceneComments.Count(o => o.comment.type == CommentType.ToDo);

        string filter;
        Vector2 scrollPosition;
        private void OnGUI()
        {
            using(new GUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                userFilter = (UserFilter)EditorGUILayout.EnumPopup(userFilter, EditorStyles.toolbarDropDown, GUILayout.Width(140));
                if (GUILayout.Button("Refresh", EditorStyles.toolbarButton))
                    Refresh();
                GUILayout.FlexibleSpace();
                filter = EditorGUILayout.DelayedTextField(filter, EditorStyles.toolbarSearchField, GUILayout.Width(180));
                showInfo = GUILayout.Toggle(showInfo, $"Info ({infoCount})", EditorStyles.toolbarButton);
                showBugs = GUILayout.Toggle(showBugs, $"Bugs ({bugCount})", EditorStyles.toolbarButton);
                showRequests = GUILayout.Toggle(showRequests, $"Requests ({requestCount})", EditorStyles.toolbarButton);
                showTodo = GUILayout.Toggle(showTodo, $"To Do ({todoCount})", EditorStyles.toolbarButton);
            }

            GUI.backgroundColor = Color.white * 1.25f;

            // Header
            using(new GUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                GUILayout.Button("Title", Styles.header, GUILayout.Width(180));
                GUILayout.Button("Description", Styles.header, GUILayout.Width(position.width - 420));
                GUILayout.Button("Reported", Styles.header, GUILayout.Width(80));
                GUILayout.Button("Type", Styles.header, GUILayout.Width(80));
                GUILayout.Button("Status", Styles.header, GUILayout.Width(80));
            }
            GUI.backgroundColor = Color.white;
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            int i = 0;

            foreach(var sceneComment in sceneComments)
            {
                if (sceneComment.comment.type == CommentType.Bug && !showBugs) continue;
                if (sceneComment.comment.type == CommentType.Info && !showInfo) continue;
                if (sceneComment.comment.type == CommentType.Request && !showRequests) continue;
                if (sceneComment.comment.type == CommentType.ToDo && !showTodo) continue;

                GUI.backgroundColor = (i%2 == 0)? Color.white : Color.white * 0.9f;
                using (new GUILayout.HorizontalScope(EditorStyles.toolbar))
                {
                    if(GUILayout.Button(sceneComment.comment.message.title, Styles.line, GUILayout.Width(180)))
                    {
                        Selection.activeGameObject = sceneComment.gameObject;
                    }
                    GUILayout.Label(sceneComment.comment.message.body, Styles.line, GUILayout.Width(position.width - 420));
                    GUILayout.Label(sceneComment.comment.message.from, Styles.line, GUILayout.Width(80));
                    GUILayout.Label(sceneComment.comment.type.ToString(), Styles.line, GUILayout.Width(80));
                    GUILayout.Label(sceneComment.comment.type.ToString(), Styles.line, GUILayout.Width(80));
                    GUILayout.Label(sceneComment.comment.state.ToString(), Styles.line, GUILayout.Width(80));
                }

                i++;
            }
            EditorGUILayout.EndScrollView();
        }

        List<SceneComment> sceneComments;

        void Refresh()
        {
            sceneComments = new List<SceneComment>();
            foreach(var obj in Resources.FindObjectsOfTypeAll(typeof(SceneComment)))
            {
                sceneComments.Add((SceneComment)obj);
            }

        }

        static class Styles
        {
            public static GUIStyle header;
            public static GUIStyle sortHeader;
            public static GUIStyle line;
            public static GUIStyle lineWw;
            static Styles()
            {
                header = new GUIStyle(EditorStyles.toolbarButton);
                header.alignment = TextAnchor.MiddleLeft;
                header.fontStyle = FontStyle.Bold;

                sortHeader = new GUIStyle(EditorStyles.toolbarDropDown);
                sortHeader.alignment = TextAnchor.MiddleLeft;
                sortHeader.fontStyle = FontStyle.Bold;

                line = new GUIStyle(EditorStyles.toolbarButton);
                line.alignment = TextAnchor.MiddleLeft;

                lineWw = new GUIStyle(line);
                lineWw.wordWrap = true;
            }
        }


        [SettingsProvider]
        public static SettingsProvider CommentsPreferences()
        {
            return new SettingsProvider("Preferences/Gameplay Ingredients/Comments", SettingsScope.User)
            {
                label = "Comments",
                guiHandler = (searchContext) =>
                {
                    user = EditorGUILayout.DelayedTextField("Project User Nickname", user);
                }
            };


        }

    }
}
