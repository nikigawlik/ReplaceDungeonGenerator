using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

namespace ReplaceDungeonGenerator
{
	[RequireComponent(typeof(PatternView))]
    public class DungeonBuilder : MonoBehaviour
    {
		[Tooltip("Object to use for a room. Has to have a IRoomGenerator component. ")]
        public GameObject roomPrefab;
		[Tooltip("Distance between rooms in x,y,z direction. ")]
        public Vector3 gridDelta = Vector3.one;

		// build the dungeon from the provided pattern
        [Button]
        public void Build()
        {
            PatternView pv = GetComponent<PatternView>();
            Pattern pattern = pv.pattern;

			// get container object
			GameObject container = Utils.CreateChildWithName(transform, "[Dungeon]");

            foreach (Vector3Int pos in Utils.IterateGrid3D(pattern.Size))
            {
                Tile t = pattern.GetTile(pos);

                if (t.IsRoom())
                {
					// detect doors for all of the connection flags
                    bool posXOpen = pattern.GetTile(pos + new Vector3Int(1, 0, 0)).IsDoor();
                    bool posYOpen = pattern.GetTile(pos + new Vector3Int(0, 1, 0)).IsDoor();
                    bool posZOpen = pattern.GetTile(pos + new Vector3Int(0, 0, 1)).IsDoor();
                    bool negXOpen = pattern.GetTile(pos + new Vector3Int(-1, 0, 0)).IsDoor();
                    bool negYOpen = pattern.GetTile(pos + new Vector3Int(0, -1, 0)).IsDoor();
                    bool negZOpen = pattern.GetTile(pos + new Vector3Int(0, 0, -1)).IsDoor();

					// create room object and run room generation
                    GameObject obj = Instantiate(roomPrefab, pv.GetPositionInWorldSpace(pos, gridDelta), Quaternion.identity);
                    obj.transform.SetParent(container.transform, true);
                    obj.GetComponent<IRoomGenerator>().Generate(posXOpen, posYOpen, posZOpen, negXOpen, negYOpen, negZOpen, t.Label);
                }
            }
        }
    }
}