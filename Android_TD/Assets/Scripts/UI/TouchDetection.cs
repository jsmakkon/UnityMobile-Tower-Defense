using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TouchDetection : MonoBehaviour {
	public GameObject character;
	public GameObject cameraObject;
    public GameObject selecter;

    private Touch beginTouch;

    float touchTime = 0;

	//Change the time touch is defined as a tap
	public float tapTime = 0.5f;
    public float moveTime = 0.3f;

	Vector3 startDragPos;

    public GameObject gameController;

    private bool twoTouchMode = false;
    private bool isMoved = false;

	// Use this for initialization
    void Awake ()
    {
        gameController = GameObject.Find("GameController");
        selecter = GameObject.Find("MapHexaSelectHighlight");
    }
	
	// Update is called once per frame
	void Update () {
		int numTouches = Input.touchCount;

        if (numTouches == 0)
            twoTouchMode = false;
         
		if (numTouches > 0)
		{
            // Don't do anything if we are zooming
            if (numTouches >= 2)
            {
                twoTouchMode = true;
                gameController.GetComponent<PinchZoom>().DoPinchZoom();
                return;
            }

            Touch touch = Input.GetTouch(0);
			TouchPhase phase = touch.phase;

            
			//Checks that number of touches is 1
			if (numTouches == 1 && !twoTouchMode)
			{
				switch (phase)
				{
				case TouchPhase.Began:

                    touchTime = Time.time;
                    beginTouch = touch;
                        
                    cameraObject.GetComponent<DragCamera>().setDragOriginTouch(touch);
                        
                    break;

				case TouchPhase.Ended:
					// This checks if touch was a TAP
					if (Time.time - touchTime <= tapTime && !isMoved)
					{
						Debug.Log("tap");
                            

                        // If we didn't click menu, check for tile click
                        if (!checkForMenuTap())
                        {
                            checkForTileTap();
                            character.GetComponent<CharacterMovement>().setFlagTrue();
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
                        cameraObject.GetComponent<DragCamera>().slideCamera();
                        cameraObject.GetComponent<DragCamera>().setDragOriginTouch(touch);

                    }
                    break;

				}
			}
		}
	}
	
    // Raycast for tiles
    private bool checkForTileTap()
    {
        Ray ray;
        ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        RaycastHit hit; 
        if (Physics.Raycast(ray, out hit, 100f, 1 << 10)) // Cast to maphexa layer
        {
            GameObject hitHexa = hit.transform.gameObject;
            //Debug.Log("Hit hexa in checkforTile: "+ hitHexa.name);
            selecter.GetComponent<SelectedHexa>().switchSelectedHexa(hitHexa);
            return true;
        }
        return false;
    }

    private bool checkForMenuTap()
    {
        PointerEventData ped = new PointerEventData(EventSystem.current);
        ped.position = Input.GetTouch(0).position;

        // create an empty list of raycast results
        List<RaycastResult> hits = new List<RaycastResult>();

        EventSystem.current.RaycastAll(ped, hits);

        if (hits.Count != 0) // Cast to UI layer
        {
            return true;
        }
        return false;
    }
}