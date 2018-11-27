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
        public int waitSteps = 0;
        public bool reverseApplication = false;

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
        public Rule[] GetPermutations()
        {
            if(reverseApplication) {
                // add additions reverse permutations
                Rule[] rotationPermutations = GetRotationPermutations();
                Rule[] perms = new Rule[rotationPermutations.Length * 2];
                for(int i = 0; i < rotationPermutations.Length; i++) {
                    Rule r = rotationPermutations[i];
                    perms[i*2] = r;
                    perms[i*2+1] = new Rule(r.rightSide, r.leftSide,r.weight, r.shortName, true, r.maximumApplications);
                }
                return perms;
            }
            else{
                // just return rotations
                return GetRotationPermutations();
            }
        }

        private Rule[] GetRotationPermutations()
        {
            if (strictRotation)
            {
                return new Rule[1] { this };
            }
            else
            {
                Rule[] permutiations = new Rule[4];
                permutiations[0] = this;
                Rule r = this;
                for (int i = 1; i < 4; i++)
                {
                    r = new Rule(Pattern.Rotate90Y(r.leftSide), Pattern.Rotate90Y(r.rightSide), r.weight, r.shortName, true, r.maximumApplications);
                    permutiations[i] = r;
                }

                return permutiations;
            }
        }
    }
}
