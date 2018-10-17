using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReplaceDungeonGenerator
{
	[System.Serializable]
	public class Pattern {
        public Tile[,,] tiles = new Tile[1, 1, 1];

		public Pattern(Tile[,,] tiles) {
			this.tiles = tiles;
		}

		public Pattern(Vector3Int size) {
            tiles = new Tile[size.x, size.y, size.z];
		}

		public Pattern(Vector3Int size, Tile initialTile)
        {
            InitializeTiles(size, initialTile);
        }

        private void InitializeTiles(Vector3Int size, Tile initialTile)
        {
            tiles = new Tile[size.x, size.y, size.z];

            foreach (Vector3Int pos in Utils.IterateGrid3D(size))
            {
                tiles[pos.x, pos.y, pos.z] = initialTile;
            }
        }

        public Vector3Int Size {
            get {
				if(tiles == null) {
					tiles = new Tile[1, 1, 1];
				}
            	return new Vector3Int(tiles.GetLength(0), tiles.GetLength(1), tiles.GetLength(2)); 
            }
			set {
				// TODO preserve stuff
				if(value.x != tiles.GetLength(0) || value.y != tiles.GetLength(1) || value.z != tiles.GetLength(2)) {
					tiles = new Tile[value.x, value.y, value.z];
				}
			}
        }

		public Tile TileAt(Vector3Int position) {
			if(!Utils.InBounds(position, Size)) {
				return new Tile(Tile.Type.OutOfBounds);
			}

			return tiles[position.x, position.y, position.z];
		}
    }
}