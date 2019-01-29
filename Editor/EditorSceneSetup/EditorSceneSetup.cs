using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEditor.Callbacks;

namespace GameplayIngredients.Editor
{
    public class EditorSceneSetup : ScriptableObject
    {
        [OnOpenAsset]
        static bool OnOpenAsset(int instanceID, int line)
        {
            var obj = EditorUtility.InstanceIDToObject(instanceID);
            if(obj is EditorSceneSetup)
            {
                EditorSceneSetup setup = (EditorSceneSetup)obj;
                int active = setup.ActiveScene;

                try
                {
                    for (int i = 0; i < setup.LoadedScenes.Length; i++)
                    {
                        var scene = setup.LoadedScenes[i];
                        EditorUtility.DisplayProgressBar("Loading Scenes", string.Format("Loading {0}", scene.Scene.name), (float)i / setup.LoadedScenes.Length - 1);
                        var path = AssetDatabase.GetAssetPath(scene.Scene);
                        EditorSceneManager.OpenScene(path, i == 0 ? OpenSceneMode.Single : (scene.Loaded ? OpenSceneMode.Additive : OpenSceneMode.AdditiveWithoutLoading));
                    }
                    EditorUtility.DisplayProgressBar("Loading Scenes", "Set Active Scene", 1.0f);
                    SceneManager.SetActiveScene(EditorSceneManager.GetSceneAt(active));
                }
                finally
                {
                    EditorUtility.ClearProgressBar();
                }
            }
            return true;
        }


        [MenuItem("Assets/Create/Editor Scene Setup", priority = 200)]
        public static void CreateAsset()
        {
            AssetDatabase.CreateAsset(CreateInstance<EditorSceneSetup>(), "Assets/New Editor Scene Setup.asset");
        }
        
        public int ActiveScene;
        public EditorScene[] LoadedScenes;

        [System.Serializable]
        public struct EditorScene
        {
            public SceneAsset Scene;
            public bool Loaded;
        }
    }
}


