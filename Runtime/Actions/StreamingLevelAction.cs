using UnityEngine.Events;
using GameplayIngredients;
using GameplayIngredients.LevelStreaming;

namespace GameplayIngredients.Actions
{
    public class StreamingLevelAction : ActionBase
    {
        public string[] Scenes;
        public string SceneToActivate;
        public LevelStreamingManager.StreamingAction Action = LevelStreamingManager.StreamingAction.Load;

        public bool ShowUI = false;

        public UnityEvent OnLoadComplete;

        public override void Execute()
        {
            Manager.Get<LevelStreamingManager>().LoadScenes(Action, Scenes, SceneToActivate, ShowUI, OnLoadComplete);
        }
    }
}

