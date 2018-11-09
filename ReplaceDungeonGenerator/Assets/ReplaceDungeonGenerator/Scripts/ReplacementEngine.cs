using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReplaceDungeonGenerator
{
    [RequireComponent(typeof(PatternView))]
    [RequireComponent(typeof(RuleSet))]
    public class ReplacementEngine : MonoBehaviour
    {
        class Match
        {
            public Vector3Int position;
            public Rule rule;

            public Match(Vector3Int position, Rule rule)
            {
                this.position = position;
                this.rule = rule;
            }
        }

        public void SetStartSymbol()
        {
            RuleSet ruleSet = GetComponent<RuleSet>();
            Vector3Int startPatternSize = ruleSet.startPattern.Size;
            Pattern mainPattern = GetComponent<PatternView>().pattern;
            Vector3Int mainPatternSize = mainPattern.Size;
            Vector3Int position = new Vector3Int((mainPatternSize.x - startPatternSize.x) / 2, (mainPatternSize.y - startPatternSize.y) / 2, (mainPatternSize.z - startPatternSize.z) / 2);
            // Vector3Int position = Vector3Int.zero;
            SetPattern(position, ruleSet.startPattern);
        }

        public bool GenerationStep()
        {
            Match match = FindRandomMatch();
            if (match == null)
            {
                return false;
            }

            // replace
            Pattern replacement = match.rule.rightSide;
            Vector3Int position = match.position;
            SetPattern(position, replacement);

            return true;
        }

        private void SetPattern(Vector3Int position, Pattern pattern)
        {
            Vector3Int pSize = pattern.Size;
            Pattern mainPattern = GetComponent<PatternView>().pattern;

			foreach (Vector3Int p in Utils.IterateGrid3D(pSize))
			{
                Tile tile = pattern.tiles[p.x, p.y, p.z];
                if (tile.Label != Tile.Wildcard.Label) {
                    mainPattern.SetTile(position + p, tile);
                }
			}
        }

        private Match FindRandomMatch()
        {
            Pattern mainPattern = GetComponent<PatternView>().pattern;
            Vector3Int size = mainPattern.Size;
            List<Match> matches = new List<Match>();
            List<Rule> rules = GetComponent<RuleSet>().rules;

            // iterate over grid, rules, left side pattern of rule
			foreach (Vector3Int pos in Utils.IterateGrid3D(size))
			{
				foreach (Rule ruleGroup in rules)
				{
                    foreach(Rule r in ruleGroup.GetPermutations()) {
                        Pattern patternToMatch = r.leftSide;
                        Vector3Int pSize = patternToMatch.Size;
                        bool fail = false;

                        foreach (Vector3Int posLocal in Utils.IterateGrid3D(pSize))
                        {
                            Tile mainPatternTile = mainPattern.GetTile(pos + posLocal);
                            Tile patternToMatchTile = patternToMatch.tiles[posLocal.x, posLocal.y, posLocal.z];
                            
                            // TODO make more elegant
                            // compare tiles of potential match and the main pattern
                            // we ignore wildcards
                            if (patternToMatchTile.Label != Tile.Wildcard.Label && patternToMatchTile.Label != mainPatternTile.Label)
                            {
                                fail = true;
                                break;
                            }
                        }

                        // if we matched, we remember the match for later
                        if (!fail)
                        {
                            matches.Add(new Match(pos, r));
                        }
                    } 
				}
			}

            if (matches.Count == 0)
            {
                // no match found
                return null;
            }

            // pick a random match from results
            return Utils.Choose<Match>(matches, WeightOfMatch);
        }

        /// Wrapper for rule weight, used by the choose utility function
        private float WeightOfMatch(Match m)
        {
            return m.rule.weight;
        }
    }
}
