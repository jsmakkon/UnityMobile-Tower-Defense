using UnityEngine;
using System.Collections;

public class DragCamera : MonoBehaviour {

	public float dragSpeed = 2;

    Touch dragOriginTouch; // Prev frame touch
    Touch currentTouch; // Current frame touch

    private Vector3 getWorldPoint (Vector3 screenPoint)
    {
        RaycastHit hit;
        Physics.Raycast(Camera.main.ScreenPointToRay(screenPoint), out hit, 200.0f);
        return hit.point;
    }

    // Move camera for slide delta
	public void slideCamera() {
        Vector3 worldDelta = this.getWorldPoint(currentTouch.position) - this.getWorldPoint(dragOriginTouch.position);
        Camera.main.transform.Translate(-worldDelta.x, -worldDelta.y, 0);
    }
    public void setDragOriginTouch(Touch touch)
    {
        dragOriginTouch = touch;
    }

    public void setCurrentTouch(Touch touch)
    {
        currentTouch = touch;
    }
}