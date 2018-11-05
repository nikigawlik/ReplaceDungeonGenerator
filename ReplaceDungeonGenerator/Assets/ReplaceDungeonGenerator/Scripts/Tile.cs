using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReplaceDungeonGenerator
{
	[System.Serializable]
	public struct Tile {
		// Don't change these symbols.
        public static Tile Empty       {get {return new Tile(".");}}
        public static Tile OutOfBounds {get {return new Tile("#");}}
        public static Tile Wildcard    {get {return new Tile("*");}}

        [Tooltip("Unique label identifying the Tile.")]
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

        public bool IsRoom() {
            return IsStructure() && !IsDoor();
        }

        public bool IsStructure() {
            return Label != "." && Label != "" && Label != "#" && Label != "*";
        }

        public bool IsDoor() {
            return Label == "-" || Label == ">" || Label == "<" || Label == ":";
        }
	}
}