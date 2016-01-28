using UnityEngine;
using System.Collections;

public class CameraButtons : MonoBehaviour {
    public GameObject gameController;
    /*
	public void OnClick() {
		Debug.Log("Transform tag is: " + transform.tag);
		if (transform.tag == "PlusButton") {
			LiftCamera();
		}
		if (transform.tag == "MinusButton") {
			LiftCamera();
		}
	}
	*/
    public void LiftCamera() {
		// TODO: add maximums
		Camera.main.orthographicSize = Camera.main.orthographicSize + 5;
	}

	public void LowerCamera() {
		Camera.main.orthographicSize = Camera.main.orthographicSize - 5;
	}

    public void Save()
    {
        gameController.GetComponent<DataSaver>().Save();
    }

    public void Load()
    {
        gameController.GetComponent<DataSaver>().Load();
    }
}
