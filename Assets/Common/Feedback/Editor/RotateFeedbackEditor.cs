using System;
using System.Collections;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RotateFeedback))]
public sealed class RotateFeedbackEditor : Editor
{
    const object WAIT_FOR_NEXT_FRAME = null;
    RotateFeedback _rotateFeedback = null;
    const string multiplierToolTip = "you can keep the curve between 0 and 1 values. This value cales it up to your input. f.e. 360 when you want to make full rotation.";
    static readonly GUILayoutOption MAX_WIDTH_50 = GUILayout.MaxWidth(50.0f);
    
    SerializedProperty _targetProp = null;
    SerializedProperty _durationProp = null;
    SerializedProperty _restorePreviousProp = null;
    SerializedProperty _loopProp = null;
    SerializedProperty _multipliersProp = null;
    SerializedProperty _curveXProp = null;
    SerializedProperty _curveYProp = null;
    SerializedProperty _curveZProp = null;

    // for Testing 
    EditorCoroutine _testRotation = null;
    EditorCoroutine _startTestCoroutine = null;

    void OnEnable()
    {
        _rotateFeedback = (RotateFeedback)target;
        _targetProp = serializedObject.FindProperty("_target");
        _durationProp = serializedObject.FindProperty("_duration");
        _restorePreviousProp = serializedObject.FindProperty("_restorePrevious");
        _loopProp = serializedObject.FindProperty("_loop");
        _multipliersProp = serializedObject.FindProperty("_multipliers");
        _curveXProp = serializedObject.FindProperty("_curveX");
        _curveYProp = serializedObject.FindProperty("_curveY");
        _curveZProp = serializedObject.FindProperty("_curveZ");
    }

    // ToDo:
    // -transform values display 

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //serializedObject.Update();
        
        //DrawProperties();

        EditorGUILayout.Space();

        if (!Application.isPlaying)
            DrawTestButton();

        //serializedObject.ApplyModifiedProperties();
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
        _multipliersProp.vector3IntValue = EditorGUILayout.Vector3IntField("Multipliers", _multipliersProp.vector3IntValue);

        // Animation Curves
        _curveXProp.animationCurveValue = EditorGUILayout.CurveField("X", _curveXProp.animationCurveValue);
        _curveYProp.animationCurveValue = EditorGUILayout.CurveField("Y", _curveYProp.animationCurveValue);
        _curveZProp.animationCurveValue = EditorGUILayout.CurveField("Z", _curveZProp.animationCurveValue);
    }
    
    bool IsTestRunning() => _startTestCoroutine != null;
    
    void DrawTestButton()
    {
        bool testPressed = false;
        bool stopTestPressed = false;

        bool testIsRunning = IsTestRunning();
        
        using (new HorizontalGroup())
        {
            using (new DisabledGroup(disabled: testIsRunning))
                testPressed = GUILayout.Button("Test", EditorStyles.miniButtonLeft);

            using (new DisabledGroup(disabled: !testIsRunning))
                stopTestPressed = GUILayout.Button("Stop Test", EditorStyles.miniButtonRight);
        }

        if (testPressed) 
            StartTest();
        else if (stopTestPressed)
            StopTest();

        if (testIsRunning)
            DrawTargetRotationValues();
    }
    
    void DrawTargetRotationValues()
    {
        using (new DisabledGroup(disabled: true))
        {
            EditorGUILayout.Vector3Field("Target", _rotateFeedback.Target.localEulerAngles);
        }

        //_logInScene = EditorGUILayout.Toggle("Log in Scene", _logInScene);
        //if (_logInScene)
        //{
        //    _labelOffset = EditorGUILayout.Vector3Field("Scene Log Offset", _labelOffset);
        //}
    }
    
    //bool _logInScene = false;
    //Vector3 _labelOffset = new Vector3(1.0f, 1.0f, 0.0f);
    //void OnSceneGUI()
    //{
    //    if (_logInScene && IsTestRunning())
    //    {
    //        Vector3 labelPosition = _rotateFeedback.Target.position + _labelOffset;
    //        Handles.Label(labelPosition, $"[{_rotateFeedback.name}] TestRotation: {_rotateFeedback.Target.localEulerAngles}");
    //    }
    //}

    void StartTest()
    {
        if (!IsTestRunning())
        {
            _startTestCoroutine = EditorCoroutineUtility.StartCoroutineOwnerless(StartTestRotation()); // dont block the Inspector UI 
            IEnumerator StartTestRotation()
            {
                _testRotation = EditorCoroutineUtility.StartCoroutineOwnerless(_rotateFeedback.Rotating());

                bool rotationStarted = false;
                do
                {
                    yield return WAIT_FOR_NEXT_FRAME;
                    
                    if (_rotateFeedback.IsRotating)
                        rotationStarted = true;
                } 
                while (!rotationStarted);
            
                do
                {
                    yield return WAIT_FOR_NEXT_FRAME;
                } 
                while (_rotateFeedback.IsRotating);

                _testRotation = null;
                _startTestCoroutine = null;
            }
        }
        else
        {
            _rotateFeedback.LogWarning("Test is already running");
        }
    }

    void StopTest()
    {
        if (_testRotation != null)
        {
            EditorCoroutineUtility.StopCoroutine(_testRotation);
            _rotateFeedback.StopInstantly();
            _testRotation = null;
            
            if (_startTestCoroutine != null)
            {
                EditorCoroutineUtility.StopCoroutine(_startTestCoroutine);
                _startTestCoroutine = null;
            }
        }
        else
            _rotateFeedback.LogWarning($"Cannot stop test. Not running or already finished");
    }

    void OnDestroy()
    {
        if (IsTestRunning())
            StopTest();
    }
}
