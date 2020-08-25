using NaughtyAttributes;
using System;
using UnityEditor;
using UnityEngine;

namespace GameplayIngredients.Comments
{
    [Serializable]
    public struct Comment
    {
        public string title;
        public CommentMessage message;
        public CommentType type;
        public CommentState state;
        public CommentPriority priority;
    }

    [Serializable]
    public struct CommentMessage
    {
        public string from;

        public string URL;
        [Multiline]
        public string body;
        [ReorderableList]
        public UnityEngine.Object[] targets;
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

    [Serializable]
    public enum CommentPriority
    {
        High,
        Medium,
        Low,
    }

}
