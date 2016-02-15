using UnityEngine;
using System.Collections;

public class TouchDetection : MonoBehaviour {
	public GameObject character;
	public GameObject cameraObject;
	//private bool flag = false;
	float touchTime = 0;
	//Change the time touch is defined as a tap
	public float tapTime = 0.5f;

	Vector3 startDragPos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		int numTouches = Input.touchCount;

		if (numTouches > 0)
		{

			for (int i = 0; i < numTouches; i++)
			{
				Touch touch = Input.GetTouch(i);
				TouchPhase phase = touch.phase;

				//Checks that number of touches is 1
				if (numTouches == 1)
				{
					switch (phase)
					{
					case TouchPhase.Began:
						//Check when the touch began
						touchTime = Time.time;
						startDragPos = touch.position;
						break;
					case TouchPhase.Ended:
						// This checks if touch was a TAP
						if (Time.time - touchTime <= tapTime)
						{
							Debug.Log("tap");
							character.GetComponent<CharacterMovement> ().setFlagTrue ();
						}
						// This checks if touch was a LONG PRESS
						else
						{
							Debug.Log("Long press");
						}
						break;
					case TouchPhase.Moved:

						//Debug.Log ("Inside TouchPhase.Moved");
						cameraObject.GetComponent<DragCamera> ().setDragOrigin (startDragPos);
						cameraObject.GetComponent<DragCamera> ().setCameraDragTrue ();
						break;

					}
				}
			}
		}
	}
}