using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReplaceDungeonGenerator
{
    public class Utils {
        /// a delegate to use for normal events
        public delegate void StandardEventHandler();

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
        
        public delegate float WeightFunction<T>(T obj); 

        /// Chooses a thing from an IEnumerable (Array, List) using weighted randomness
        public static T Choose<T>(IEnumerable<T> spawnOptions, WeightFunction<T> WeightOf) {
            float weightSum = 0;
            foreach (T t in spawnOptions)
            {
                weightSum += WeightOf(t);
            }

            if(weightSum == 0) {
                return default(T);
            }

            float choice = UnityEngine.Random.Range(0f, weightSum);
            foreach (T t in spawnOptions)
            {
                choice -= WeightOf(t);
                if (choice <= 0)
                {
                    return t;
                }
            }

            // Safety fallback, just return first element
            IEnumerator<T> enumerator = spawnOptions.GetEnumerator();
            enumerator.MoveNext();
            return enumerator.Current;
        }

        /// get a child object of specific name, destroys existing object with that name
        public static GameObject CreateChildWithName(Transform transform, string name) { 
            Transform existingTransform = transform.Find(name);
            if(existingTransform != null) {
                GameObject.DestroyImmediate(existingTransform.gameObject);
            }

            GameObject obj = new GameObject(name);
            obj.transform.SetParent(transform);

            return obj;
        }
    }
}