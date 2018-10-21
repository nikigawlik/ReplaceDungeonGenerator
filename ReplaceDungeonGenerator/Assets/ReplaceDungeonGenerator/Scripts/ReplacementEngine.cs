using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReplaceDungeonGenerator
{
    [RequireComponent(typeof(LevelGrid))]
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
            LevelGrid lg = GetComponent<LevelGrid>();
            RuleSet lr = GetComponent<RuleSet>();
            Vector3Int pSize = lr.startPattern.Size;
            Vector3Int lgSize = lg.Size;
            Vector3Int position = new Vector3Int((lgSize.x - pSize.x) / 2, (lgSize.y - pSize.y) / 2, (lgSize.z - pSize.z) / 2);
            // Vector3Int position = Vector3Int.zero;
            SetPattern(lg, position, lr.startPattern);
        }

        public bool GenerationStep()
        {
            LevelGrid lg = GetComponent<LevelGrid>();

            Match match = FindRandomMatch(lg);
            if (match == null)
            {
                return false;
            }

            // replace
            Pattern replacement = match.rule.replacement;
            Vector3Int position = match.position;
            SetPattern(lg, position, replacement);

            return true;
        }

        private static void SetPattern(LevelGrid lg, Vector3Int position, Pattern pattern)
        {
            Vector3Int pSize = pattern.Size;

			foreach (Vector3Int p in Utils.IterateGrid3D(pSize))
			{
				lg.SetTile(position + p, pattern.tiles[p.x, p.y, p.z]);
			}
        }

        private Match FindRandomMatch(LevelGrid lg)
        {
            Vector3Int size = lg.Size;
            List<Match> matches = new List<Match>();
            List<Rule> rules = GetComponent<RuleSet>().rules;

			foreach (Vector3Int pos in Utils.IterateGrid3D(size))
			{
				foreach (Rule r in rules)
				{
					Pattern p = r.structure;
					Vector3Int pSize = p.Size;
					bool fail = false;

					foreach (Vector3Int posLocal in Utils.IterateGrid3D(pSize))
					{
						Tile levelTile = lg.TileAt(pos + posLocal);
						Tile patternTile = p.tiles[posLocal.x, posLocal.y, posLocal.z];
						// TODO make more elegant
						if (patternTile.Label != Tile.Wildcard.Label && patternTile.Label != levelTile.Label)
						{
							fail = true;
							break;
						}
					}

					if (!fail)
					{
						// just return first match for now
						// return new Match(new Vector3Int(x, y, z), r);
						matches.Add(new Match(pos, r));
					}
				}
			}

            if (matches.Count == 0)
            {
                return null;
            }

            return Utils.Choose<Match>(matches.ToArray(), WeightOfMatch);
        }

        private float WeightOfMatch(Match m)
        {
            return m.rule.weight;
        }
    }
}
