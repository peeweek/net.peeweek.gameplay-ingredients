using GameplayIngredients.Rigs;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameplayIngredients.Editor
{
    public class RigDebugWindow : EditorWindow
    {
        [MenuItem("Window/Gameplay Ingredients/Rig Debug", priority = MenuItems.kWindowMenuPriority + 30)]
        public static void OpenWindow()
        {
            OpenWindow(null);
        }

        public static void OpenWindow(Rig selected)
        {
            var wnd = GetWindow<RigDebugWindow>();

            wnd.SelectRig(selected);
        }

        private void OnEnable()
        {
            titleContent = new GUIContent("Rigs Debug",EditorGUIUtility.IconContent("UnityEditor.ProfilerWindow").image);
            Reload();
            m_FilterString = string.Empty;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
            EditorSceneManager.sceneOpened += EditorSceneManager_sceneOpened;
            EditorSceneManager.sceneClosed += EditorSceneManager_sceneClosed;
        }
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
            SceneManager.sceneUnloaded -= SceneManager_sceneUnloaded;
            EditorSceneManager.sceneOpened -= EditorSceneManager_sceneOpened;
            EditorSceneManager.sceneClosed -= EditorSceneManager_sceneClosed;
        }

        private void EditorSceneManager_sceneClosed(Scene scene) { RequestReload(); }

        private void EditorSceneManager_sceneOpened(Scene scene, OpenSceneMode mode) { RequestReload(); }
 
        private void SceneManager_sceneUnloaded(Scene arg0) { RequestReload(); }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1) { RequestReload(); }


        Dictionary<int, List<Rig>> updateRigs;
        Dictionary<int, List<Rig>> fixedUpdateRigs;
        Dictionary<int, List<Rig>> lateUpdateRigs;

        bool m_NeedReload = false;
        void RequestReload()
        {
            m_NeedReload = true;
            Repaint();
        }

        void Reload()
        {
            if (updateRigs == null)
                updateRigs = new Dictionary<int, List<Rig>>();
            else
                updateRigs.Clear();

            if (fixedUpdateRigs == null)
                fixedUpdateRigs = new Dictionary<int, List<Rig>>();
            else
                fixedUpdateRigs.Clear();

            if (lateUpdateRigs == null)
                lateUpdateRigs = new Dictionary<int, List<Rig>>();
            else
                lateUpdateRigs.Clear();


            var all = UnityEngine.Object.FindObjectsOfType<Rig>();
            foreach(var rig in all)
            {
                switch (rig.updateMode)
                {
                    case Rig.UpdateMode.Update:
                        if (!updateRigs.ContainsKey(rig.rigPriority))
                            updateRigs.Add(rig.rigPriority, new List<Rig>());
                        updateRigs[rig.rigPriority].Add(rig);

                        break;
                    case Rig.UpdateMode.LateUpdate:
                        if (!lateUpdateRigs.ContainsKey(rig.rigPriority))
                            lateUpdateRigs.Add(rig.rigPriority, new List<Rig>());
                        lateUpdateRigs[rig.rigPriority].Add(rig);

                        break;
                    case Rig.UpdateMode.FixedUpdate:
                        if (!fixedUpdateRigs.ContainsKey(rig.rigPriority))
                            fixedUpdateRigs.Add(rig.rigPriority, new List<Rig>());
                        fixedUpdateRigs[rig.rigPriority].Add(rig);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            selectedRig = null;

            m_NeedReload = false;
        }

        const string kPrefix = "GameplayIngredients.RigDebugWindow.Foldout.";
        string m_FilterString;
        Vector2 scroll;
        private void OnGUI()
        {
            if (m_NeedReload)
                Reload();

            using (new GUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                if (GUILayout.Button("Reload", EditorStyles.toolbarButton))
                    Reload();
                GUILayout.FlexibleSpace();
                m_FilterString = EditorGUILayout.DelayedTextField(m_FilterString, EditorStyles.toolbarSearchField);
            }
            EditorGUILayout.BeginScrollView(scroll);
            DrawDictEntries(updateRigs, "Update", "update");
            DrawDictEntries(fixedUpdateRigs, "FixedUpdate", "fixedupdate");
            DrawDictEntries(lateUpdateRigs, "LateUpdate", "lateupdate");
            EditorGUILayout.EndScrollView();
        }

        void DrawDictEntries(Dictionary<int,List<Rig>> dict, string label, string prefName)
        {
            if (dict.Keys.Count > 0 && Foldout($"{label}({dict.Keys.Count})", prefName))
            {
                using (new EditorGUI.IndentLevelScope(1))
                    foreach (var key in dict.Keys)
                    {
                        if (Foldout($"#{key}", $"{prefName}-{key}"))
                        {
                            using (new EditorGUI.IndentLevelScope(1))
                                foreach (var rig in dict[key])
                                {
                                    // Filter upon string
                                    if (m_FilterString != string.Empty &&
                                        !rig.gameObject.name.ToLower().Contains(m_FilterString.ToLower()) &&
                                        !rig.GetType().Name.ToLower().Contains(m_FilterString.ToLower())
                                        )
                                        continue;

                                    Rect r = EditorGUILayout.BeginHorizontal();
                                    if (isSelected(rig))
                                        EditorGUI.DrawRect(r, new Color(15f / 256 , 128f / 256, 190f / 256));

                                    var texture = EditorGUIUtility.ObjectContent(rig, rig.GetType()).image;
                                        GUILayout.Space(EditorGUI.indentLevel * 16);
                                        if (GUILayout.Button(new GUIContent($"{rig.gameObject.name} ({rig.GetType().Name})", texture), RigStyle(rig)))
                                            SelectRig(rig);

                                    EditorGUILayout.EndHorizontal();
                                }
                        }
                    }
            }
        }

        Rig selectedRig = null;

        void SelectRig(Rig rig)
        {
            if(rig != null)
            {
                Selection.activeObject = rig;
                RigEditor.PingObject(rig);
            }
            selectedRig = rig;
        }

        GUIStyle RigStyle(Rig rig)
        {
            return isSelected(rig) ? EditorStyles.boldLabel : EditorStyles.label;
        }

        bool isSelected(Rig rig)
        {
            return rig == selectedRig;
        }

        bool Foldout(string label, string name, bool defaultValue = true)
        {
            var value = EditorPrefs.GetBool(name, defaultValue);
            EditorGUI.BeginChangeCheck();
            bool newVal = EditorGUILayout.Foldout(value, label);
            if(EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetBool(name, newVal);
            }
            return newVal;
        }




    }
}

