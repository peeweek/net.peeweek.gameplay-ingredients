using System;
using UnityEngine;

namespace GameplayIngredients.Comments
{
    [Serializable]
    public struct Comment
    {
        public CommentMessage message;
        public CommentType type;
        public CommentState state;
    }

    [Serializable]
    public struct CommentMessage
    {
        public string from;
        public string[] attn;
        public string title;
        public string URL;
        public string body;
        public object[] targets;
        public CommentMessage[] replies;
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
