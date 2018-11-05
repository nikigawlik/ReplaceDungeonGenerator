using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;
using VoxelRooms;

namespace ReplaceDungeonGenerator
{
    /// class that generates a room using the VoxelRooms package 
    [RequireComponent(typeof(BlockChunk))]
    public class VoxelRoomGenerator : MonoBehaviour, IRoomGenerator
    {
        public Block airBlock;
        public Block wallBlock;
        public Block decoBlock;
        public Vector3Int size = new Vector3Int(11, 6, 11);
        
        public int baseLayerHeight = 2;
        public int upperLayerHeight = 2;
        public int minRoomWidth = 4;
        [Range(0f, 1f)] public float chanceOfUpperLevel = 0.5f;

        public bool posXOpen = false;
        public bool posYOpen = false;
        public bool posZOpen = false;
        public bool negXOpen = false;
        public bool negYOpen = false;
        public bool negZOpen = false;

        private void Start()
        {
            GenRoom();
        }

        [Button]
        private void GenRoom()
        {
            BlockChunk chunk = GetComponent<BlockChunk>();
            chunk.Size = size;

            // get pattern
            Tile wall = new Tile("w");
            Tile path = new Tile("p");
            Tile ladder = new Tile("l");
            Tile empty = new Tile("e");

            Pattern room = new Pattern(size, wall);

            // get random rectangle for base layer
            float f1 = Random.Range(0f, 0.99f) * Random.Range(0f, 0.99f);
            float f2 = Random.Range(0f, 0.99f) * Random.Range(0f, 0.99f);
            Vector2Int dim = new Vector2Int(
                minRoomWidth + Mathf.FloorToInt(f1 * (size.x - minRoomWidth - 1)),
                minRoomWidth + Mathf.FloorToInt(f2 * (size.z - minRoomWidth - 1))
            );
            Vector2Int pos = new Vector2Int(
                Random.Range(1, size.x - dim.x),
                Random.Range(1, size.z - dim.y)
            );
            RectInt baseRect = new RectInt(pos, dim);

            // fill bottom layer
            Vector3Int offset = new Vector3Int(baseRect.x, 1, baseRect.y);
            foreach (Vector3Int p in Utils.IterateGrid3D(new Vector3Int(baseRect.width, baseLayerHeight, baseRect.height))) {
                room.SetTile(offset + p, empty);
            }

            // clear paths
            if(posXOpen) BuildRay(room, path, new Vector3Int(size.x / 2, 1, size.z / 2), new Vector3Int(1, 0, 0));
            // if(posYOpen) BuildRay(room, ladder, new Vector3Int(size.x / 2, 1, size.z / 2), new Vector3Int(0, 1, 0));
            if(posZOpen) BuildRay(room, path, new Vector3Int(size.x / 2, 1, size.z / 2), new Vector3Int(0, 0, 1));
            if(negXOpen) BuildRay(room, path, new Vector3Int(size.x / 2, 1, size.z / 2), new Vector3Int(-1, 0, 0));
            // if(negYOpen) BuildRay(room, ladder, new Vector3Int(size.x / 2, 1, size.z / 2), new Vector3Int(0, -1, 0));
            if(negZOpen) BuildRay(room, path, new Vector3Int(size.x / 2, 1, size.z / 2), new Vector3Int(0, 0, -1));

            // if upper floor
            bool hasUpperLevel = Random.Range(0f, 1f) <= chanceOfUpperLevel;
            if(hasUpperLevel) {
                // get upper rectangle for top layer, that is never smaller than bottom layer
                Vector2Int p1 = new Vector2Int(
                    Random.Range(1, baseRect.x + 1),
                    Random.Range(1, baseRect.y + 1)
                );
                Vector2Int p2 = new Vector2Int(
                    Random.Range(baseRect.x + baseRect.width, size.x - 1),
                    Random.Range(baseRect.y + baseRect.height, size.z - 1)
                );
                RectInt upperRect = new RectInt(p1, p2 - p1);

                
                // fill upper layer
                Vector3Int offset2 = new Vector3Int(upperRect.x, 1 + baseLayerHeight, upperRect.y);
                foreach (Vector3Int p in Utils.IterateGrid3D(new Vector3Int(upperRect.width, upperLayerHeight, upperRect.height))) {
                    room.SetTile(offset2 + p, empty);
                }
            }

            // up down overrides, generate big openings for going up and down
            if(negYOpen) {
                Vector3Int offset3 = new Vector3Int(1, 1, 1);
                foreach (Vector3Int p in Utils.IterateGrid3D(new Vector3Int(size.x - 2, baseLayerHeight, size.z - 2))) {
                    room.SetTile(offset3 + p, empty);
                }
                offset3 = new Vector3Int(2, 0, 2);
                foreach (Vector3Int p in Utils.IterateGrid3D(new Vector3Int(size.x - 4, 1, size.z - 4))) {
                    room.SetTile(offset3 + p, empty);
                }
            }
            if(posYOpen) {
                Vector3Int offset3 = new Vector3Int(1, 1, 1);
                foreach (Vector3Int p in Utils.IterateGrid3D(new Vector3Int(size.x - 2, size.y - 2, size.z - 2))) {
                    room.SetTile(offset3 + p, empty);
                }
                offset3 = new Vector3Int(2, size.y - 1, 2);
                foreach (Vector3Int p in Utils.IterateGrid3D(new Vector3Int(size.x - 4, 1, size.z - 4))) {
                    room.SetTile(offset3 + p, empty);
                }
            }

            // choose three random deco positions
            Vector2Int[] decoPositions = new Vector2Int[3];
            for(int i = 0; i < decoPositions.Length; i++) {
                decoPositions[i] = new Vector2Int(Random.Range(1, size.x-1), Random.Range(1, size.z-1));
            }

            // set the blocks according to out room pattern
            foreach(Vector3Int chunkPos in Utils.IterateGrid3D(chunk.Size)) {
                Block block = airBlock;

                switch(room.GetTile(chunkPos).Label) {
                    case "w":
                        block = wallBlock;
                    break; 
                    case "e": 
                        block = airBlock;
                        // if there is floor below us we can spawn deco
                        if(room.GetTile(chunkPos + Vector3Int.down).Label == "w") {
                            foreach(Vector2Int p in decoPositions) {
                                if(p.x == chunkPos.x && p.y == chunkPos.z) {
                                    block = decoBlock;
                                }
                            }
                        }
                    break;
                    case "p": 
                    case "l": 
                    default: 
                        block = airBlock;
                    break;
                }
                chunk.SetBlock(chunkPos, block, false);
            }

            chunk.RecalculateMesh();
        }

        /// fills a ray with a certain tile, starting at position going in direction
        private Vector3Int BuildRay(Pattern room, Tile tile, Vector3Int position, Vector3Int direction)
        {
            while (Utils.InBounds(position, size))
            {
                room.SetTile(position, tile);

                position = position + direction;
            }

            return position;
        }

        /// wrapper on the GenRoom function for the interface
        public void Generate(bool posXOpen, bool posYOpen, bool posZOpen, bool negXOpen, bool negYOpen, bool negZOpen, string label)
        {
            this.posXOpen = posXOpen;
            this.posYOpen = posYOpen;
            this.posZOpen = posZOpen;
            this.negXOpen = negXOpen;
            this.negYOpen = negYOpen;
            this.negZOpen = negZOpen;
            GenRoom();
        }
    }
}
