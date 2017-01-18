﻿using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/*
[CustomEditor(typeof(NewtonWorld))]
public class NewtonWorldEditor: Editor
{
    public override void OnInspectorGUI()
    {
        NewtonWorld world = (NewtonWorld)target;
        world.m_asyncUpdate = EditorGUILayout.Toggle("async update", world.m_asyncUpdate);

        world.m_numberOfThreads = EditorGUILayout.IntPopup("worked threads", world.m_numberOfThreads, m_numberOfThreads, m_numberOfThreadsValues);

        world.m_solverIterationsCount = EditorGUILayout.IntField("solver iterations", world.m_solverIterationsCount);
        if (world.m_solverIterationsCount < 1)
        {
            world.m_solverIterationsCount = 1;
        }
        world.m_updateRate = EditorGUILayout.IntPopup("frame rate", world.m_updateRate, m_displayedOptions, m_displayedOptionsValues);

        world.m_broadPhaseType = EditorGUILayout.IntPopup("broad phase type", world.m_broadPhaseType, m_broaphase, m_broaphaseValues);

        world.m_gravity = EditorGUILayout.Vector3Field("gravity", world.m_gravity);
    }

    static private int[] m_displayedOptionsValues = {60, 90, 120, 150, 180, 240};
    static private string[] m_displayedOptions = { "60.0f fps", "90.0f fps", "120.0f fps", "150.0f fps", "180.0f fps", "240.0f fps" };

    static private int[] m_numberOfThreadsValues = { 0, 2, 3, 4, 8};
    static private string[] m_numberOfThreads = { "single threaded", "2 worked threads", "3 worked threads", "4 worked threads", "8 worked threads"};

    static private int[] m_broaphaseValues = { 0, 1};
    static private string[] m_broaphase = { "optimized for dynamic scenes", "optimize for static scenes"};
}
*/

[CustomEditor(typeof(NewtonWorld))]
public class NewtonWorldEditor : Editor
{
    void OnEnable()
    {
        // Setup the SerializedProperties
        m_numThreadsProp = serializedObject.FindProperty("m_numberOfThreads");
        m_solverIterationsCountProp = serializedObject.FindProperty("m_solverIterationsCount");
    }

    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();

        // Show the custom GUI controls
        int oldNumThreadsValue = m_numThreadsProp.intValue;
        EditorGUILayout.IntPopup(m_numThreadsProp, m_numberOfThreadsOptions, m_numberOfThreadsValues, new GUIContent("Worker threads"));

        int oldIterationsCountValue = m_solverIterationsCountProp.intValue;
        EditorGUILayout.IntSlider(m_solverIterationsCountProp, 1, 10, new GUIContent("Solver iteration count"));

        // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties();

        if (oldNumThreadsValue != m_numThreadsProp.intValue)
        {
            Debug.Log("Threads property changed!");
        }

        if (oldIterationsCountValue != m_solverIterationsCountProp.intValue)
        {
            Debug.Log("Solver iterations property changed!");
        }
    }

    SerializedProperty m_numThreadsProp;
    SerializedProperty m_solverIterationsCountProp;

    static private GUIContent[] m_numberOfThreadsOptions = { new GUIContent("Single threaded"), new GUIContent("2 worker threads"), new GUIContent("3 worker threads"), new GUIContent("4 worker threads"), new GUIContent("8 worker threads") };
    static private int[] m_numberOfThreadsValues = { 0, 2, 3, 4, 8 };
}



