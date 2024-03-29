﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace ReplaceDungeonGenerator
{
    [CustomEditor(typeof(PatternView))]
    public class PatternViewEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            PatternView patternView = (PatternView)target;

            DrawDefaultInspector();

			// draw a custom int vector UI
            EditorGUILayout.BeginHorizontal();
            float previousLabelWidth = EditorGUIUtility.labelWidth;
            float previousFieldWidth = EditorGUIUtility.fieldWidth;

            EditorGUIUtility.labelWidth = 1f;
            EditorGUIUtility.fieldWidth = 1f;
            EditorGUILayout.LabelField("Size:");
            EditorGUIUtility.labelWidth = 12f;

            EditorGUI.BeginChangeCheck();

            int x = EditorGUILayout.IntField("X", patternView.pattern.Size.x);
            int y = EditorGUILayout.IntField("Y", patternView.pattern.Size.y);
            int z = EditorGUILayout.IntField("Z", patternView.pattern.Size.z);


            if(EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(target, "Changed Pattern");
                PrefabUtility.RecordPrefabInstancePropertyModifications(target);
                patternView.pattern.Size = new Vector3Int(x, y, z);
            }

            EditorGUIUtility.labelWidth = previousLabelWidth;
            EditorGUIUtility.fieldWidth = previousFieldWidth;
            EditorGUILayout.EndHorizontal();

			// Button for test pattern generation
            if (GUI.Button(EditorGUILayout.GetControlRect(), "Set test pattern"))
            {
                patternView.GenerateTestPattern();
            }
        }
    }
}