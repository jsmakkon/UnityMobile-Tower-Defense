using UnityEngine;
using System.Collections;

public class DragCamera : MonoBehaviour {
	//public GameObject cameraObject;
	public float dragSpeed = 2;
	private Vector3 dragOrigin;

	public bool cameraDragging = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (cameraDragging == true) {
			//slideCamera ();
			//for (Touch touch
			//Debug.Log("CameraDragging = " + cameraDragging);
			//slideCamera();
		}
	}

	void slideCamera() {
		//Debug.Log ("Slide cameran sisällä");
		Vector3 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
		//Vector3 newPosition = -touchDeltaPosition - dragOrigin;
		//Debug.Log ("touchDeltaposition = " + touchDeltaPosition);
		//Debug.Log ("dragorigin = " + dragOrigin);
		//Debug.Log ("newposition = " + newPosition);
		gameObject.transform.Translate (-touchDeltaPosition * dragSpeed);

	}
	
	public void setCameraDragTrue() {
		cameraDragging = true;
	}

	public void setDragOrigin(Vector3 startDragPos) {
		dragOrigin = startDragPos;
		//Debug.Log ("dragOrigin = " + dragOrigin);
	}
}