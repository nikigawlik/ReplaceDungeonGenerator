using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactible : MonoBehaviour {
	public string text = "interact";
	public string animationTrigger = "interact";
	// public Camera cam;
	[System.NonSerialized] public bool showText = false;

    private GUIStyle textStyle;

    private UnityEvent _onInteract;
    public UnityEvent onInteract
    {
        get
        {
			if(_onInteract == null) {
				_onInteract = new UnityEvent();
			}
            return _onInteract;
        }

        set
        {
            _onInteract = value;
        }
    }

    private void Start() {
		textStyle = new GUIStyle();
		textStyle.alignment = TextAnchor.MiddleCenter;
		textStyle.normal.textColor = Color.white;
	}

	public void Interact() {
		Animator anim = GetComponent<Animator>();
		if(anim != null) {
			anim.SetTrigger(animationTrigger);
		}
		onInteract.Invoke();
	}

	private void OnGUI() {
		Camera cam = Camera.main;
		if(cam && showText && Event.current.type == EventType.Repaint) {
			showText = false;

			Vector3 position = transform.position;

			Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(cam);

			if (GeometryUtility.TestPlanesAABB(frustumPlanes, new Bounds(position, Vector3.one))) {
				Vector3 screenPosition = cam.WorldToScreenPoint(position);
				Vector2 size = new Vector2(100, 25);
				GUI.Label(new Rect(
					screenPosition.x - (size.x / 2f), 
					Screen.height - (screenPosition.y + (size.y / 2f)), 
					size.x, size.y), text,
					textStyle);
			}
		}
	}
}
