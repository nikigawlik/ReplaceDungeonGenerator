using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FPSController : MonoBehaviour {
	public float lookSpeed = 1f;
	public float hLimit = 90f;
	public float acceleration = 1f;
	public float maxSpeed = 1f;
	
	private Vector3 previousMousePos;
	private Vector3 eulerLookRotation;
	private Rigidbody rb;

	private void Start() {
		previousMousePos = Input.mousePosition;
		rb = GetComponent<Rigidbody>();
	}

	private void OnMouseDown() {
	}

	private void Update() {
		// Handle rotation 

		eulerLookRotation = eulerLookRotation + new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * lookSpeed;
		eulerLookRotation = new Vector3(Mathf.Clamp(eulerLookRotation.x, -hLimit, hLimit), eulerLookRotation.y, eulerLookRotation.z);

		transform.rotation = Quaternion.Euler(eulerLookRotation);

		previousMousePos = Input.mousePosition;

		if(Input.GetMouseButtonDown(0)) {
			
			Cursor.lockState = CursorLockMode.Locked;
		}
	}

	private void FixedUpdate() {
		// Handle walking
		Vector3 force = transform.right * Input.GetAxis("Horizontal")
			+ transform.forward * Input.GetAxis("Vertical");
		rb.AddForce(force * acceleration, ForceMode.Acceleration);
		rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
	}
}
