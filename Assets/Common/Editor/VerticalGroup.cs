using System;
using UnityEditor;

public sealed class VerticalGroup : IDisposable
{
    public VerticalGroup()
    {
        EditorGUILayout.BeginVertical();
    }

    public void Dispose()
    {
        EditorGUILayout.EndVertical();
    }
}
