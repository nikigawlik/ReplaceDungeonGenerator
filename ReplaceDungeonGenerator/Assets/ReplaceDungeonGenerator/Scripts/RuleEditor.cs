using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReplaceDungeonGenerator
{
	[RequireComponent(typeof(RuleSet))]
	public class RuleEditor : MonoBehaviour {
		[Multiline] [SerializeField] private string match = "";
		[Multiline] [SerializeField] private string replacement = "";
		[SerializeField] private Rule currentRule;

		[SerializeField] private PatternView matchPatternView;
		[SerializeField] private PatternView replacementPatternView;
		[HideInInspector] public int selectedRule = 0;

		private string prevMatch = "";
		private string prevReplacement = "";
		private int prevSelectedRule = 0;

		public void Refresh() {
			RuleSet ruleSet = GetComponent<RuleSet>();
			selectedRule = Mathf.Clamp(selectedRule, 0, ruleSet.rules.Count - 1);
			currentRule = ruleSet.rules[selectedRule];
			if(prevMatch != match) {
				matchPatternView.pattern = SerializedRule.StringToPattern(match);
				prevMatch = match;
				ruleSet.rules[selectedRule].structure = matchPatternView.pattern;
			}
			if(prevReplacement != replacement) {
				replacementPatternView.pattern = SerializedRule.StringToPattern(replacement);
				prevReplacement = replacement;
				ruleSet.rules[selectedRule].replacement = replacementPatternView.pattern;
			}
			if(prevSelectedRule != selectedRule) {
				Rule rule = ruleSet.rules[selectedRule];
				matchPatternView.pattern = rule.structure;
				prevMatch = match = SerializedRule.PatternToString(matchPatternView.pattern);
				replacementPatternView.pattern = rule.replacement;
				prevReplacement = replacement = SerializedRule.PatternToString(replacementPatternView.pattern);
				prevSelectedRule = selectedRule;
			}
			PatternView.UpdateView();
		}
	}
}