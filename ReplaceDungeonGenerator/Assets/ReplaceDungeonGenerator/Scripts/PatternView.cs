using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ReplaceDungeonGenerator
{
    public class PatternView : MonoBehaviour
    {
        public interface IReactToPatternChange {
            void OnPatternChange();
        }

        private Pattern _pattern;
        public Vector3 displayDelta = Vector3.one;
        public bool alwaysShowLabel = false;
        public bool showCubes = false;

        public Pattern pattern
        {
            get
            {
                if(_pattern == null) {
                    _pattern = new Pattern(Vector3Int.one);
                }
                return _pattern;
            }

            set
            {
                _pattern = value;
                _pattern.OnChange += OnPatternChanged;
                OnPatternChanged();
            }
        }

        private void OnPatternChanged() {
            foreach(IReactToPatternChange component in GetComponents<IReactToPatternChange>()) {
                component.OnPatternChange();
            }
        }

        public void GenerateTestPattern()
        {
            foreach (Vector3Int pos in Utils.IterateGrid3D(pattern.Size))
            {
                pattern.tiles[pos.x, pos.y, pos.z] = new Tile(
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
                    Tile t = pattern.GetTile(pos);

                    if(showCubes) {
                        Gizmos.color = ReplaceDungeonGenerator.Preferences.RoomBoxColor;
                        Gizmos.DrawWireCube(GetPositionInWorldSpace(pos, displayDelta), Vector3.one * Preferences.RoomBoxSize);
                    }

                    if (t.Label != "")
                    {
						GUI.color = ReplaceDungeonGenerator.Preferences.RoomLabelColor;
                        Gizmos.color = ReplaceDungeonGenerator.Preferences.RoomLabelColor;
                        DrawTile(t, pos);
                    }
                }
            }
        }

        private void DrawTile(Tile t, Vector3Int pos) {
            Vector3 worldPosition = GetPositionInWorldSpace(pos, displayDelta);
            Vector3 dir;
            switch(t.Label) {
                case ".": 
                break;
                case ">":
                case "<":
                    dir = Vector3.Scale(CalculateDirection(pattern, pos), displayDelta);
                    if(t.Label == ">") 
                        DrawArrow(worldPosition - dir, worldPosition + dir);
                    else
                        DrawArrow(worldPosition + dir, worldPosition - dir);
                break;
                case "-":
                    dir = Vector3.Scale(CalculateDirection(pattern, pos), displayDelta);
                    DrawLine(worldPosition - dir, worldPosition + dir);
                break;
                case ":":
                    dir = Vector3.Scale(CalculateDirection(pattern, pos), displayDelta);
                    DrawDotted(worldPosition - dir, worldPosition + dir);
                break;
                default:
                    if(!alwaysShowLabel) DrawLabel(t.Label, worldPosition);
                break;
            }
            if(alwaysShowLabel) DrawLabel(t.Label, worldPosition);
        }

        private Vector3Int CalculateDirection(Pattern p, Vector3Int pos) {
            if(IsRoom(p.GetTile(pos - Vector3Int.right))) {
                return Vector3Int.right;
            } 
            else if(IsRoom(p.GetTile(pos - Vector3Int.up))) {
                return Vector3Int.up;
            }
            else if(IsRoom(p.GetTile(pos - new Vector3Int(0, 0, 1)))) {
                return new Vector3Int(0, 0, 1);
            }
            else {
                return Vector3Int.zero;
            }
        }

        private bool IsRoom(Tile tile) {
            // simple shortcut
            return tile.IsStructure();
        }

        private void DrawLine(Vector3 from, Vector3 to) {
            Handles.color = Gizmos.color;
            Handles.DrawLine(from, to);
        }

        private void DrawArrow(Vector3 from, Vector3 to) {
            Handles.color = Gizmos.color;
            Handles.DrawLine(from, to);
            float s = .05f;
            Handles.ConeHandleCap(0, from * s + to * (1f - s), Quaternion.LookRotation(to - from), (to - from).magnitude * s, EventType.Repaint);
        }

        private void DrawDotted(Vector3 from, Vector3 to) {
            Handles.color = Gizmos.color;
            Handles.DrawDottedLine(from, to, 8);
        }

        private void DrawLabel(string str, Vector3 position)
        {
            position = position + Vector3.up * 0.1f;
            UnityEditor.Handles.BeginGUI();
            // cpu behind camera check
            Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(SceneView.currentDrawingSceneView.camera);
            if (!GeometryUtility.TestPlanesAABB(frustumPlanes, new Bounds(position, Vector3.one)))
            {
                return;
            }

            Vector3 screenPosition = HandleUtility.WorldToGUIPoint(position);
            Vector2 size = GUI.skin.label.CalcSize(new GUIContent(str));

            GUI.Label(new Rect(screenPosition.x - (size.x / 2f), screenPosition.y - (size.y / 2f), size.x, size.y), str);
            UnityEditor.Handles.EndGUI();
        }

        // private void DrawCross(Vector3 screenPosition)
        // {
        //     mat.SetPass(0);
        //     float d = 10f;

        //     GL.Begin(GL.LINES);
        //     GL.Vertex3(screenPosition.x - d, screenPosition.y - d, 0);
        //     GL.Vertex3(screenPosition.x + d, screenPosition.y + d, 0);
        //     GL.Vertex3(screenPosition.x - d, screenPosition.y + d, 0);
        //     GL.Vertex3(screenPosition.x + d, screenPosition.y - d, 0);
        //     GL.End();
        // }
#endif

        public Vector3 GetPositionInWorldSpace(Vector3Int position, Vector3 delta)
        {
            return transform.TransformPoint(Vector3.Scale(position - (pattern.Size - Vector3.one) * 0.5f, delta));
        }

        public static void UpdateView() {
#if UNITY_EDITOR
            UnityEditor.SceneView.RepaintAll(); 
#endif
        }
    }
}