using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public float cameraZoomSpeed = 0.2f;
    public float closestZoomValue = 0.2f;

    public void ZoomCamera(float delta)
    {
        Camera.main.orthographicSize += delta * cameraZoomSpeed;

        Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize, closestZoomValue);
    }
    
}
