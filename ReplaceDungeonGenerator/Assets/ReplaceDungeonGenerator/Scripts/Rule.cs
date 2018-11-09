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
        public bool strictRotation = false;

        public Rule(Pattern leftSide, Pattern rightSide, float weight, string shortName, bool strictRotation)
        {
            this.leftSide = leftSide;
            this.rightSide = rightSide;
            this.weight = weight;
            this.shortName = shortName;
            this.strictRotation = strictRotation;
        }

        // TODO cache this for better performance
        public Rule[] GetPermutations() {
            if(strictRotation) {
                return new Rule[1] {this};
            }
            else {
                Rule[] permutiations = new Rule[4];
                permutiations[0] = this;
                Rule r = this;
                for(int i = 1; i < 4; i++) {
                    r = new Rule(Pattern.Rotate90Y(r.leftSide), Pattern.Rotate90Y(r.rightSide), r.weight, r.shortName, true);
                    permutiations[i] = r;
                }

                return permutiations;
            }
        }
    }
}
