using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReplaceDungeonGenerator
{
	[System.Serializable]
	public class Tile {
		// if the numbers change here, serialisation will break!
		public enum Type {
			Empty = 0,
			OutOfBounds = 1,
			Wildcard = 2,
			NonterminalSymbol = 3,
			TerminalSymbol = 4,
			DirectedEdge = 5,
			UndirectedEdge = 6,
		}

		public Type type = Type.Empty;
		[Tooltip("Unique label identifying the Tile. Usually left empty for non-symbols.")]
		public string label = "";

		public Tile(Type type) {
			this.type = type;
		}
	}
}