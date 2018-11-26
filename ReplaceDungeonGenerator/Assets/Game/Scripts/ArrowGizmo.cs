using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowGizmo : MonoBehaviour {
	public Mesh arrowShaftMesh;
	public Mesh arrowHeadMesh;
	public Color color = Color.white;

	public int shaftRot = 0;
	public int headRot = 0;

	private void OnDrawGizmos() {
		Gizmos.color = color;
		if(arrowShaftMesh) Gizmos.DrawMesh(
			arrowShaftMesh, 
			-1, 
			transform.position, 
			transform.rotation * Quaternion.Euler(0, shaftRot * 90, 0), 
			transform.localScale
		);
		
		if(arrowHeadMesh) Gizmos.DrawMesh(arrowHeadMesh, 
			-1, 
			transform.position, 
			transform.rotation * Quaternion.Euler(0, headRot * 90, 0), 
			transform.localScale
		);
	}
}
