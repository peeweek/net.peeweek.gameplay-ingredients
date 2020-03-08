using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GameplayIngredients.Editor
{
    public class CheckWindow : EditorWindow
    {
        [MenuItem("Window/Gameplay Ingredients/Check and Resolve")]
        static void OpenWindow()
        {
            GetWindow<CheckWindow>(false);
        }

        private void OnEnable()
        {
            this.titleContent = new GUIContent("Check/Resolve");
            this.minSize = new Vector2(640, 180);
            InitializeCheckStates();
        }

        void InitializeCheckStates()
        {
            s_CheckStates = new Dictionary<Check, bool>();
            foreach (var check in Check.allChecks)
            {
                s_CheckStates.Add(check, EditorPrefs.GetBool(kPreferencePrefix + check.name, check.defaultEnabled));
            }
        }

        void SetCheckState(Check check, bool value)
        {
            s_CheckStates[check] = value;
            EditorPrefs.SetBool(kPreferencePrefix + check.name, value);
        }

        const string kPreferencePrefix = "GameplayIngrediensts.Check.";

        Vector2 Scroll = new Vector2();
        Dictionary<Check, bool> s_CheckStates;

        private void OnGUI()
        {
            using(new GUILayout.HorizontalScope(EditorStyles.toolbar, GUILayout.Height(22)))
            {
                if (GUILayout.Button("Check", EditorStyles.toolbarButton))
                    PerformChecks();
                if(GUILayout.Button("", EditorStyles.toolbarPopup))
                {
                    Rect r = new Rect(0, 0, 16, 20);
                    GenericMenu menu = new GenericMenu();
                    foreach (Check check in s_CheckStates.Keys)
                    {
                        menu.AddItem(new GUIContent(check.name), s_CheckStates[check], () => SetCheckState(check, !s_CheckStates[check]));
                    }
                    menu.DropDown(r);
                }

                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Resolve", EditorStyles.toolbarButton))
                    Resolve();

            }
            using(new GUILayout.VerticalScope())
            {
               
                GUI.backgroundColor = Color.white * 1.3f;
                using (new GUILayout.HorizontalScope(EditorStyles.toolbar))
                {
                    GUILayout.Label("Check Type", Styles.header, GUILayout.Width(128));
                    GUILayout.Label("Object", Styles.header, GUILayout.Width(128));
                    GUILayout.Label("Message", Styles.header, GUILayout.ExpandWidth(true));
                    GUILayout.Label("Resolution", Styles.header, GUILayout.Width(128));
                    GUILayout.Space(12);
                }

                Scroll = GUILayout.BeginScrollView(Scroll, false,true);

                if (m_Results != null && m_Results.Count > 0)
                {
                    bool odd = true;
                    foreach (var result in m_Results)
                    {
                        GUI.backgroundColor = Color.white * (odd ? 0.9f : 0.8f);
                        odd = !odd;

                        using (new GUILayout.HorizontalScope(EditorStyles.toolbar))
                        {
                            GUILayout.Label(result.check.name, Styles.line, GUILayout.Width(128));
                            if(GUILayout.Button(result.mainObject.name, Styles.line, GUILayout.Width(128)))
                            {
                                Selection.activeObject = result.mainObject;
                            }
                            GUILayout.Label(result.message, Styles.line, GUILayout.ExpandWidth(true));
                            ShowMenu(result);

                        }
                    }

                }
                else
                {
                    GUILayout.Label("No Results");
                }
                GUI.backgroundColor = Color.white;

                GUILayout.FlexibleSpace();
                GUILayout.EndScrollView();

            }
        }

        void ShowMenu(CheckResult<Check> result)
        {
            if (s_IntValues == null)
                s_IntValues = new Dictionary<Check, int[]>();

            if(!s_IntValues.ContainsKey(result.check))
            {
                int count = result.check.ResolutionActions.Length;
                int[] values = new int[count];
                for(int i = 0; i < count; i++)
                {
                    values[i] = i;
                }
                s_IntValues.Add(result.check, values);
            }

            result.resolutionActionIndex = EditorGUILayout.IntPopup(result.resolutionActionIndex, result.check.ResolutionActions, s_IntValues[result.check], EditorStyles.toolbarDropDown, GUILayout.Width(128));

        }

        static Dictionary<Check, int[]> s_IntValues;

        List<CheckResult<Check>> m_Results;

        void Resolve()
        {

        }

        void PerformChecks()
        {
            List<CheckResult<Check>> results = new List<CheckResult<Check>>();
            bool canceled = false;
            try
            {
                int count = s_CheckStates.Count;
                int i = 0;
                foreach (var kvp in s_CheckStates)
                {
                    float t = (float)i / count;
                    if (EditorUtility.DisplayCancelableProgressBar("Performing Checks", kvp.Key.name, t))
                    {
                        canceled = true;
                        break;
                    }

                    if (kvp.Value)
                        results.AddRange(kvp.Key.GetResults());
                    i++;
                }
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }

            if(!canceled)
                m_Results = results;

            Repaint();
        }

        static class Styles
        {
            public static GUIStyle header;
            public static GUIStyle line;

            static Styles()
            {
                header = new GUIStyle(EditorStyles.toolbarButton);
                header.alignment = TextAnchor.MiddleLeft;
                header.fontStyle = FontStyle.Bold;

                line = new GUIStyle(EditorStyles.toolbarButton);
                line.alignment = TextAnchor.MiddleLeft;
            }
        }

    }

}
