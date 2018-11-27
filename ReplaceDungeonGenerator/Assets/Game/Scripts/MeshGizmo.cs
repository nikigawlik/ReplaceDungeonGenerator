using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGizmo : MonoBehaviour {
	public Mesh gizmoMesh;
	public Color color = Color.white;
	public Vector3 positionOffset;

	private void OnDrawGizmos() {
		Gizmos.color = color;
		Gizmos.DrawMesh(gizmoMesh, -1, transform.position+positionOffset, transform.rotation, transform.localScale);
	}
}
	
