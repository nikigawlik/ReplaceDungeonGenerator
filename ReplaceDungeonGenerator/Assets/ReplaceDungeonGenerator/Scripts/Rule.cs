using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReplaceDungeonGenerator
{
    [System.Serializable]
    public class Rule
    {
        // the pattern that has to match
        public Pattern leftSide;
        // the replacement
        public Pattern rightSide;
        public float weight;
        public string shortName = "";

        public Rule(Pattern structure, Pattern replacement, float weight, string shortDescription)
        {
            this.leftSide = structure;
            this.rightSide = replacement;
            this.weight = weight;
            this.shortName = shortDescription;
        }
    }
}
