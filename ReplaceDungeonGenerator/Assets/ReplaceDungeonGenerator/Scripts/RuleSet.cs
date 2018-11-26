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
        public string startLabel = "start";
        [HideInInspector] public string pathToRules; // hidden bec. of custom drawer
        [HideInInspector] public List<Rule> rules; // hidden bec. of custom drawer

        private int _selectedRuleIndex = -1;

        public int selectedRuleIndex
        {
            get
            {
                return _selectedRuleIndex;
            }

            set
            {
                _selectedRuleIndex = value;
            }
        }

        public Pattern startPattern
        {
            get
            {
                return new Pattern(Vector3Int.one, new Tile(startLabel));
            }
        }

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
            if (pathToRules == "")
            {
                return;
            }

            foreach (Rule rule in rules)
            {
                // serialize as JSON
                string json = JsonUtility.ToJson(new SerializedRule(rule));
                string path;

                path = pathToRules + "/" + rule.shortName + ".json";

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