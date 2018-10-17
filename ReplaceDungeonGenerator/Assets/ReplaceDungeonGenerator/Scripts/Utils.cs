using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReplaceDungeonGenerator
{
    public class Utils {

        /// iterates over 3d space in x, y, z order
        public static IEnumerable<Vector3Int> IterateGrid3D(Vector3Int size) {
            for(int x = 0; x < size.x; x++)
            for(int y = 0; y < size.y; y++)
            for(int z = 0; z < size.z; z++) {
                yield return new Vector3Int(x, y, z);
            }
        }

        /// checks for bounds in 3d space
        public static bool InBounds(Vector3Int position, Vector3Int size) {
            return position.x >= 0 && position.x < size.x 
                && position.y >= 0 && position.y < size.y
                && position.z >= 0 && position.z < size.z;
        }
    }
}