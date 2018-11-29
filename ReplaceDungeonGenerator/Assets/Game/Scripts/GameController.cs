using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	public static GameController instance;

	public Text keyCounterText;

	private int _numberOfKeys;
	public int numberOfKeys {
		get {
			return _numberOfKeys;
		}
		set {
			_numberOfKeys = value;
			keyCounterText.text = _numberOfKeys.ToString();
		}
	}

	// Use this for initialization
	void Start () {
		if(instance != null) {
			GameObject.Destroy(instance.gameObject);
		}

		instance = this;
	}
}
