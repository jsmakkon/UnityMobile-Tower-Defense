using UnityEngine;
using System.Collections;

public class TouchDetection : MonoBehaviour {

	//private bool flag = false;
	float touchTime = 0;

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
						break;
					case TouchPhase.Ended:
						// This checks if touch was a TAP
						if (Time.time - touchTime <= 0.5)
						{
							Debug.Log("tap");
							GameObject.Find ("Character").GetComponent<characterMovement> ().setFlagTrue ();
						}
						// This checks if touch was a LONG PRESS
						else
						{
							Debug.Log("Long press");
						}
						break;

					}
				}
			}
		}
	}
}