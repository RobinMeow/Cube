using System;
using UnityEditor;

public sealed class HorizontalGroup : IDisposable
{
    public HorizontalGroup()
    {
        EditorGUILayout.BeginHorizontal();
    }

    public void Dispose()
    {
        EditorGUILayout.EndHorizontal();
    }
}
