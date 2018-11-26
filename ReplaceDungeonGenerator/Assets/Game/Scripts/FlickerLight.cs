using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerLight : MonoBehaviour {
	public float strength = .1f;
	public float radiusMod = .1f;
	public float minDelay = 0f;
	public float maxDelay = .1f;
	public float smoothness = 1f;
	public float displacementStrength = 0.1f;

	private float defaultIntensity;
	private Vector3 defaultPosition;
	private float defaultRadius;
	private float currentIntesity;
	private Vector3 currentPosition;
	private float timer;

	// Use this for initialization
	void Start () {
		Light light = GetComponent<Light>();
		defaultIntensity = light.intensity;
		defaultPosition = transform.position;
		defaultRadius = light.range;
	}
	
	// Update is called once per frame
	void Update () {
		Light light = GetComponent<Light>();

		timer -= Time.deltaTime;
		if(timer <= 0) {
			timer = Random.Range(minDelay, maxDelay);
			currentIntesity = Random.Range( - 1,  + 1);
			currentPosition = defaultPosition + Random.rotation * Vector3.forward * Random.Range(0, displacementStrength);
		}
		light.intensity = Mathf.Lerp(light.intensity, defaultIntensity + currentIntesity * strength, smoothness * Time.deltaTime);
		light.range = Mathf.Lerp(light.range, defaultRadius + currentIntesity * radiusMod, smoothness * Time.deltaTime);
		transform.position = Vector3.Lerp(transform.position, currentPosition, smoothness * Time.deltaTime);
	}
}
