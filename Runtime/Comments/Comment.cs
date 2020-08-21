using System;
using UnityEngine;

namespace GameplayIngredients.Comments
{
    [Serializable]
    public struct Comment
    {
        public string user;
        public string attn;
        public string title;
        public string URL;
        public string body;
        public object target;
        public Vector3 position;
        public Quaternion rotation;
        public CommentType type;
        public Comment[] replies;
    }

    [Serializable]
    public enum CommentType
    {
        Info,
        Bug,
        Request,
        ToDo,
    }

    [Serializable]
    public enum CommentState
    {
        Open,
        Blocked,
        Resolved,
        WontFix,
        Closed,
    }

}
