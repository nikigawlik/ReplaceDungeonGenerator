using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactible))]
public class InteractDestroy : MonoBehaviour {
	void Start () {
		Interactible i = GetComponent<Interactible>();
		i.onInteract.AddListener(Destruct);
	}
	
	public void Destruct() {
		GameObject.Destroy(gameObject);
	}
}
