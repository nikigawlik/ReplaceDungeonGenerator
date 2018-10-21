using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReplaceDungeonGenerator
{
    [System.Serializable]
    public class Rule
    {
        public Pattern structure;
        public Pattern replacement;
        public float weight;
        public string shortDescription = "";

        public Rule(Pattern structure, Pattern replacement, float weight, string shortDescription)
        {
            this.structure = structure;
            this.replacement = replacement;
            this.weight = weight;
            this.shortDescription = shortDescription;
        }
    }
}
