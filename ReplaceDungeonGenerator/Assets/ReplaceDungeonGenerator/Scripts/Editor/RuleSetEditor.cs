using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace ReplaceDungeonGenerator
{
	[CustomEditor(typeof(RuleSet))]
	public class RuleSetEditor : Editor {
		private SerializedProperty rules;
		private SerializedProperty jsonFile;
		private ReorderableList rlRules;
	
		private void OnEnable() {
			rules = serializedObject.FindProperty("rules");
			jsonFile = serializedObject.FindProperty("jsonFile");

			rlRules = new ReorderableList(serializedObject, rules, true, false, true, true) {
				drawHeaderCallback = DrawListHeader,
				drawElementCallback = DrawListElement
			};
		}
	
		private void DrawListHeader(Rect rect) {
			GUI.Label(rect, "Rules");
		}
	
		private void DrawListElement(Rect rect, int index, bool isActive, bool isFocused) {
			SerializedProperty item = rules.GetArrayElementAtIndex(index);
			EditorGUI.LabelField(rect, item.FindPropertyRelative("shortName").stringValue);
		}
	
		public override void OnInspectorGUI() {
			DrawDefaultInspector();

			serializedObject.Update();
			RuleSet ruleSet = (RuleSet) target;

			EditorGUILayout.PropertyField(jsonFile);
			
			EditorGUILayout.BeginHorizontal();

			if(GUILayout.Button("Load rules")) {
				ruleSet.LoadRules();
			}
			if(GUILayout.Button("Save rules")) {
				ruleSet.SaveRules();
			}

			EditorGUILayout.EndHorizontal();
			serializedObject.ApplyModifiedProperties();

			// update serialized object because of potential rule change
			serializedObject.Update();
			
			EditorGUILayout.Space();
			rlRules.DoLayoutList();
			
			// set values
			ruleSet.selectedRuleIndex = rlRules.index;
			serializedObject.ApplyModifiedProperties();
		}
	}
}