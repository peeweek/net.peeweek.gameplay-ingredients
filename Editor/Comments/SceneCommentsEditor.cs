using UnityEditor;
using UnityEngine;

namespace GameplayIngredients.Comments.Editor
{
    [CustomEditor(typeof(SceneComment))]
    public class SceneCommentsEditor : UnityEditor.Editor
    {
        private void OnEnable()
        {
            SceneView.lastActiveSceneView.AlignViewToObject((serializedObject.targetObject as SceneComment).transform);
        }

        public override void OnInspectorGUI()
        {
            if(GUILayout.Button("Open Comments", GUILayout.Height(24)))
                CommentsWindow.Open();
        }
    }
}
