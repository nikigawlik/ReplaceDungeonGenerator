using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using EasyButtons;
using System.Text.RegularExpressions;

namespace ReplaceDungeonGenerator
{
    /// Wrapper for a list of rules, can load the rules from files
    public class RuleSet : MonoBehaviour
    {
        [System.Serializable] private class RuleSetJsonObject {
            public SerializedRule[] serializedRules;
        }

        public string startLabel = "start";
        [HideInInspector] public TextAsset jsonFile; // hidden bec. of custom drawer
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

#if UNITY_EDITOR
        public void LoadRules()
        {
            RuleSetJsonObject rsjo = JsonUtility.FromJson<RuleSetJsonObject>(jsonFile.text);
            rules = new List<Rule>();
            foreach(SerializedRule sr in rsjo.serializedRules) {
                rules.Add(sr.Rule);
            }
        }

        public void SaveRules()
        {
            if (jsonFile == null)
            {
                return;
            }

            // create a serializable object with list of rules
            List<SerializedRule> serializedRules = new List<SerializedRule>();

            foreach (Rule rule in rules)
            {
                serializedRules.Add(new SerializedRule(rule));
            }

            RuleSetJsonObject rsjo = new RuleSetJsonObject();
            rsjo.serializedRules = serializedRules.ToArray();

            string json = JsonUtility.ToJson(rsjo, true);
            string path = UnityEditor.AssetDatabase.GetAssetPath(jsonFile);

            // write to file
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(json);
                }
            }

            UnityEditor.AssetDatabase.Refresh();
        }
#endif
    }
}