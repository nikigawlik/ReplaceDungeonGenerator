using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactible))]
public class Lock : MonoBehaviour {
	private void Start() {
		GetComponent<Interactible>().onInteract.AddListener(TryUnlock);
	}

	private void TryUnlock() {
		if(GameController.instance.numberOfKeys > 0) {
			GameController.instance.numberOfKeys--;
			GameObject.Destroy(gameObject);
		}
	}
}
