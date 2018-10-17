using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReplaceDungeonGenerator
{
	[System.Serializable]
	public struct Tile {
		// if the numbers change here, serialisation will break!
		public enum TileType {
			Empty = 0,
			OutOfBounds = 1,
			Wildcard = 2,
			NonterminalSymbol = 3,
			TerminalSymbol = 4,
			DirectedEdge = 5,
			UndirectedEdge = 6,
		}

        [SerializeField] private TileType type;
        [Tooltip("Unique label identifying the Tile. Usually left empty for non-symbols.")]
        [SerializeField] private string label;

        public TileType Type
        {
            get
            {
                return type;
            }
        }

        public string Label
        {
            get
            {
                return label;
            }
        }

        public Tile(TileType type, string label) {
			this.type = type;
			this.label = label;
		}

		public Tile(TileType type) {
			this.type = type;
			this.label = "";
		}
	}
}