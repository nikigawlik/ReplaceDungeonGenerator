using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelRooms
{
    public class BlockChunk : MonoBehaviour
    {
        public Block[,,] blocks = null;
        private Vector3Int size = new Vector3Int(0, 0, 0);

        public Vector3Int Size
        {
            get
            {
                return size;
            }

            set
            {
                size = value;
                blocks = new Block[size.x, size.y, size.z];
            }
        }

        public void SetBlock(Vector3Int position, Block block, bool updateMesh = true)
        {
            position.Clamp(Vector3Int.zero, Size - Vector3Int.one);

            blocks[position.x, position.y, position.z] = block;
            if (updateMesh)
            {
                RecalculateMesh();
            }
        }

        public void RecalculateMesh()
        {
            //TODO safety checks


            Vector3 offset = new Vector3(-Size.x / 2f + .5f, .5f, -Size.z / 2f + .5f);

            // prefabs
            Transform existingContainer = transform.Find("[GeneratedObjects]");
            if(existingContainer != null) {
                GameObject.DestroyImmediate(existingContainer.gameObject);
            }

            GameObject container = new GameObject("[GeneratedObjects]");
            container.transform.SetParent(transform);
            existingContainer = container.transform;
            
            for (int x = 0; x < Size.x; x++)
                for (int y = 0; y < Size.y; y++)
                    for (int z = 0; z < Size.z; z++) {
                        Block block = blocks[x, y, z];
                        Vector3Int position = new Vector3Int(x, y, z);

                        if(block.spawnPrefab != null) {
                            GameObject obj = Instantiate(block.spawnPrefab, container.transform);
                            obj.transform.position = transform.position + Vector3.Scale(position + offset, transform.localScale);
                        }
                    }


            // mesh
            Mesh mesh = GetComponent<MeshFilter>().mesh;
            if(mesh == null) {
                GetComponent<MeshFilter>().sharedMesh = new Mesh();
            }

            List<CombineInstance> meshes = new List<CombineInstance>();

            for (int x = 0; x < Size.x; x++)
                for (int y = 0; y < Size.y; y++)
                    for (int z = 0; z < Size.z; z++)
                    {
                        Block block = blocks[x, y, z];
                        Vector3Int position = new Vector3Int(x, y, z);

                        if (block.always != null)
                        {
                            CombineInstance comb = new CombineInstance();
                            comb.mesh = block.always.GetComponent<MeshFilter>().sharedMesh;
                            comb.transform = Matrix4x4.Translate(position + offset) * block.always.transform.localToWorldMatrix;
                            meshes.Add(comb);
                        }
                        if (block.top != null && !GetBlock(position + Vector3Int.up).solid)
                        {
                            CombineInstance comb = new CombineInstance();
                            comb.mesh = block.top.GetComponent<MeshFilter>().sharedMesh;
                            comb.transform = Matrix4x4.Translate(position + offset) * block.top.transform.localToWorldMatrix;
                            meshes.Add(comb);
                        }
                        if (block.bottom != null && !GetBlock(position + Vector3Int.down).solid)
                        {
                            CombineInstance comb = new CombineInstance();
                            comb.mesh = block.bottom.GetComponent<MeshFilter>().sharedMesh;
                            comb.transform = Matrix4x4.Translate(position + offset) * block.bottom.transform.localToWorldMatrix;
                            meshes.Add(comb);
                        }
                        if (block.side != null)
                        {
                            for (int d = 0; d < 4; d++)
                            {
                                Vector3Int deltaPos = new Vector3Int(
                                    d == 0 ? 1 : d == 2 ? -1 : 0,
                                    0,
                                    d == 1 ? 1 : d == 3 ? -1 : 0
                                );

                                if (!GetBlock(position + deltaPos).solid)
                                {
                                    CombineInstance comb = new CombineInstance();
                                    comb.mesh = block.side.GetComponent<MeshFilter>().sharedMesh;
                                    comb.transform = Matrix4x4.Translate(position + offset)
                                        * Matrix4x4.Rotate(Quaternion.Euler(0, -90 * d, 0))
                                        * block.side.transform.localToWorldMatrix;
                                    meshes.Add(comb);
                                }
                            }
                        }
                    }

            mesh.CombineMeshes(meshes.ToArray(), true, true, false);

            MeshCollider col = GetComponent<MeshCollider>();
            if (col)
            {
                col.sharedMesh = mesh;
            }
        }

        public Block GetBlock(Vector3Int position)
        {
            return blocks[
                Mathf.Clamp(position.x, 0, Size.x - 1),
                Mathf.Clamp(position.y, 0, Size.y - 1),
                Mathf.Clamp(position.z, 0, Size.z - 1)
            ];
        }
    }
}
