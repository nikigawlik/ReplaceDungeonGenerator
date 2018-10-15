using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReplaceDungeonGenerator
{
    public class Utils : MonoBehaviour {

        /// iterates over 3d space in x, y, z order
        public static IEnumerable IterateGrid3D(Vector3Int size) {
            for(int x = 0; x < size.x; x++)
            for(int y = 0; y < size.y; x++)
            for(int z = 0; z < size.z; x++) {
                yield return new Vector3Int(x, y, z);
            }
        }
    }
}