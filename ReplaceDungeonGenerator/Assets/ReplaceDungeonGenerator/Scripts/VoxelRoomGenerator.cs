using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;
using VoxelRooms;

namespace ReplaceDungeonGenerator
{
    [RequireComponent(typeof(BlockChunk))]
    public class VoxelRoomGenerator : MonoBehaviour, IRoomGenerator
    {
        public Block block1;
        public Block block2;
        public Vector3Int size = new Vector3Int(5, 5, 5);
        public int wallThicknessMin = 1;
        public int wallThicknessMax = 1;
        public int ceilingThickness = 1;
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
            int wallThickness = Random.Range(wallThicknessMin, wallThicknessMax+1);

            foreach(Vector3Int pos in Utils.IterateGrid3D(size))
            {
                bool free = !(pos.x < wallThickness || pos.y < 1 || pos.z < wallThickness
                    || pos.x >= size.x - wallThickness || pos.y >= size.y - ceilingThickness || pos.z >= size.z - wallThickness);

                float doorHalfHeight = 0.5f;
                float doorPos = 1f;

                

                free = free || (Mathf.Abs(pos.y - doorPos) <= doorHalfHeight && Mathf.Abs(pos.z - ((size.z - 1) / 2f)) <= 0.5f && posXOpen && pos.x >= size.x - wallThickness);
                free = free || Mathf.Abs(pos.y - doorPos) <= doorHalfHeight && Mathf.Abs(pos.z - ((size.z - 1) / 2f)) <= 0.5f && negXOpen && pos.x < wallThickness;
                free = free || Mathf.Abs(pos.x - ((size.x - 1) / 2f)) <= 0.5f && Mathf.Abs(pos.z - ((size.z - 1) / 2f)) <= 0.5f && posYOpen && pos.y >= size.y - ceilingThickness;
                free = free || Mathf.Abs(pos.x - ((size.x - 1) / 2f)) <= 0.5f && Mathf.Abs(pos.z - ((size.z - 1) / 2f)) <= 0.5f && negYOpen && pos.y < 1;
                free = free || Mathf.Abs(pos.x - ((size.x - 1) / 2f)) <= 0.5f && Mathf.Abs(pos.y - doorPos) <= doorHalfHeight && posZOpen && pos.z >= size.z - wallThickness;
                free = free || Mathf.Abs(pos.x - ((size.x - 1) / 2f)) <= 0.5f && Mathf.Abs(pos.y - doorPos) <= doorHalfHeight && negZOpen && pos.z < wallThickness;

                if(!free)
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

        public void Generate(bool posXOpen, bool posYOpen, bool posZOpen, bool negXOpen, bool negYOpen, bool negZOpen)
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
