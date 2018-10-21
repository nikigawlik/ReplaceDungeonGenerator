using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using EasyButtons;
using System.Text.RegularExpressions;

namespace ReplaceDungeonGenerator
{
    public class RuleSet : MonoBehaviour
    {
        public string pathToRules;

        public List<Rule> rules;
        public Pattern startPattern;

        private void Awake()
        {

        }

        [EasyButtons.Button]
        public void LoadRules()
        {
            string[] guids = AssetDatabase.FindAssets("t:TextAsset", new string[1] { pathToRules });
            rules = new List<Rule>();
            foreach (string str in guids)
            {
                TextAsset ta = AssetDatabase.LoadAssetAtPath<TextAsset>(AssetDatabase.GUIDToAssetPath(str));
                // Debug.Log(Regex.Replace(ta.text, @"(\n+\s+)", ""));
                rules.Add(JsonUtility.FromJson<SerializedRule>(ta.text).Rule);
            }
        }

        [EasyButtons.Button]
        public void SaveRules()
        {
			if(pathToRules == "") {
				return;
			}
            int runningNumber = 0;

            foreach (Rule rule in rules)
            {
                string json = JsonUtility.ToJson(new SerializedRule(rule));
                string path;

                path = pathToRules + "/" + rule.shortDescription + ".json";

                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        writer.Write(json);
                    }
                }

                runningNumber++;
            }

            UnityEditor.AssetDatabase.Refresh();
        }
    }
}