using UnityEditor;
using UnityEngine.SceneManagement;

namespace GameplayIngredients.Utils
{
    public class PathUtils
    {
        public static string GetCurrentSceneResourcesPath()
        {
            string path = SceneManager.GetActiveScene().path
                .Replace(SceneManager.GetActiveScene().name + ".unity", "Resources");
            if (AssetDatabase.IsValidFolder(path))
            {
                return path; 
            }
            return null;
        }
    }
}