using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;
using UnityEngine.Events;

namespace GameplayIngredients.Editor
{
    public class EmptyGameObjectCheck : Check
    {
        public override string name => "EmptyGameObject";

        public override bool defaultEnabled => true;

        public override string[] ResolutionActions => new string[] { "Do Nothing", "Set Static", "Delete Object" };

        public override int defaultResolutionActionIndex => 0;

        public override IEnumerable<CheckResult> GetResults()
        {
            GameObject[] all = GameObject.FindObjectsOfType<GameObject>();

            GetReferencedObjects(all);

            try
            {
                int count = all.Length;
                int i = 0;

                foreach (var go in all)
                {
                    float progress = ++i / count;
                    if (EditorUtility.DisplayCancelableProgressBar("Finding Empty Game Objects...", $"{go.name}", progress))
                    {
                        break;
                    }

                    var allComps = go.GetComponents<Component>();
                    if (allComps.Length == 1)
                    {
                        if (!go.isStatic)
                        {
                            if(!referencedGameObjects.Contains(go))
                            {
                                var result = new CheckResult(this, CheckResult.Result.Warning, $"Empty Game Object {go.name} is not static", go);
                                result.resolutionActionIndex = 1;
                                yield return result;
                            }
                        }
                        else
                        {
                            if (go.transform.childCount == 0)
                            {
                                if (!referencedGameObjects.Contains(go))
                                {
                                    var result =  new CheckResult(this, CheckResult.Result.Notice, "Empty Static Game Object has no children and could be deleted if unused.", go);
                                    result.resolutionActionIndex = 2;
                                    yield return result;
                                }
                            }
                        }
                    }
                }

            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }
        public List<GameObject> referencedGameObjects;
        public List<Component> referencedComponents;
        public List<Object> referencedObjects;

        public void GetReferencedObjects(GameObject[] sceneObjects)
        {
            referencedGameObjects = new List<GameObject>();
            referencedComponents = new List<Component>();
            referencedObjects = new List<Object>();

            if (sceneObjects == null || sceneObjects.Length == 0)
                return;

            try
            {
                int count = sceneObjects.Length;
                int i = 0;

                foreach (var sceneObject in sceneObjects)
                {
                    float progress = ++i / (float)count;
                    if(EditorUtility.DisplayCancelableProgressBar("Building Reference Table...", $"{sceneObject.name}", progress))
                    {
                        referencedComponents.Clear();
                        referencedGameObjects.Clear();
                        referencedObjects.Clear();
                        break;
                    }

                    var components = sceneObject.GetComponents<Component>();
                    foreach (var component in components)
                    {
                        if (!(component is Transform))
                        {
                            Type t = component.GetType();
                            foreach (var f in t.GetFields())
                            {

                                if (f.FieldType == typeof(GameObject))
                                {
                                    var go = f.GetValue(component) as GameObject;
                                    if (go != component.gameObject)
                                    {
                                        referencedGameObjects.Add(go);
                                    }
                                }
                                else if (f.FieldType == typeof(Transform))
                                {
                                    var tr = f.GetValue(component) as Transform;
                                    if (tr.gameObject != component.gameObject)
                                    {
                                        referencedGameObjects.Add(tr.gameObject);
                                    }
                                }
                                else if (f.FieldType.IsSubclassOf(typeof(Component)))
                                {
                                    var comp = f.GetValue(component) as Component;
                                    if (comp != null && comp.gameObject != component.gameObject)
                                    {
                                        referencedComponents.Add(comp);
                                    }
                                }
                                else if (f.FieldType.IsSubclassOf(typeof(Object)))
                                {
                                    var obj = f.GetValue(component) as Object;
                                    referencedObjects.Add(obj);
                                } 
                            }
                        }
                    }
                }
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        public override void Resolve(int action)
        {
            throw new System.NotImplementedException();
        }
    }
}

