using UnityEditor;
using UnityEngine.SceneManagement;

namespace GameplayIngredients.Utils
{
    public class PathUtils
    {
        public static string GetCurrentSceneResourcesPath()
        {
            // Method 1: Search into Assets/Resources/<SceneName>
            string path = "Assets/Resources/" + SceneManager.GetActiveScene().name;
            if (AssetDatabase.IsValidFolder(path))
            {
                return path;
            }

            // Method 2: Search into the current scene path the directory "Resources"
            path = SceneManager.GetActiveScene().path
                .Replace(SceneManager.GetActiveScene().name + ".unity", "Resources");
            if (AssetDatabase.IsValidFolder(path))
            {
                return path; 
            }
            
            return null;
        }
    }
}