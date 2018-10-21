using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReplaceDungeonGenerator
{
	[System.Serializable]
	public struct Tile {
		// if the numbers change here, serialisation will break!
        public static Tile Empty       {get {return new Tile(".");}}
        public static Tile OutOfBounds {get {return new Tile("#");}}
        public static Tile Wildcard    {get {return new Tile("*");}}

        [Tooltip("Unique label identifying the Tile. Usually left empty for non-symbols.")]
        [SerializeField] private string label;

        public string Label
        {
            get
            {
                return label;
            }
        }

        public Tile(string label) {
			this.label = label;
		}
	}
}