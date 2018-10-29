using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

namespace ReplaceDungeonGenerator
{
	public class GeometryGenerator : MonoBehaviour {
		public GameObject roomPrefab;
		public Vector3 gridDelta = Vector3.one;

		[Button]
		public void Build() {
			PatternView pv = GetComponent<PatternView>();
			LevelGrid lg = GetComponent<LevelGrid>();
			Pattern pattern = lg.patternView.pattern;

			foreach (Vector3Int pos in Utils.IterateGrid3D(pattern.Size))
			{
				Tile t = pattern.GetTile(pos);
				
				if(t.IsRoom()) {
					bool posXOpen = pattern.GetTile(pos + new Vector3Int(1, 0, 0)).IsDoor();
					bool posYOpen = pattern.GetTile(pos + new Vector3Int(0, 1, 0)).IsDoor();
					bool posZOpen = pattern.GetTile(pos + new Vector3Int(0, 0, 1)).IsDoor();
					bool negXOpen = pattern.GetTile(pos + new Vector3Int(-1, 0, 0)).IsDoor();
					bool negYOpen = pattern.GetTile(pos + new Vector3Int(0, -1, 0)).IsDoor();
					bool negZOpen = pattern.GetTile(pos + new Vector3Int(0, 0, -1)).IsDoor();

					GameObject obj = Instantiate(roomPrefab, Vector3.Scale(pv.GetPositionInWorldSpace(pos), gridDelta), Quaternion.identity);
					obj.transform.SetParent(this.transform, true);
					obj.GetComponent<IRoomGenerator>().Generate(posXOpen, posYOpen, posZOpen, negXOpen, negYOpen, negZOpen, t.Label);
				}
			}
		}
	}
}