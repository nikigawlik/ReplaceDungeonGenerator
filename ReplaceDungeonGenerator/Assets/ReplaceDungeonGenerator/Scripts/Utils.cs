using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;

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

        /// checks if two boxes at specific positions intersect in 3d
        public static bool BoundsIntersect(Vector3Int pos1, Vector3Int size1, Vector3Int pos2, Vector3Int size2) {
            return pos2.x < pos1.x + size1.x && pos2.x + size2.x > pos1.x
                && pos2.y < pos1.y + size1.y && pos2.y + size2.y > pos1.y
                && pos2.z < pos1.z + size1.z && pos2.z + size2.z > pos1.z;
        }

        /// clamp a Vector3Int to bounds
        public static Vector3Int ClampVector3Int(Vector3Int value, Vector3Int bounds) {
            return new Vector3Int(
                Mathf.Clamp(value.x, 0, bounds.x),
                Mathf.Clamp(value.y, 0, bounds.y),
                Mathf.Clamp(value.z, 0, bounds.z)
            );
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

        public static void DrawLabel(string str, Vector3 position)
        {
#if UNITY_EDITOR
            position = position + Vector3.up * 0.1f;
            UnityEditor.Handles.BeginGUI();
            // cpu behind camera check
            Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(SceneView.currentDrawingSceneView.camera);
            if (!GeometryUtility.TestPlanesAABB(frustumPlanes, new Bounds(position, Vector3.one)))
            {
                UnityEditor.Handles.EndGUI();
                return;
            }

            Vector3 screenPosition = HandleUtility.WorldToGUIPoint(position);
            Vector2 size = GUI.skin.label.CalcSize(new GUIContent(str));

            GUI.Label(new Rect(screenPosition.x - (size.x / 2f), screenPosition.y - (size.y / 2f), size.x, size.y), str);
            UnityEditor.Handles.EndGUI();
#endif
        }

        [MenuItem("GameObject/AssetForgeTools/Add Mesh Colliders by Name", false, 0)]
        public static void AddMeshCollidersByName() {
            Transform[] transforms = Selection.GetTransforms(SelectionMode.Deep);
            string matchPattern = @"struct_*";
            
            foreach(Transform t in transforms) {
                GameObject obj = t.gameObject;
                if(Regex.IsMatch(obj.name, matchPattern)) {
                    MeshFilter mf = obj.GetComponent<MeshFilter>();
                    if(mf != null) {
                        MeshCollider mc = obj.GetComponent<MeshCollider>();
                        if(mc != null) {
                            Undo.DestroyObjectImmediate(mc);
                        }
                        Undo.AddComponent<MeshCollider>(obj);
                    }
                }
            }
        }
    }
}