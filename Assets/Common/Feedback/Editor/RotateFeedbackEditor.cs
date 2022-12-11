using System;
using System.IO.Ports;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RotateFeedback))]
public sealed class RotateFeedbackEditor : Editor
{
    RotateFeedback _rotateFeedback = null;
    const string multiplierToolTip = "you can keep the curve between 0 and 1 values. This value cales it up to your input. f.e. 360 when you want to make full rotation.";
    static readonly char[] _axis = new char[] { 'X', 'Y', 'Z' };

    void OnEnable()
    {
        _rotateFeedback = (RotateFeedback)target;
    }

    public override void OnInspectorGUI()
    {
        //serializedObject.Update();

        GUILayoutOption MAX_WIDTH_50 = GUILayout.MaxWidth(50);

        SerializedObject serializedObj = serializedObject;
        
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("Target", MAX_WIDTH_50); 
            EditorGUILayout.ObjectField(serializedObj.FindProperty("_target").objectReferenceValue, typeof(Transform), allowSceneObjects: true);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("Duration", MAX_WIDTH_50);
            EditorGUILayout.FloatField(serializedObj.FindProperty("_duration").floatValue, MAX_WIDTH_50);
            EditorGUILayout.LabelField("Restore Values", GUILayout.MaxWidth(90));
            EditorGUILayout.Toggle(serializedObj.FindProperty("_restorePrevious").boolValue);
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space();

        foreach (char axis in _axis)
        {
            EditorGUILayout.BeginHorizontal();
            {
                GUIContent curveLabelContent = new GUIContent("Curve " + axis);
                curveLabelContent.tooltip = multiplierToolTip;
                EditorGUILayout.LabelField(curveLabelContent, MAX_WIDTH_50);
                EditorGUILayout.IntField(serializedObj.FindProperty("_multiplier" + axis).intValue, MAX_WIDTH_50);
                EditorGUILayout.CurveField(serializedObj.FindProperty("_curve" + axis).animationCurveValue);
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Test", EditorStyles.miniButtonMid))
        {
            if (_rotateFeedback == null)
            {
                Debug.LogError($"I think this never happens. Unless OnInspectorGUI is called before OnEnabled. (????)?????");
                return;
            }

            if (!_rotateFeedback.IsRotating)
            {
                EditorCoroutineUtility.StartCoroutineOwnerless(_rotateFeedback.Rotating());
            }
            else
            {
                // Running multiple Coroutines will cause the coroutines, after the first one, to remember and restore wrong values. 
                _rotateFeedback.Log($"Wait for the rotation to finish, before testing again.\nThis ensures, the 'Restore Values' function works propperly.");
            }
        }

        //serializedObject.ApplyModifiedProperties();
    }
}
