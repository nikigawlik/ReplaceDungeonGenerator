using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnCollisionEnter(Collision other) {
		Rigidbody rb = GetComponent<Rigidbody>();
		rb.constraints = RigidbodyConstraints.None;
	}
}
