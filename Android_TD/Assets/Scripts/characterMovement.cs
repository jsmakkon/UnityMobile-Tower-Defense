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
	// Use this for initialization
	void Start () {
        zAxis = gameObject.transform.position.z;
	}

    // Update is called once per frame
    void Update()
    {
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

	//Sets the flag true if tapped screen (TouchDetection.cs)
	public void setFlagTrue () {
		//Declare a variable of Raycasthit struct
		RaycastHit hit;
		//Create a ray on the tapped position
		Ray ray;

		ray = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);

		//Set the flag as true and get the endpoint
		if (Physics.Raycast (ray, out hit)) {
			flag = true;
			endPoint = hit.point;
			endPoint.z = zAxis;
			print ("endPoint");
		}
	}
}