using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReplaceDungeonGenerator
{
    [RequireComponent(typeof(PatternView))]
    public class PatternToPrefabs : MonoBehaviour, PatternView.IReactToPatternChange
    {
        public PrefabDictionary prefabDictionary;
        // method called from PatternView
        public void OnPatternChange()
        {
            PatternView patternView = GetComponent<PatternView>();
            Pattern pattern = patternView.pattern;
            GameObject container = Utils.CreateChildWithName(transform, "[GeneratedRooms]");
            foreach (Vector3Int position in Utils.IterateGrid3D(pattern.Size))
            {
                string[] tags = pattern.GetTile(position).Label.Split('_');
                if (tags.Length != 2)
                {
                    // Debug.LogWarning("Could not parse tile label: Label has to consist of two tags separated by an underscore.", gameObject);
                    continue;
                }
                int rotation;
                try
                {
                    rotation = int.Parse(tags[1]);
                }
                catch (System.FormatException)
                {
                    Debug.LogWarning("Could not parse tile label: Second tag in label is not an integer.", gameObject);
                    continue;
                }
                catch (System.OverflowException)
                {
                    Debug.LogWarning("Could not parse tile label: The rotation is fractional or out of the integer number range.", gameObject);
                    continue;
                }
                GameObject prefab = prefabDictionary.FindPrefab(tags[0]);
                if (prefab == null)
                {
                    Debug.LogWarning("Could not parse tile label: Specified tag \"" + tags[0] + "\"does not exist.");
                    continue;
                }

                GameObject instance = Instantiate(
                    prefab,
                    patternView.GetPositionInWorldSpace(position, patternView.displayDelta),
                    Quaternion.Euler(0, rotation * 90, 0)
                );
                instance.transform.SetParent(container.transform, true);
            }
        }
    }
}