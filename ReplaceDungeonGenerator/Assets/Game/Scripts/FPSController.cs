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
	public GameObject bulletPrefabOrPool;
	public float bulletSpeed = 5f;
	
	private Vector3 eulerLookRotation;
	private Rigidbody rb;
	private bool jumpTrigger = false;

	private void Start() {
		rb = GetComponent<Rigidbody>();
	}

	private void Update() {
		// Handle rotation 

		eulerLookRotation = eulerLookRotation + new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * lookSpeed;
		eulerLookRotation = new Vector3(Mathf.Clamp(eulerLookRotation.x, -hLimit, hLimit), eulerLookRotation.y, eulerLookRotation.z);

		head.rotation = Quaternion.Euler(eulerLookRotation);

		if(Input.GetMouseButtonDown(0)) {
			
			Cursor.lockState = CursorLockMode.Locked;
		}
	
		// jumping
		if(Input.GetButtonDown("Jump")) {
			jumpTrigger = true;
		}

		// shooty
		if(Input.GetButtonDown("Fire1")) {
			ObjectPool pool = bulletPrefabOrPool.GetComponent<ObjectPool>();
			GameObject obj;
			if(pool == null) {
				obj = Instantiate(bulletPrefabOrPool);
			} else {
				obj = pool.GetObject();
			}
			obj.transform.position = head.position;
			obj.transform.rotation = head.rotation;
			obj.GetComponent<Rigidbody>().AddForce(head.forward * bulletSpeed, ForceMode.VelocityChange);
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
		// TODO Use look rotation instead of transform
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

		if(jumpTrigger) {
			jumpTrigger = false;
			rb.AddForce(Vector3.up * jumpSpeed, ForceMode.VelocityChange);
		}
	}
}
