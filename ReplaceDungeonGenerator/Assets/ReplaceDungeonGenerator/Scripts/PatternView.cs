using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ReplaceDungeonGenerator
{
    public class PatternView : MonoBehaviour
    {
        public Pattern pattern;
		
        public void GenerateTestPattern()
        {
            foreach (Vector3Int pos in Utils.IterateGrid3D(pattern.Size))
            {
                pattern.tiles[pos.x, pos.y, pos.z] = new Tile(
                    Tile.TileType.NonterminalSymbol,
                    "(" + pos.x.ToString() + ", " +
                    pos.y.ToString() + ", " +
                    pos.z.ToString() + ")"
                );
            }

        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (pattern != null)
            {
                foreach (Vector3Int pos in Utils.IterateGrid3D(pattern.Size))
                {
                    Tile t = pattern.TileAt(pos);

					Gizmos.color = ReplaceDungeonGenerator.Preferences.roomBoxColor;
                    Gizmos.DrawWireCube(GetPositionInWorldSpace(pos), Vector3.one * Preferences.roomBoxSize);

                    if (t.Label != "")
                    {
						GUI.color = ReplaceDungeonGenerator.Preferences.roomLabelColor;
                        DrawLabel(t.Label, pos);
                    }
                }
            }
        }

        private void DrawLabel(string str, Vector3Int position)
        {
            UnityEditor.Handles.BeginGUI();
            Vector3 worldPosition = GetPositionInWorldSpace(position) + Vector3.up * 0.1f;

            // cpu behind camera check
            Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(SceneView.currentDrawingSceneView.camera);
            if (!GeometryUtility.TestPlanesAABB(frustumPlanes, new Bounds(worldPosition, Vector3.one)))
            {
                return;
            }

            Vector3 screenPosition = HandleUtility.WorldToGUIPoint(worldPosition);
            Vector2 size = GUI.skin.label.CalcSize(new GUIContent(str));

            GUI.Label(new Rect(screenPosition.x - (size.x / 2f), screenPosition.y - (size.y / 2f), size.x, size.y), str);

            UnityEditor.Handles.EndGUI();
        }

        private static void DrawCross(Vector3 screenPosition)
        {
            Material mat = AssetDatabase.LoadAssetAtPath<Material>("Assets/ReplaceDungeonGenerator/Materials/LineMaterial.mat");
            mat.SetPass(0);
            float d = 10f;

            GL.Begin(GL.LINES);
            GL.Vertex3(screenPosition.x - d, screenPosition.y - d, 0);
            GL.Vertex3(screenPosition.x + d, screenPosition.y + d, 0);
            GL.Vertex3(screenPosition.x - d, screenPosition.y + d, 0);
            GL.Vertex3(screenPosition.x + d, screenPosition.y - d, 0);
            GL.End();
        }
#endif

        private Vector3 GetPositionInWorldSpace(Vector3Int position)
        {
            return transform.TransformPoint(position - (pattern.Size - Vector3.one) * 0.5f);
        }
    }
}