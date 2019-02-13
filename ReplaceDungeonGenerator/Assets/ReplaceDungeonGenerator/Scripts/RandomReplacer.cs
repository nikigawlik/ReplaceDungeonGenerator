using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;
using UnityEngine.Profiling;

namespace ReplaceDungeonGenerator
{
    /// This class is mostly a wrapper for the ReplacementEngine, doing repeated replacements
    [RequireComponent(typeof(PatternView))]
    [RequireComponent(typeof(ReplacementEngine))]
    public class RandomReplacer : MonoBehaviour
    {
        public int maxGenerationSteps = 10;
		public PatternView patternView;
        public bool increaseSeed = true;
        public int seed = 0;

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
                    if (!re.ReplaceMatch())
                    {
                        break;
                    }
                }
            }
        }

		[Button]
        public void InitializeGeneration()
        {
#if UNITY_EDITOR
			UnityEditor.Undo.RecordObject(this.gameObject, "Intialize Generation");
			UnityEditor.Undo.RecordObject(this, "Intialize Generation");
			UnityEditor.Undo.RecordObject(patternView, "Intialize Generation");
			UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(this.gameObject);
			UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(this);
			UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(patternView);
#endif
            if(increaseSeed) seed++;

			patternView.pattern = new Pattern(patternView.pattern.Size, Tile.Empty);
            ReplacementEngine re = GetComponent<ReplacementEngine>();
            re.ResetGeneration(seed);
            PatternView.UpdateView();
        }

		[Button]
        public void GenerateStep()
        {
#if UNITY_EDITOR
			UnityEditor.Undo.RecordObject(this.gameObject, "Generation Step");
			UnityEditor.Undo.RecordObject(this, "Generation Step");
			UnityEditor.Undo.RecordObject(patternView, "Generation Step");
			UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(this.gameObject);
			UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(this);
			UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(patternView);
#endif

            ReplacementEngine re = GetComponent<ReplacementEngine>();
            re.ReplaceMatch();
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