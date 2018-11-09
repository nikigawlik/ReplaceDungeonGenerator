using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReplaceDungeonGenerator
{
	/// Basic Editor for rules
	[RequireComponent(typeof(RuleSet))]
	public class RuleEditor : MonoBehaviour {
		[SerializeField] private PatternView matchPatternView;
		[SerializeField] private PatternView replacementPatternView;
	}
}