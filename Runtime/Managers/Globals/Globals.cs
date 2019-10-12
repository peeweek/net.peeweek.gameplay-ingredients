using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals
{
    static Dictionary<string, bool> globalBooleans;
    static Dictionary<string, int> globalInts;
    static Dictionary<string, string> globalStrings;
    static Dictionary<string, float> globalFloats;
    static Dictionary<string, GameObject> globalObjects;

    static Dictionary<string, bool> localBooleans;
    static Dictionary<string, int> localInts;
    static Dictionary<string, string> localStrings;
    static Dictionary<string, float> localFloats;
    static Dictionary<string, GameObject> localObjects;

    public enum Scope
    {
        Local,
        Global
    }
    public enum Type
    {
        Boolean,
        Integer,
        String,
        Float,
        GameObject
    }

    static Globals()
    {
        globalBooleans = new Dictionary<string, bool>();
        globalInts = new Dictionary<string, int>();
        globalStrings = new Dictionary<string, string>();
        globalFloats = new Dictionary<string, float>();
        globalObjects = new Dictionary<string, GameObject>();


        localBooleans = new Dictionary<string, bool>();
        localInts = new Dictionary<string, int>();
        localStrings = new Dictionary<string, string>();
        localFloats = new Dictionary<string, float>();
        localObjects = new Dictionary<string, GameObject>();
    }

    public static void ResetLocals()
    {
        localBooleans.Clear();
        localInts.Clear();
        localStrings.Clear();
        localFloats.Clear();
        localObjects.Clear();
    }
    public static void ResetGlobals()
    {
        globalBooleans.Clear();
        globalInts.Clear();
        globalStrings.Clear();
        globalFloats.Clear();
        globalObjects.Clear();
    }


    #region Has()
    public static bool HasBool(string name, Scope scope)
    {
        switch (scope)
        {
            default:
            case Scope.Local:
                return localBooleans.ContainsKey(name);
            case Scope.Global:
                return globalBooleans.ContainsKey(name);
        }
    }
    public static bool HasInt(string name, Scope scope)
    {
        switch (scope)
        {
            default:
            case Scope.Local:
                return localInts.ContainsKey(name);
            case Scope.Global:
                return globalInts.ContainsKey(name);
        }
    }
    public static bool HasString(string name, Scope scope)
    {
        switch (scope)
        {
            default:
            case Scope.Local:
                return localStrings.ContainsKey(name);
            case Scope.Global:
                return globalStrings.ContainsKey(name);
        }
    }
    public static bool HasFloat(string name, Scope scope)
    {
        switch (scope)
        {
            default:
            case Scope.Local:
                return localFloats.ContainsKey(name);
            case Scope.Global:
                return globalFloats.ContainsKey(name);
        }
    }
    public static bool HasObject(string name, Scope scope)
    {
        switch (scope)
        {
            default:
            case Scope.Local:
                return localObjects.ContainsKey(name);
            case Scope.Global:
                return globalObjects.ContainsKey(name);
        }
    }
    #endregion

    #region Get()
    public static bool GetBool(string name, Scope scope)
    {
        switch (scope)
        {
            default:
            case Scope.Local:
                return localBooleans[name];
            case Scope.Global:
                return globalBooleans[name];
        }
    }
    public static int GetInt(string name, Scope scope)
    {
        switch (scope)
        {
            default:
            case Scope.Local:
                return localInts[name];
            case Scope.Global:
                return globalInts[name];
        }
    }
    public static string GetString(string name, Scope scope)
    {
        switch (scope)
        {
            default:
            case Scope.Local:
                return localStrings[name];
            case Scope.Global:
                return globalStrings[name];
        }
    }
    public static float GetFloat(string name, Scope scope)
    {
        switch (scope)
        {
            default:
            case Scope.Local:
                return localFloats[name];
            case Scope.Global:
                return globalFloats[name];
        }
    }
    public static bool GetObject(string name, Scope scope)
    {
        switch (scope)
        {
            default:
            case Scope.Local:
                return localObjects[name];
            case Scope.Global:
                return globalObjects[name];
        }
    }
    #endregion

    #region Set()

    static void SetValue<T>(Dictionary<string, T> dict, string name, T value)
    {
        if (dict.ContainsKey(name))
            dict[name] = value;
        else
            dict.Add(name, value);
    }

    public static void SetBool(string name, bool value, Scope scope)
    {
        switch (scope)
        {
            default:
            case Scope.Local:
                SetValue(localBooleans, name, value); return;
            case Scope.Global:
                SetValue(globalBooleans, name, value); return;
        }
    }
    public static void SetInt(string name, int value, Scope scope)
    {
        switch (scope)
        {
            default:
            case Scope.Local:
                SetValue(localInts, name, value); return;
            case Scope.Global:
                SetValue(globalInts, name, value); return;
        }
    }
    public static void SetString(string name, string value, Scope scope)
    {
        switch (scope)
        {
            default:
            case Scope.Local:
                SetValue(localStrings, name, value); return;
            case Scope.Global:
                SetValue(globalStrings, name, value); return;
        }
    }
    public static void SetFloat(string name, float value, Scope scope)
    {
        switch (scope)
        {
            default:
            case Scope.Local:
                SetValue(localFloats, name, value); return;
            case Scope.Global:
                SetValue(globalFloats, name, value); return;
        }
    }
    public static void SetObject(string name, GameObject value, Scope scope)
    {
        switch (scope)
        {
            default:
            case Scope.Local:
                SetValue(localObjects, name, value); return;
            case Scope.Global:
                SetValue(globalObjects, name, value); return;
        }
    }
    #endregion

}
