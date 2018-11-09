using System.Collections;
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

            if(ruleSet.selectedRuleIndex >= 0 && ruleSet.selectedRuleIndex < ruleSet.rules.Count) {
                Rule currentRule = rules[ruleSet.selectedRuleIndex];

                // some extra fields
                currentRule.shortName = EditorGUILayout.TextField("Rule name", currentRule.shortName);
                currentRule.weight = EditorGUILayout.FloatField("Rule weight", currentRule.weight);
                
                // rotation
                currentRule.strictRotation = EditorGUILayout.BeginToggleGroup("Obey rule orientation", currentRule.strictRotation);
                bool doRotation = GUILayout.Button("Rotate Rule");
                EditorGUILayout.EndToggleGroup();

                // text areas for rule editing
                string leftSideText = SerializedRule.PatternToString(currentRule.leftSide);
                EditorGUILayout.PrefixLabel(new GUIContent("Match: ", "String representation of left side (match). \";\", \",\", \" \" are separators for x, y, z dimension"));
                leftSideText = EditorGUILayout.TextArea(leftSideText, GUILayout.ExpandHeight(true));
                currentRule.leftSide = SerializedRule.StringToPattern(leftSideText);
                if(doRotation) currentRule.leftSide = Pattern.Rotate90Y(currentRule.leftSide);
                ((PatternView) matchPatternView.objectReferenceValue).pattern = currentRule.leftSide;

                string rightSideText = SerializedRule.PatternToString(currentRule.rightSide);
                EditorGUILayout.PrefixLabel(new GUIContent("Replacement: ", "String representation of right side (replacement). \";\", \",\", \" \" are separators for x, y, z dimension"));
                rightSideText = EditorGUILayout.TextArea(rightSideText, GUILayout.ExpandHeight(true));
                currentRule.rightSide = SerializedRule.StringToPattern(rightSideText);
                if(doRotation) currentRule.rightSide = Pattern.Rotate90Y(currentRule.rightSide);
                ((PatternView) replacementPatternView.objectReferenceValue).pattern = currentRule.rightSide;
            } else {
                EditorGUILayout.HelpBox("Please select a rule in the RuleSet component.", MessageType.Info);
            }
        }
    }
}