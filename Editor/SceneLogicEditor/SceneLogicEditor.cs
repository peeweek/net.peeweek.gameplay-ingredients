using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GraphProcessor;
using System.Reflection;
using GameplayIngredients.Events;
using GameplayIngredients.Logic;
using GameplayIngredients.Actions;

public class SceneLogicEditor : BaseGraphWindow
{
    // TODO : When usable, uncomment this
    //[MenuItem("Window/Gameplay Ingredients/Scene Logic")]
    static void OpenLogicEditor()
    {
        GetWindow<SceneLogicEditor>();
    }

    protected override void Initialize(BaseGraph graph)
    {
        titleContent = Contents.title;

    }

    static class Contents
    {
        public static readonly GUIContent title = new GUIContent("Scene Logic");
    }

    public List<Type> hookTypes;
    public List<Type> logicTypes;
    public List<Type> actionTypes;

    void PopulateAllTypes()
    {
        var result = new List<Type>();
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        var hookBase = typeof(EventBase);
        var logicBase = typeof(LogicBase);
        var actionBase = typeof(ActionBase);

        hookTypes = new List<Type>();
        logicTypes = new List<Type>();
        actionTypes = new List<Type>();

        foreach (var assemly in assemblies)
        {
            Type[] types = assemly.GetTypes();
            foreach (var type in types)
            {
                if (hookBase.IsAssignableFrom(type) && !type.IsAbstract)
                {
                    hookTypes.Add(type);
                }
                else if (logicBase.IsAssignableFrom(type) && !type.IsAbstract)
                {
                    logicTypes.Add(type);
                }
                else if (hookBase.IsAssignableFrom(type) && !type.IsAbstract)
                {
                    actionTypes.Add(type);
                }
            }
        }
    }
}
