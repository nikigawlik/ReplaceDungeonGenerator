using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

namespace ReplaceDungeonGenerator
{
    public class LevelGrid : MonoBehaviour
    {
        public int maxGenerationSteps = 100;
		public PatternView patternView;
		public Vector3Int Size {get{return patternView.pattern.Size;}}

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
			patternView.pattern = new Pattern(Size, Tile.Empty);
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

		public Tile TileAt(Vector3Int position) {
			return patternView.pattern.GetTile(position);
		}

		public void SetTile(Vector3Int position, Tile tile) {
			patternView.pattern.SetTile(position, tile);
		}

		private void OnValidate() {
			if(patternView == null) {
				patternView = GetComponent<PatternView>();
			}

			if(patternView == null) {
				Debug.LogError("Pattern view needs to be assigned.", this.gameObject);
			}
		}
    }
}