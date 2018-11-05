using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReplaceDungeonGenerator
{
	/// Basic Editor for rules
	[RequireComponent(typeof(RuleSet))]
	public class RuleEditor : MonoBehaviour {
		[Tooltip("String representation of left side (match). \";\", \",\", \" \" are separators for x, y, z dimension")]
		[Multiline] [SerializeField] private string match = "";
		[Tooltip("String representation of right side (replacement). \";\", \",\", \" \" are separators for x, y, z dimension")]
		[Multiline] [SerializeField] private string replacement = "";

		// declared just so it shows up in inspector, to edit things like weight and description
		// TODO separate into custom inspector
		[SerializeField] private Rule currentRule;

		[SerializeField] private PatternView matchPatternView;
		[SerializeField] private PatternView replacementPatternView;
		[HideInInspector] public int selectedRule = 0;

		private string prevMatch = "";
		private string prevReplacement = "";
		private int prevSelectedRule = 0;

		/// called by the custom editor to update the state of the selected rule
		public void Refresh() {
			RuleSet ruleSet = GetComponent<RuleSet>();

			selectedRule = Mathf.Clamp(selectedRule, 0, ruleSet.rules.Count - 1);
			currentRule = ruleSet.rules[selectedRule];

			// detect if match pattern has changed
			if(prevMatch != match) {
				// update pattern view
				try {
					matchPatternView.pattern = SerializedRule.StringToPattern(match);
					// update left side of rule
					ruleSet.rules[selectedRule].leftSide = matchPatternView.pattern;
				}
				catch(System.ArgumentException) {
					// parsing failed, we keep old pattern for now
				}

				prevMatch = match;
			}

			// detect if replacement pattern has changed
			if(prevReplacement != replacement) {
				// update pattern view
				try {
					replacementPatternView.pattern = SerializedRule.StringToPattern(replacement);
					// update right side of rule
					ruleSet.rules[selectedRule].rightSide = replacementPatternView.pattern;
				} 
				catch(System.ArgumentException) {
					// parsing failed, we keep old pattern for now
				}

				prevReplacement = replacement;
			}

			// detect if rule has changed
			if(prevSelectedRule != selectedRule) {
				// get the current rule
				Rule rule = ruleSet.rules[selectedRule];
				// set the match pattern view's pattern
				matchPatternView.pattern = rule.leftSide;
				// set the replacement pattern view's pattern
				replacementPatternView.pattern = rule.rightSide;

				// set the match and replacement patterns from the strings
				prevMatch = match = SerializedRule.PatternToString(matchPatternView.pattern); 
				prevReplacement = replacement = SerializedRule.PatternToString(replacementPatternView.pattern);
				prevSelectedRule = selectedRule;
			}

			// update the view, because might have changed the patterns
			PatternView.UpdateView();
		}
	}
}