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
    public float duration = 50.0f;

	// Use this for initialization
	void Start () {
        zAxis = gameObject.transform.position.z;
	}
	
	// Update is called once per frame
	void Update () {

        if((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)){
            //Declare a variable of Raycasthit struct
            RaycastHit hit;
            //Create a ray on the tapped position
            Ray ray;

            ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            
            if (Physics.Raycast(ray, out hit))
            {
                flag = true;
                endPoint = hit.point;
                endPoint.z = zAxis;
                print("endPoint");
            }
        }
        //check if the flag for movement is true and the current gameobject position is not same as the clicked position
        if (flag && !Mathf.Approximately(gameObject.transform.position.magnitude, endPoint.magnitude))
        {
            //move the gameobject to the desired position
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, endPoint, 1 / (duration * (Vector3.Distance(gameObject.transform.position, endPoint))));
        }
        //set the movement indicator flag to false if the endPoint and current gameobject position are equal
        else if (flag && Mathf.Approximately(gameObject.transform.position.magnitude, endPoint.magnitude))
        {
            flag = false;
            print("I am here");
        }
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
