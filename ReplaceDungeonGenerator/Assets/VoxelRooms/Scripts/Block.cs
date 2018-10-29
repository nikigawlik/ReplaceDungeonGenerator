using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelRooms
{
    [CreateAssetMenu(fileName = "Block", menuName = "VoxelRooms/Block", order = 0)]
    public class Block : ScriptableObject
    {
        public GameObject side = null;
        public GameObject top = null;
        public GameObject bottom = null;
        public GameObject always = null;
        public GameObject spawnPrefab = null;
        public bool solid = true;
    }
}
