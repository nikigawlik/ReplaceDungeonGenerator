using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FPSController : MonoBehaviour {
	public float lookSpeed = 1f;
	public float hLimit = 90f;
	public float acceleration = 1f;
	public float maxSpeed = 1f;
	public float hDrag = 1f;
	public float jumpSpeed = 5f;

	public Transform head;
	
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

		head.rotation = Quaternion.Euler(eulerLookRotation);

		previousMousePos = Input.mousePosition;

		if(Input.GetMouseButtonDown(0)) {
			
			Cursor.lockState = CursorLockMode.Locked;
		}
	}

	private void FixedUpdate() {
        Vector3 removeY = new Vector3(1f, 0f, 1f);
		// do artificial drag
		Vector3 dragAcceleration = new Vector3(
			-hDrag * rb.velocity.x, 
			0f,
			-hDrag * rb.velocity.z
		);
		rb.velocity += dragAcceleration * Time.fixedDeltaTime;

		// get walking inputs
		Vector3 right = head.right;
        right.Scale(removeY);
		right.Normalize();
		Vector3 forward = head.forward;
		forward.Scale(removeY);
		forward.Normalize();

		Vector3 walkInput = right * Input.GetAxis("Horizontal")
			+ forward * Input.GetAxis("Vertical");

		// get acceleration
		Vector3 acc = acceleration * walkInput;
		
		// restrict acceleration
		Vector3 vel = rb.velocity;
		vel.Scale(removeY);
		float prevMag = vel.magnitude;
		vel += acc;
		vel = Vector3.ClampMagnitude(vel, Mathf.Max(maxSpeed, prevMag));

		vel.y = rb.velocity.y;
		rb.velocity = vel;

		// jumping
		if(Input.GetButtonDown("Jump")) {
			rb.AddForce(Vector3.up * jumpSpeed, ForceMode.VelocityChange);
		}

		// // Handle walking
		// Vector2 right = new Vector2(head.right.x, head.right.z);
		// right.Normalize();
		// Vector2 forward = new Vector2(head.forward.x, head.forward.z);
		// forward.Normalize();

		// Vector2 force = right * Input.GetAxis("Horizontal")
		// 	+ forward * Input.GetAxis("Vertical");

		// Vector2 v = new Vector2(rb.velocity.x, rb.velocity.z);
		// Vector2 dragAcceleration = new Vector2(
		// 	-hDrag * rb.velocity.x, 
		// 	-hDrag * rb.velocity.z
		// );
		// v += dragAcceleration * Time.fixedDeltaTime;
		// v += force * acceleration * Time.fixedDeltaTime;
        // float velMag = Mathf.Sqrt(rb.velocity.x * rb.velocity.x + rb.velocity.y * rb.velocity.y);
        // v = Vector2.ClampMagnitude(v, Mathf.Max(maxSpeed, velMag));
		// rb.velocity = new Vector3(v.x, rb.velocity.y, v.y);


		// // rb.AddForce(force * acceleration, ForceMode.Acceleration);
		// // rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
	}
}
