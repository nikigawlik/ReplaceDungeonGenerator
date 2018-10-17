using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReplaceDungeonGenerator
{
	[System.Serializable]
	public class Pattern : ISerializationCallbackReceiver{
        public Tile[,,] tiles = new Tile[1, 1, 1];
		
		// For serialization
		[SerializeField] [HideInInspector] private Tile[] serializedTiles;
		[SerializeField] [HideInInspector] private Vector3Int serializedSize;

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
				if(tiles == null || value.x != tiles.GetLength(0) || value.y != tiles.GetLength(1) || value.z != tiles.GetLength(2)) {
					tiles = new Tile[value.x, value.y, value.z];
				}
			}
        }

		public Tile TileAt(Vector3Int position) {
			if(!Utils.InBounds(position, Size)) {
				return new Tile(Tile.TileType.OutOfBounds);
			}

			return tiles[position.x, position.y, position.z];
		}

		public void OnBeforeSerialize() {
			serializedSize = Size;
			serializedTiles = new Tile[serializedSize.x * serializedSize.y * serializedSize.z];

			foreach (Vector3Int p in Utils.IterateGrid3D(serializedSize))
			{
				serializedTiles[p.x * serializedSize.y * serializedSize.z + p.y * serializedSize.z + p.z] = tiles[p.x, p.y, p.z];
			}
		}

		public void OnAfterDeserialize() {
			Size = serializedSize;

			foreach (Vector3Int p in Utils.IterateGrid3D(serializedSize))
			{
				tiles[p.x, p.y, p.z] = serializedTiles[p.x * serializedSize.y * serializedSize.z + p.y * serializedSize.z + p.z];
			}
		}
    }
}