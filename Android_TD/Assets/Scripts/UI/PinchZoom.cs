using UnityEngine;
using System.Collections;

/* Pinch zoom, taken from unity tutorials*/

public class PinchZoom : MonoBehaviour {
    
    public GameObject gameController;

    void Awake ()
    {
        gameController = GameObject.Find("GameController");
    }

	public void DoPinchZoom () {

        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);

        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

        float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

        gameController.GetComponent<CameraController>().ZoomCamera(deltaMagnitudeDiff);
    }
}
