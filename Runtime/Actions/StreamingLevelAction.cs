using System.Collections.Generic;
using UnityEngine.SceneManagement;
using GameplayIngredients.LevelStreaming;
using NaughtyAttributes;

namespace GameplayIngredients.Actions
{
    public class StreamingLevelAction : ActionBase
    {
        [ReorderableList]
        public Scene[] Scenes;
        public string SceneToActivate;
        public LevelStreamingManager.StreamingAction Action = LevelStreamingManager.StreamingAction.Load;

        public bool ShowUI = false;
        
        [ReorderableList]
        public Callable[] OnLoadComplete;

        public override void Execute()
        {
            List<string> sceneNames = new List<string>();
            foreach (var scene in Scenes)
                sceneNames.Add(scene.name);
            Manager.Get<LevelStreamingManager>().LoadScenes(Action, sceneNames.ToArray(), SceneToActivate, ShowUI, OnLoadComplete);
        }
    }
}

