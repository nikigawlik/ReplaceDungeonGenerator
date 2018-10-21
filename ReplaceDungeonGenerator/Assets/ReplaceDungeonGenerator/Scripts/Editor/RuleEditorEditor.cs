using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ReplaceDungeonGenerator
{
	[CustomEditor(typeof(RuleEditor))]
	public class RuleEditorEditor : Editor {
		public override void OnInspectorGUI() {
			RuleEditor patternView = (RuleEditor) target;

			List<Rule> rules = patternView.GetComponent<RuleSet>().rules;
			string[] options = new string[rules.Count];
			for(int i = 0; i < rules.Count; i++) {
				options[i] = rules[i].shortDescription;
			}
			patternView.selectedRule = EditorGUILayout.Popup(patternView.selectedRule, options);
			patternView.Refresh();

			DrawDefaultInspector();
		}
	}
}