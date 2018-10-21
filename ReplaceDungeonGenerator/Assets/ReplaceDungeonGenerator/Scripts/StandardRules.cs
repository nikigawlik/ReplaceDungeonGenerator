#pragma warning disable 0219

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

namespace ReplaceDungeonGenerator
{
    [RequireComponent(typeof(RuleSet))]
    public class StandardRules : MonoBehaviour
    {

        private void Awake()
        {
        }

        [Button]
        private void AddRules()
        {
            Rule[] rules = null;
            Pattern startPattern;

            List<Rule> ruleList = new List<Rule>();

            Tile lA = new Tile("A"); // room, symbol
            Tile la = new Tile("a"); // room, terminal symbol
            Tile lb = new Tile("b"); // boss room
            Tile le = new Tile("e"); // entrance room
            Tile t = new Tile("-"); // two way connection
            Tile s = new Tile(":"); // sight connection
            // Tile wl = Tile.wildcard);
            Tile fr = Tile.Empty;

            Tile po = new Tile(">");
            Tile no = new Tile("<");

            // // horizontal sight connection
            // AddRule(
            // 	ruleList, 
            // 	new Pattern(new Tile[3, 1, 1]{
            // 		{{lA}},
            // 		{{fr}},
            // 		{{lA}}
            // 	}),
            // 	new Pattern(new Tile[3, 1, 1]{
            // 		{{lA}},
            // 		{{sX}},
            // 		{{lA}}
            // 	}),
            // 	1f
            // );

            // horizontal extension
            AddRule(
                ruleList,
                new Pattern(new Tile[3, 1, 3]{
                {{lA, fr, fr}},
                {{t, fr, fr}},
                {{lA, fr, fr}}
                }),
                new Pattern(new Tile[3, 1, 3]{
                {{lA, t, lA}},
                {{fr, fr, t}},
                {{lA, t, lA}}
                }),
                1f,
                true,
                "horizontal extension"
            );

            // horizontal flip
            AddRule(
                ruleList,
                new Pattern(new Tile[3, 1, 5]{
                {{t, lA, fr, fr, fr}},
                {{fr, t, fr, fr, fr}},
                {{fr, lA, t, lA, t}}
                }),
                new Pattern(new Tile[3, 1, 5]{
                {{t, lA, t, lA, fr}},
                {{fr, fr, fr, t, fr}},
                {{fr, fr, fr, lA, t}}
                }),
                1f,
                true,
                "horizontal flip"
            );

            // horizontal cycle
            AddRule(
                ruleList,
                new Pattern(new Tile[3, 1, 3]{
                {{lA, fr, fr}},
                {{t, fr, fr}},
                {{lA, fr, fr}}
                }),
                new Pattern(new Tile[3, 1, 3]{
                {{lA, t, lA}},
                {{t, fr, t}},
                {{lA, t, lA}}
                }),
                .2f,
                true,
                "horizontal cycle"
            );

            // vertical extension
            AddRule(
                ruleList,
                new Pattern(new Tile[1, 3, 3]{{
                {lA, t, lA},
                {fr, fr, fr},
                {fr, fr, fr}
            }}),
                new Pattern(new Tile[1, 3, 3]{{
                {lA, fr, lA},
                {t, fr, t},
                {lA, t, lA}
            }}),
                0.3f,
                true,
                "vertical extension"
            );

            // vertical extension y mirror
            AddRule(
                ruleList,
                new Pattern(new Tile[1, 3, 3]{{
                {fr, fr, fr},
                {fr, fr, fr},
                {lA, t, lA}
            }}),
                new Pattern(new Tile[1, 3, 3]{{
                {lA, t, lA},
                {t, fr, t},
                {lA, fr, lA}
            }}),
                0.3f,
                true,
                "vertical extension y mirror"
            );

            // termination
            // not active becasue it can end everything early
            // AddRule(
            // 	ruleList,
            // 	new Pattern(new Tile[1, 1, 1] {{{
            // 		lA
            // 	}}}),
            // 	new Pattern(new Tile[1, 1, 1] {{{
            // 		la
            // 	}}}),
            // 	0.01f,
            // 	false
            // );

            // move boss room
            // not active bec it can move the boss room to the beginning, and does never terminate
            // AddRule(
            // 	ruleList,
            // 	new Pattern(new Tile[3, 1, 1]{
            // 		{{lA}},
            // 		{{tX}},
            // 		{{lb}}
            // 	}),
            // 	new Pattern(new Tile[3, 1, 1]{
            // 		{{lb}},
            // 		{{tX}},
            // 		{{lA}}
            // 	}),
            // 	1f
            // );

            startPattern = new Pattern(new Tile[3, 1, 3]{
				{{le, t, lA}},
				{{no, fr, t}},
				{{lb, t, lA}}
			});

            RuleSet lr = GetComponent<RuleSet>();
            lr.rules = ruleList;
            lr.startPattern = startPattern;
        }

        private static void AddRule(List<Rule> ruleList, Pattern match, Pattern repl, float weight, bool rotateY, string shortDescription)
        {
            ruleList.Add(new Rule(match, repl, weight, shortDescription));
            if (rotateY)
            {
                for (int i = 0; i < 3; i++)
                {
                    match = Pattern.Rotate90Y(match);
                    repl = Pattern.Rotate90Y(repl);
                    ruleList.Add(new Rule(match, repl, weight, shortDescription + "_rotX_" + i.ToString()));
                }
            }
        }
    }
}
