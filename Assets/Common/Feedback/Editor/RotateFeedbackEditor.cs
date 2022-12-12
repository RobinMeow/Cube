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
    static readonly GUILayoutOption MAX_WIDTH_50 = GUILayout.MaxWidth(50.0f);
    static readonly GUILayoutOption MAX_WIDTH_90 = GUILayout.MaxWidth(90.0f);
    static readonly GUILayoutOption MAX_WIDTH_30 = GUILayout.MaxWidth(30.0f);
    
    SerializedProperty _targetProp = null;
    SerializedProperty _durationProp = null;
    SerializedProperty _restorePreviousProp = null;
    SerializedProperty _loopProp = null;
    SerializedProperty _multiplierXProp = null;
    SerializedProperty _multiplierYProp = null;
    SerializedProperty _multiplierZProp = null;
    SerializedProperty _curveXProp = null;
    SerializedProperty _curveYProp = null;
    SerializedProperty _curveZProp = null;

    void OnEnable()
    {
        _rotateFeedback = (RotateFeedback)target;
        _targetProp = serializedObject.FindProperty("_target");
        _durationProp = serializedObject.FindProperty("_duration");
        _restorePreviousProp = serializedObject.FindProperty("_restorePrevious");
        _loopProp = serializedObject.FindProperty("_loop");
        _multiplierXProp = serializedObject.FindProperty("_multiplierX");
        _multiplierYProp = serializedObject.FindProperty("_multiplierY");
        _multiplierZProp = serializedObject.FindProperty("_multiplierZ");
        _curveXProp = serializedObject.FindProperty("_curveX");
        _curveYProp = serializedObject.FindProperty("_curveY");
        _curveZProp = serializedObject.FindProperty("_curveZ");
    }

    // ToDo:
    // -transform values display 
    // -stop test, (stop coroutine) when is on loop

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        DrawProperties();

        EditorGUILayout.Space();

        DrawTestButton();

        serializedObject.ApplyModifiedProperties();
    }

    void DrawProperties()
    {
        // properties 
        _targetProp.objectReferenceValue = EditorGUILayout.ObjectField("Target", _targetProp.objectReferenceValue, typeof(Transform), allowSceneObjects: true);
        _durationProp.floatValue = EditorGUILayout.FloatField("Duration", _durationProp.floatValue);
        _restorePreviousProp.boolValue = EditorGUILayout.Toggle("Restore Values", _restorePreviousProp.boolValue);
        _loopProp.boolValue = EditorGUILayout.Toggle("Loop", _loopProp.boolValue);

        EditorGUILayout.Space();

        GUIContent curveLabelContent = new GUIContent("Curves", multiplierToolTip);
        EditorGUILayout.LabelField(curveLabelContent, MAX_WIDTH_50);

        // Multipliers for AnimationCurve-KeyValues 
        EditorGUILayout.BeginHorizontal();
        {
            Vector3Int curveMultipliers = EditorGUILayout.Vector3IntField("Multipliers", new Vector3Int(_multiplierXProp.intValue, _multiplierYProp.intValue, _multiplierZProp.intValue));
            _multiplierXProp.intValue = curveMultipliers.x;
            _multiplierYProp.intValue = curveMultipliers.y;
            _multiplierZProp.intValue = curveMultipliers.z;
        }
        EditorGUILayout.EndHorizontal();

        // Animation Curves
        _curveXProp.animationCurveValue = EditorGUILayout.CurveField("X", _curveXProp.animationCurveValue);
        _curveYProp.animationCurveValue = EditorGUILayout.CurveField("Y", _curveYProp.animationCurveValue);
        _curveZProp.animationCurveValue = EditorGUILayout.CurveField("Z", _curveZProp.animationCurveValue);
    }

    void DrawTestButton()
    {
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
    }
}
