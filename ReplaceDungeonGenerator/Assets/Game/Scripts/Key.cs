using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactible))]
public class Key : MonoBehaviour {

    private void Start() {
        Interactible interactible = GetComponent<Interactible>();

        interactible.onInteract.AddListener(PickMeUp);
    }

	public void PickMeUp() {
        GameController.instance.numberOfKeys++;
        GameObject.Destroy(gameObject);
    }
}
