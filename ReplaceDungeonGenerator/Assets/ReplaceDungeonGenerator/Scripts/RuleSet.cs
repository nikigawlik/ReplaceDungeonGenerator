using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using EasyButtons;
using System.Text.RegularExpressions;

namespace ReplaceDungeonGenerator
{
    /// Wrapper for a list of rules, can load the rules from files
    public class RuleSet : MonoBehaviour
    {
        public string pathToRules;

        public List<Rule> rules;
        public Pattern startPattern;

        [EasyButtons.Button]
        public void LoadRules()
        {
            // Use the Unity search to find text assets at the path
            string[] guids = AssetDatabase.FindAssets("t:TextAsset", new string[1] { pathToRules });
            rules = new List<Rule>();
            // Load and deserialize the text assets
            foreach (string str in guids)
            {
                TextAsset ta = AssetDatabase.LoadAssetAtPath<TextAsset>(AssetDatabase.GUIDToAssetPath(str));
                rules.Add(JsonUtility.FromJson<SerializedRule>(ta.text).Rule);
            }
        }

        [EasyButtons.Button]
        public void SaveRules()
        {
			if(pathToRules == "") {
				return;
			}

            foreach (Rule rule in rules)
            {
                // serialize as JSON
                string json = JsonUtility.ToJson(new SerializedRule(rule));
                string path;

                path = pathToRules + "/" + rule.shortDescription + ".json";

                // write to file
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        writer.Write(json);
                    }
                }
            }

            UnityEditor.AssetDatabase.Refresh();
        }
    }
}