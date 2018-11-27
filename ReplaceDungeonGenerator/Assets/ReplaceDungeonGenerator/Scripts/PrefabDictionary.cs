using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReplaceDungeonGenerator
{
    [CreateAssetMenu(fileName = "PrefabDictionary", menuName = "ReplaceDungeonGenerator/PrefabDictionary", order = 0)]
	public class PrefabDictionary : ScriptableObject {
        [System.Serializable]
        public class RoomPrefab
        {
            public string label;
            public GameObject prefab;
        }

        public RoomPrefab[] prefabs;
		
        public GameObject[] ToPrefabs(string[] labels)
        {
            GameObject[] objects = new GameObject[labels.Length];
            for(int i = 0; i < labels.Length; i++){
                string label = labels[i];
                objects[i] = null;

                foreach(RoomPrefab rp in prefabs)
                {
                    if (rp.label == label)
                    {
                        objects[i] = rp.prefab;
                        break;
                    }
                }
            }

            return objects;
        }
	}
}