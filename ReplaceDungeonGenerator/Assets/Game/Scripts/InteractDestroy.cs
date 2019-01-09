using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactible))]
public class InteractDestroy : MonoBehaviour {
	public GameObject objectToDestroy;

	void Start () {
		Interactible i = GetComponent<Interactible>();
		i.onInteract.AddListener(Destruct);
	}
	
	public void Destruct() {
		objectToDestroy = objectToDestroy? objectToDestroy : gameObject;
		GameObject.Destroy(objectToDestroy);
	}
}
