using UnityEngine;
using System.Collections;

public class TouchDetection : MonoBehaviour {
	public GameObject character;
	public GameObject cameraObject;
    public Material tappedMaterial;
    public Material originalMaterial;

    private GameObject previousHit;
    private Vector3 placeOfTap;
    //Vertical position of the gameobject
    private int tappedHexa;
    private float zAxis;

    //private bool flag = false;
    float touchTime = 0;
	//Change the time touch is defined as a tap
	public float tapTime = 0.5f;
    public float moveTime = 0.3f;

	Vector3 startDragPos;

    private bool isMoved = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		int numTouches = Input.touchCount;

		if (numTouches > 0)
		{

			//for (int i = 0; i < numTouches; i++)
			//{
				Touch touch = Input.GetTouch(0);
				TouchPhase phase = touch.phase;

				//Checks that number of touches is 1
				if (numTouches == 1)
				{
					switch (phase)
					{
					case TouchPhase.Began:
						//Check when the touch began
						touchTime = Time.time;
						//startDragPos = touch.position;
                        cameraObject.GetComponent<DragCamera>().setDragOriginTouch(touch);
                        break;
					case TouchPhase.Ended:
						// This checks if touch was a TAP
						if (Time.time - touchTime <= tapTime && !isMoved)
						{
							Debug.Log("tap");
							character.GetComponent<CharacterMovement> ().setFlagTrue ();
						}
						// This checks if touch was a LONG PRESS
						else if (!isMoved)
						{
							Debug.Log("Long press");
                                
                                //Select a hexa on map and change its material
                                Ray ray;
                                ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                                RaycastHit[] hits;

                                hits = Physics.RaycastAll(ray,200.0f);
                                Debug.Log("Hits count: " + hits.Length);
                                for(int j = 0; j < hits.Length; j++)
                                {

                                    RaycastHit hit = hits[j];
                                    Debug.Log("Touched object: " + hit.collider.gameObject.name);
                                    Renderer rend = hit.transform.GetComponent<Renderer>();

                                    if (hit.collider.gameObject.transform.parent.parent.name == "Map")
                                    {
                                        //TODO: Check if null parent
                                        if (previousHit != null)
                                        {
                                            previousHit.GetComponent<Renderer>().material = originalMaterial;
                                        }
                                            originalMaterial = hit.collider.GetComponent<Renderer>().material;
                                            previousHit = hit.collider.gameObject;
                                            rend.material = tappedMaterial;
                                    }
                                }
                            }
                        isMoved = false;
						break;
					case TouchPhase.Moved:
                        if (Time.time - touchTime >= moveTime)
                        {
                            isMoved = true;
                            //Check for camera slide
                            cameraObject.GetComponent<DragCamera>().setCurrentTouch(touch);
                            //cameraObject.GetComponent<DragCamera> ().setDragOrigin (startDragPos);
                            //cameraObject.GetComponent<DragCamera> ().setCameraDragTrue ();
                            cameraObject.GetComponent<DragCamera>().slideCamera();
                            cameraObject.GetComponent<DragCamera>().setDragOriginTouch(touch);

                        }
                        break;

					}
				}
			}
		}
	//}
}