using System;
using System.Diagnostics;
using UnityEngine;
using UnityObject = UnityEngine.Object;

public static class GameObjectUnityConsoleLogger 
{
    [Conditional("UNITY_EDITOR")] 
    static void log(Action<string, UnityObject> logAction, string prefixColor, string prefix, UnityObject obj, params object[] messages)
    {
        logAction($"<color={prefixColor}>{prefix}</color>[<color=lightblue>{obj.name}</color>]: {String.Join("<color=#00ff99> | </color>", messages)}\n ", obj);
    }

    [Conditional("UNITY_EDITOR")] 
    public static void Log(this UnityObject obj, params object[] messages)
    {
        
        log(UnityEngine.Debug.Log, "#ffffff", "       ", obj, messages);
    }

    [Conditional("UNITY_EDITOR")]
    public static void LogError(this UnityObject obj, params object[] messages)
    {
        log(UnityEngine.Debug.LogError, "#ff0000", "Error  ", obj, messages);
    }

    [Conditional("UNITY_EDITOR")]
    public static void LogWarning(this UnityObject obj, params object[] messages)
    {
        log(UnityEngine.Debug.LogWarning, "#ffff00", "Warning", obj, messages);
    }

    [Conditional("UNITY_EDITOR")]
    public static void LogSuccess(this UnityObject obj, params object[] messages)
    {
        log(UnityEngine.Debug.Log, "#66ff00", "Success", obj, messages);
    }
}
