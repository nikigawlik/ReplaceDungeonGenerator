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
        public int maximumApplications = -1;

        public Rule(Rule rule) {
            this.leftSide = rule.leftSide;
            this.rightSide = rule.rightSide;
            this.weight = rule.weight;
            this.shortName = rule.shortName;
            this.strictRotation = rule.strictRotation;
            this.maximumApplications = rule.maximumApplications;
        }

        public Rule(Pattern leftSide, Pattern rightSide, float weight, string shortName, bool strictRotation, int maximumApplications)
        {
            this.leftSide = leftSide;
            this.rightSide = rightSide;
            this.weight = weight;
            this.shortName = shortName;
            this.strictRotation = strictRotation;
            this.maximumApplications = maximumApplications;
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
                    r = new Rule(Pattern.Rotate90Y(r.leftSide), Pattern.Rotate90Y(r.rightSide), r.weight, r.shortName, true, r.maximumApplications);
                    permutiations[i] = r;
                }

                return permutiations;
            }
        }
    }
}
