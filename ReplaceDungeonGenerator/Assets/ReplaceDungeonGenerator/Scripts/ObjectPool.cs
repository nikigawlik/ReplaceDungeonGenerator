using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {
	public int numberOfObjects = 100;
	public GameObject prefab;

	private GameObject[] objects;
	private int pointer = 0;

	private void Start() {
		objects = new GameObject[numberOfObjects];
	}

	public GameObject GetObject() {
		if(objects[pointer] == null) {
			objects[pointer] = Instantiate(prefab, transform);
		}
		GameObject o = objects[pointer];
		o.SetActive(true);
		pointer = (pointer + 1) % objects.Length;
		return o;
	}

}
