﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

namespace ReplaceDungeonGenerator
{
    /// This class is mostly a wrapper for the ReplacementEngine, doing repeated replacements
    [RequireComponent(typeof(PatternView))]
    public class RandomReplacer : MonoBehaviour
    {
        public int maxGenerationSteps = 10;
		public PatternView patternView;

        /// Do repeated replacement steps until no replacements are possible 
        /// or the maximum step number is reached
		[Button]
        public void Generate()
        {
            ReplacementEngine re = GetComponent<ReplacementEngine>();
            InitializeGeneration();

            if (re != null)
            {
                for (int i = 0; i < maxGenerationSteps; i++)
                {
                    if (!re.GenerationStep())
                    {
                        break;
                    }
                }
            }
        }

		[Button]
        public void InitializeGeneration()
        {
			patternView.pattern = new Pattern(patternView.pattern.Size, Tile.Empty);
            ReplacementEngine re = GetComponent<ReplacementEngine>();
            re.SetStartSymbol();
            PatternView.UpdateView();
        }

		[Button]
        public void GenerateStep()
        {
            ReplacementEngine re = GetComponent<ReplacementEngine>();
            re.GenerationStep();
            PatternView.UpdateView();
        }

		private void OnValidate() {
            // assign pattern view automatically
			if(patternView == null) {
				patternView = GetComponent<PatternView>();
			}
		}
    }
}