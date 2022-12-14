using System;
using UnityEditor;

public sealed class DisabledGroup : IDisposable
{
    public DisabledGroup(bool disabled)
    {
        EditorGUI.BeginDisabledGroup(disabled);
    }

    public void Dispose()
    {
        EditorGUI.EndDisabledGroup();
    }
}
