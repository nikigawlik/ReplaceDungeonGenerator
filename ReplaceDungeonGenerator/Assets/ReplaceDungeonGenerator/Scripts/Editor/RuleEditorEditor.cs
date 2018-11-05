using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ReplaceDungeonGenerator
{
    [CustomEditor(typeof(RuleEditor))]
    public class RuleEditorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            RuleEditor ruleEditor = (RuleEditor)target;

			// show a dropdown menu to select one of the rules
            List<Rule> rules = ruleEditor.GetComponent<RuleSet>().rules;
            string[] options = new string[rules.Count];
            for (int i = 0; i < rules.Count; i++)
            {
                options[i] = rules[i].shortDescription;
            }
            ruleEditor.selectedRule = EditorGUILayout.Popup(ruleEditor.selectedRule, options);
            ruleEditor.Refresh();

            DrawDefaultInspector();
        }
    }
}