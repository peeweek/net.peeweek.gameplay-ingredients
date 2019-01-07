using UnityEngine;
using UnityEditor;

namespace GameplayIngredients
{
    static class HiearchyItems
    {
        #region TRIGGERS

        [MenuItem("GameObject/GameplayIngredients/Hooks/Trigger (Box)", false, 10)]
        static void CreateTriggerBox()
        {
            var go = new GameObject();
            var col = go.AddComponent<BoxCollider>();
            col.isTrigger = true;
            var hook = go.AddComponent<Hooks.OnTriggerHook>();
            go.name = "Box Trigger";

            if (Selection.activeGameObject != null)
                go.transform.parent = Selection.activeGameObject.transform;
        }

        [MenuItem("GameObject/GameplayIngredients/Hooks/Trigger (Sphere)", false, 10)]
        static void CreateTriggerSphere()
        {
            var go = new GameObject();
            var col = go.AddComponent<SphereCollider>();
            col.isTrigger = true;
            var hook = go.AddComponent<Hooks.OnTriggerHook>();
            go.name = "Sphere Trigger";

            if (Selection.activeGameObject != null)
                go.transform.parent = Selection.activeGameObject.transform;
        }

        [MenuItem("GameObject/GameplayIngredients/Hooks/Trigger (Capsule)", false, 10)]
        static void CreateTriggerCapsule()
        {
            var go = new GameObject();
            var col = go.AddComponent<CapsuleCollider>();
            col.isTrigger = true;
            var hook = go.AddComponent<Hooks.OnTriggerHook>();
            go.name = "Capsule Trigger";

            if (Selection.activeGameObject != null)
                go.transform.parent = Selection.activeGameObject.transform;
        }

        [MenuItem("GameObject/GameplayIngredients/Hooks/On Awake", false, 10)]
        static void CreateOnAwake()
        {
            var go = new GameObject();
            var hook = go.AddComponent<Hooks.OnAwakeHook>();
            go.name = "OnAwake Hook";

            if (Selection.activeGameObject != null)
                go.transform.parent = Selection.activeGameObject.transform;
        }
        #endregion

    }
}

