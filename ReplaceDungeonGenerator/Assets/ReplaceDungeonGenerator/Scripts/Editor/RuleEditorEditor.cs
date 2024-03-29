﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ReplaceDungeonGenerator
{
    [CustomEditor(typeof(RuleEditor))]
    public class RuleEditorEditor : Editor
    {
        private SerializedProperty matchPatternView;
        private SerializedProperty replacementPatternView;

        private int previousRuleIndex = -1;

        bool internalOptionsToggleState = false;

        private void OnEnable() {
            matchPatternView = serializedObject.FindProperty("matchPatternView");
            replacementPatternView = serializedObject.FindProperty("replacementPatternView");
        }

        public override void OnInspectorGUI()
        {
            
            // Rule editor GUI
            internalOptionsToggleState = EditorGUILayout.Foldout(internalOptionsToggleState, "Rule editor settings");

            if(internalOptionsToggleState) {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(matchPatternView);
                EditorGUILayout.PropertyField(replacementPatternView);
                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.LabelField("Current Rule: ", EditorStyles.boldLabel);

            // Rule GUI
            RuleEditor ruleEditor = (RuleEditor)target;
            RuleSet ruleSet = ruleEditor.GetComponent<RuleSet>();
            List<Rule> rules = ruleSet.rules;

            bool ruleIndexChanged = previousRuleIndex != ruleSet.selectedRuleIndex;
            previousRuleIndex = ruleSet.selectedRuleIndex;

            if(ruleSet.selectedRuleIndex >= 0 && ruleSet.selectedRuleIndex < ruleSet.rules.Count) {
                Rule currentRule = rules[ruleSet.selectedRuleIndex];

                Undo.RecordObject(ruleSet, "Edit Rule");
                EditorGUI.BeginChangeCheck();

                // some extra fields
                currentRule.name = EditorGUILayout.TextField("Rule name", currentRule.name);
                currentRule.weight = EditorGUILayout.FloatField("Rule weight", currentRule.weight);
                currentRule.maximumApplications = EditorGUILayout.IntField("Maximum applications", currentRule.maximumApplications);
                currentRule.waitSteps = EditorGUILayout.IntField("Wait steps", currentRule.waitSteps);
                
                // rotation
                currentRule.strictRotation = EditorGUILayout.BeginToggleGroup("Obey rule orientation", currentRule.strictRotation);
                bool doRotation = GUILayout.Button("Rotate Rule");
                EditorGUILayout.EndToggleGroup();

                // reverse
                currentRule.reverseApplication = EditorGUILayout.Toggle("Reversible", currentRule.reverseApplication);

                if(EditorGUI.EndChangeCheck()) {
                    PrefabUtility.RecordPrefabInstancePropertyModifications(ruleSet);
                }

                PatternView leftPatternView = ((PatternView)matchPatternView.objectReferenceValue);
                Undo.RecordObject(leftPatternView, "Edit Rule");
                // text areas for rule editing
                try{
                    string leftSideText = SerializedRule.PatternToString(currentRule.leftSide);
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PrefixLabel(new GUIContent("Match: ", "String representation of left side (match). \";\", \",\", \" \" are separators for x, y, z dimension"));
                    leftSideText = EditorGUILayout.TextField(leftSideText, GUILayout.ExpandHeight(true));
                    if(EditorGUI.EndChangeCheck() || ruleIndexChanged) {
                        currentRule.leftSide = SerializedRule.StringToPattern(leftSideText);
                        if(doRotation) currentRule.leftSide = Pattern.Rotate90Y(currentRule.leftSide);
                        leftPatternView.pattern = currentRule.leftSide;
                        PrefabUtility.RecordPrefabInstancePropertyModifications(leftPatternView);
                    }
                }
                catch(System.ArgumentException) {
                    // parsing failed, do nothing
                }

                PatternView rightPatternView = ((PatternView)replacementPatternView.objectReferenceValue);
                Undo.RecordObject(rightPatternView, "Edit Rule");
                try{
                    string rightSideText = SerializedRule.PatternToString(currentRule.rightSide);
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PrefixLabel(new GUIContent("Replacement: ", "String representation of right side (replacement). \";\", \",\", \" \" are separators for x, y, z dimension"));
                    rightSideText = EditorGUILayout.TextField(rightSideText, GUILayout.ExpandHeight(true));
                    if(EditorGUI.EndChangeCheck() || ruleIndexChanged) {
                        currentRule.rightSide = SerializedRule.StringToPattern(rightSideText);
                        if(doRotation) currentRule.rightSide = Pattern.Rotate90Y(currentRule.rightSide);
                        rightPatternView.pattern = currentRule.rightSide;
                        PrefabUtility.RecordPrefabInstancePropertyModifications(rightPatternView);
                    }
                }
                catch(System.ArgumentException) {
                    // parsing failed, do nothing
                }

            } else {
                EditorGUILayout.HelpBox("Please select a rule in the RuleSet component.", MessageType.Info);
            }
        }
    }
}