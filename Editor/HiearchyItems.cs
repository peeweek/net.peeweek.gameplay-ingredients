using UnityEngine;
using UnityEditor;
using System.Linq;
using GameplayIngredients.Actions;

namespace GameplayIngredients
{
    static class HiearchyItems
    {
        #region STATE MACHINES
        [MenuItem("GameObject/Gameplay Ingredients/State Machines/State Machine (Empty)", false, 10)]
        static void CreateEmptyStateMachine()
        {
            var go = new GameObject("New StateMachine");
            var sm = go.AddComponent<StateMachines.StateMachine>();

            if (Selection.activeGameObject != null)
                go.transform.parent = Selection.activeGameObject.transform;
        }

        [MenuItem("GameObject/Gameplay Ingredients/State Machines/State Machine (On\\Off)", false, 10)]
        static void CreateTwoStateStateMachine()
        {
            var go = new GameObject("New StateMachine");
            var sm = go.AddComponent<StateMachines.StateMachine>();

            AddState(sm, "On");
            AddState(sm, "Off");

            sm.DefaultState = "On";

            if (Selection.activeGameObject != null)
                go.transform.parent = Selection.activeGameObject.transform;
        }

        [MenuItem("GameObject/Gameplay Ingredients/State Machines/State Machine (On\\Off\\Disabled)", false, 10)]
        static void CreateThreeStateStateMachine()
        {
            var go = new GameObject("New StateMachine");
            var sm = go.AddComponent<StateMachines.StateMachine>();

            AddState(sm, "Disabled");
            AddState(sm, "On");
            AddState(sm, "Off");

            sm.DefaultState = "Disabled";

            if (Selection.activeGameObject != null)
                go.transform.parent = Selection.activeGameObject.transform;
        }

        static StateMachines.State AddState(StateMachines.StateMachine sm, string name)
        {
            var goState = new GameObject(name);
            var state = goState.AddComponent<StateMachines.State>();
            goState.transform.parent = sm.gameObject.transform;
            goState.transform.localPosition = Vector3.zero;
            goState.transform.localRotation = Quaternion.identity;
            goState.transform.localScale = Vector3.one;
            sm.States = sm.States.Concat(new StateMachines.State[] { state }).ToArray();
            return state;
        }

        #endregion

        #region TRIGGERS

        [MenuItem("GameObject/Gameplay Ingredients/Events/On Trigger (Box)", false, 10)]
        static void CreateTriggerBox()
        {
            var go = new GameObject();
            var col = go.AddComponent<BoxCollider>();
            col.isTrigger = true;
            var evt = go.AddComponent<Events.OnTriggerEvent>();
            go.name = "On Box Trigger";

            if (Selection.activeGameObject != null)
                go.transform.parent = Selection.activeGameObject.transform;
        }

        [MenuItem("GameObject/Gameplay Ingredients/Events/On Trigger (Sphere)", false, 10)]
        static void CreateTriggerSphere()
        {
            var go = new GameObject();
            var col = go.AddComponent<SphereCollider>();
            col.isTrigger = true;
            var evt = go.AddComponent<Events.OnTriggerEvent>();
            go.name = "On Sphere Trigger";

            if (Selection.activeGameObject != null)
                go.transform.parent = Selection.activeGameObject.transform;
        }

        [MenuItem("GameObject/Gameplay Ingredients/Events/On Trigger (Capsule)", false, 10)]
        static void CreateTriggerCapsule()
        {
            var go = new GameObject();
            var col = go.AddComponent<CapsuleCollider>();
            col.isTrigger = true;
            var evt = go.AddComponent<Events.OnTriggerEvent>();
            go.name = "On Capsule Trigger";

            if (Selection.activeGameObject != null)
                go.transform.parent = Selection.activeGameObject.transform;
        }
        #endregion

        #region LIFECYCLE

        [MenuItem("GameObject/Gameplay Ingredients/Events/On Awake", false, 10)]
        static void CreateOnAwake()
        {
            var go = new GameObject();
            var evt = go.AddComponent<Events.OnAwakeEvent>();
            go.name = "On Awake";

            if (Selection.activeGameObject != null)
                go.transform.parent = Selection.activeGameObject.transform;
        }

        [MenuItem("GameObject/Gameplay Ingredients/Events/On Enable", false, 10)]
        static void CreateOnEnableDisable()
        {
            var go = new GameObject();
            var evt = go.AddComponent<Events.OnEnableDisableEvent>();
            go.name = "On Enable/Disable";

            if (Selection.activeGameObject != null)
                go.transform.parent = Selection.activeGameObject.transform;
        }

        [MenuItem("GameObject/Gameplay Ingredients/Events/On Start", false, 10)]
        static void CreateOnStart()
        {
            var go = new GameObject();
            var evt = go.AddComponent<Events.OnStartEvent>();
            go.name = "On Start";

            if (Selection.activeGameObject != null)
                go.transform.parent = Selection.activeGameObject.transform;
        }

        [MenuItem("GameObject/Gameplay Ingredients/Events/On Game Manager Start", false, 10)]
        static void CreateOnGameManagerStart()
        {
            var go = new GameObject();
            var evt = go.AddComponent<Events.OnGameManagerLevelStart>();
            go.name = "On Game Manager Level Start";

            if (Selection.activeGameObject != null)
                go.transform.parent = Selection.activeGameObject.transform;
        }

        [MenuItem("GameObject/Gameplay Ingredients/Events/On Message Received", false, 10)]
        static void CreateOnMessageReceived()
        {
            var go = new GameObject();
            var evt = go.AddComponent<Events.OnMessageEvent>();
            go.name = "On Message Received";

            if (Selection.activeGameObject != null)
                go.transform.parent = Selection.activeGameObject.transform;
        }
        #endregion

        #region UTILS
        [MenuItem("GameObject/Gameplay Ingredients/Factory", false, 10)]
        static void CreateFactory()
        {
            var go = new GameObject("Factory");
            var fact = go.AddComponent<Factory>();
            fact.SpawnTarget = go;
            var sa = go.AddComponent<FactorySpawnAction>();
            sa.factory = fact;

            if (Selection.activeGameObject != null)
                go.transform.parent = Selection.activeGameObject.transform;
        }
        #endregion
    }
}

