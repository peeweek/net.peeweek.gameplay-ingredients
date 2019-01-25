using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace GameplayIngredients.Editor
{
    public class EditorSceneSetup : ScriptableObject
    {
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


