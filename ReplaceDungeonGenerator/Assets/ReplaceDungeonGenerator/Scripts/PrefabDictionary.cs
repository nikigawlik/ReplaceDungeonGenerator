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
		
        public GameObject FindPrefab(string label)
        {
            foreach (RoomPrefab rp in prefabs)
            {
                if (rp.label == label)
                {
                    return rp.prefab;
                }
            }
            return null;
        }
	}
}