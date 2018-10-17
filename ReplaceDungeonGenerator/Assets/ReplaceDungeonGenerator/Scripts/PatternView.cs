using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ReplaceDungeonGenerator
{
	public class PatternView : MonoBehaviour {
		public Pattern pattern;

		private void Awake() {
			if(pattern == null) {
				Tile[,,] tiles = new Tile[3, 3, 3];

				foreach (Vector3Int pos in Utils.IterateGrid3D(new Vector3Int(3, 3, 3)))
				{
					tiles[pos.x, pos.y, pos.z] = new Tile(Tile.Type.NonterminalSymbol);
					tiles[pos.x, pos.y, pos.z].label = "(" +
						pos.x.ToString() + ", " + 
						pos.y.ToString() + ", " + 
						pos.z.ToString() + ")"
					;
				}

				pattern = new Pattern(tiles);
			}
		}

		private void OnDrawGizmos() {
			if(pattern != null) {
				foreach (Vector3Int pos in Utils.IterateGrid3D(pattern.Size))
				{
					Tile t = pattern.TileAt(pos);

					Gizmos.DrawCube(GetPositionInWorldSpace(pos), Vector3.one * 0.1f);

					if(t != null && t.label != "") {
						DrawLabel(t.label, pos);
					}
				}
			}
		}

		private void DrawLabel(string str, Vector3Int position)
        {
            UnityEditor.Handles.BeginGUI();
            Vector3 worldPosition = GetPositionInWorldSpace(position);

			// cpu behind camera check
			Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(SceneView.currentDrawingSceneView.camera);
			if(!GeometryUtility.TestPlanesAABB(frustumPlanes, new Bounds(worldPosition, Vector3.one))) {
				return;
			}
			
			
            Vector3 screenPosition = HandleUtility.WorldToGUIPoint(worldPosition); //view.camera.WorldToScreenPoint(worldPosition);
            Vector2 size = GUI.skin.label.CalcSize(new GUIContent(str));

            Material mat = AssetDatabase.LoadAssetAtPath<Material>("Assets/ReplaceDungeonGenerator/Materials/LineMaterial.mat");
            mat.SetPass(0);
			float d = 10f;

			GL.Begin(GL.LINES);
			GL.Vertex3(screenPosition.x - d, screenPosition.y - d, 0);
			GL.Vertex3(screenPosition.x + d, screenPosition.y + d, 0);
			GL.Vertex3(screenPosition.x - d, screenPosition.y + d, 0);
			GL.Vertex3(screenPosition.x + d, screenPosition.y - d, 0);
			GL.End();

            GUI.Label(new Rect(screenPosition.x - (size.x / 2f), screenPosition.y - (size.y / 2f), size.x, size.y), str);

            UnityEditor.Handles.EndGUI();
        }

        private Vector3 GetPositionInWorldSpace(Vector3Int position)
        {
            return transform.position + position - (pattern.Size - Vector3.one) * 0.5f;
        }
    }
}