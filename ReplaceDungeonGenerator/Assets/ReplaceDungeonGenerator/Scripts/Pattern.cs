using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReplaceDungeonGenerator
{
	public class Pattern {
        private Tile[,,] tiles;

		public Pattern(Tile[,,] tiles) {
			this.tiles = tiles;
		}

        public Vector3Int Size {
            get {
				if(tiles == null) {
					tiles = new Tile[1, 1, 1];
				}
            	return new Vector3Int(tiles.GetLength(0), tiles.GetLength(1), tiles.GetLength(2)); 
            }
        }

		public Tile TileAt(Vector3Int position) {
			if(!Utils.InBounds(position, Size)) {
				return new Tile(Tile.Type.OutOfBounds);
			}

			Tile t = tiles[position.x, position.y, position.z];
			return t != null? t : new Tile(Tile.Type.Empty);
		}
    }
}