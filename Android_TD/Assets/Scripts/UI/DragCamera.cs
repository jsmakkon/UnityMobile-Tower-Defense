using UnityEngine;
using System.Collections;

public class DragCamera : MonoBehaviour {
	//public GameObject cameraObject;
	public float dragSpeed = 2;
	//private Vector3 dragOrigin;
    Touch dragOriginTouch;
    Touch currentTouch;

    Vector3 position;

	public bool cameraDragging = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private Vector3 getWorldPoint (Vector3 screenPoint)
    {
        RaycastHit hit;
        Physics.Raycast(Camera.main.ScreenPointToRay(screenPoint), out hit, 200.0f);
        //Debug.Log("Hit transform pos: "+hit.transform);
        return hit.point;
    }

	public void slideCamera() {
        //Debug.Log ("Slide cameran sisällä");
        //Vector3 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
        //Vector3 newPosition = -touchDeltaPosition - dragOrigin;
        //Debug.Log ("touchDeltaposition = " + touchDeltaPosition);
        //Debug.Log ("dragorigin = " + dragOrigin);
        //Debug.Log ("newposition = " + newPosition);
        //gameObject.transform.Translate (-touchDeltaPosition * dragSpeed);
        //Camera.main.transform.Translate(-touchDeltaPosition * dragSpeed);
        //cameraDragging = false;

            //slideCamera ();
            //for (Touch touch
            //Debug.Log("CameraDragging = " + cameraDragging);
            //slideCamera();
            //Vector3 currentTouch = Input.GetTouch(0).position;
            Vector3 worldDelta = this.getWorldPoint(currentTouch.position) - this.getWorldPoint(dragOriginTouch.position);

        Camera.main.transform.Translate(-worldDelta.x, -worldDelta.y, 0);
        //Camera.main.transform.position = new Vector3(position.x-worldDelta.x, position.y -worldDelta.y, 0);
        //cameraDragging = false;

    }
	
	public void setCameraDragTrue() {
		cameraDragging = true;
	}

	public void setDragOrigin(Vector3 startDragPos) {
	//	dragOrigin = startDragPos;
		//Debug.Log ("dragOrigin = " + dragOrigin);
	}

    public void setDragOriginTouch(Touch touch)
    {
        dragOriginTouch = touch;
        position = Camera.main.transform.position;
        //Debug.Log ("dragOrigin = " + dragOrigin);
    }

    public void setCurrentTouch(Touch touch)
    {
        currentTouch = touch;
    }
}