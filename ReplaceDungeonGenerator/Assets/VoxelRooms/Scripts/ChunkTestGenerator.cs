using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

namespace VoxelRooms
{
    [RequireComponent(typeof(BlockChunk))]
    public class ChunkTestGenerator : MonoBehaviour
    {
        public Block block1;
        public Block block2;
        public Vector3Int size = new Vector3Int(5, 5, 5);

        private void Start()
        {
            GenRoom();
        }

        [Button]
        private void GenRoom()
        {
            BlockChunk chunk = GetComponent<BlockChunk>();

            chunk.Size = size;

            for (int x = 0; x < chunk.Size.x; x++)
                for (int y = 0; y < chunk.Size.y; y++)
                    for (int z = 0; z < chunk.Size.z; z++)
                    {
                        Vector3Int pos = new Vector3Int(x, y, z);
                        if (
                            (
                                pos.x == 0 || pos.y == 0 || pos.z == 0
                                || pos.x == size.x - 1 || pos.y == size.y - 1 || pos.z == size.z - 1
                            // || Random.Range(0f, 1f) < 0.6f
                            )
                        // && pos.y < size.y - 2
                        )
                        {
                            chunk.SetBlock(pos, block1, false);
                        }
                        else
                        {
                            chunk.SetBlock(pos, block2, false);
                        }
                    }

            chunk.RecalculateMesh();
        }
    }
}
