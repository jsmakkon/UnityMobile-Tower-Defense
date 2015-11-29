using UnityEngine;
using System.Collections;

public class characterMovement : MonoBehaviour {

    //Flag to check if user has tapped the screen
    private bool flag = false;
    //destination point
    private Vector3 endPoint;
    //Vertical position of the gameobject
    private float zAxis;
    //Movement speed of gameObject
    public float speed = 0;
    float touchTime = 0;
	// Use this for initialization
	void Start () {
        zAxis = gameObject.transform.position.z;
	}

    // Update is called once per frame
    void Update()
    {
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
                                //Declare a variable of Raycasthit struct
                                RaycastHit hit;
                                //Create a ray on the tapped position
                                Ray ray;

                                ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

                                //Set the flag as true and get the endpoint
                                if (Physics.Raycast(ray, out hit))
                                {
                                    flag = true;
                                    endPoint = hit.point;
                                    endPoint.z = zAxis;
                                    print("endPoint");
                                }
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

        //check if the flag for movement is true and the current gameobject position is not same as the clicked position
        if (flag && !Mathf.Approximately(gameObject.transform.position.magnitude, endPoint.magnitude))
        {
            moveCharacter();
        }
        //set the movement indicator flag to false if the endPoint and current gameobject position are equal
        else if (flag && Mathf.Approximately(gameObject.transform.position.magnitude, endPoint.magnitude))
        {
            flag = false;
            print("I am here");
        }
    }

    void moveCharacter ()
    {
            //move the gameobject to the desired position
            float step = speed * Time.deltaTime;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, endPoint, step);
    }

}

       /* int numTouches = Input.touchCount;

        if (numTouches > 0){
            //Checks how many touched were detected and in what position
            print(numTouches + " touches detected");

            for (int i = 0; i < numTouches; i++)
            {
                Touch touch = Input.GetTouch(i);
                //print("Touch index " + touch.fingerId + " detected at position " + touch.position);
                TouchPhase phase = touch.phase;

                switch(phase)
                {
                    case TouchPhase.Began:
                        print("New touch phase detected at position" + touch.position + " , index " + touch.fingerId);
                        break;
                    case TouchPhase.Ended:
                        print("Touch index " + touch.fingerId + " ended at position " + touch.position);
                        break;

                }
            }
        }*/
