using UnityEngine;
using System.Collections;

public class CameraButtons : MonoBehaviour {
    public GameObject gameController;
    public GameObject selected;
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
    void Awake()
    {
        selected = GameObject.Find("MapHexaSelectHighlight");
    }

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

    public void CreateEnemy()
    {
        gameController.GetComponent<MapData>().SpawnEnemyToRoad(0);
    }

    public void CreateTower()
    {
        MapData mapData = gameController.GetComponent<MapData>();
        GameObject sel = selected.GetComponent<SelectedHexa>().getSelected();
        if (sel != null)
        {
            Debug.Log("selected not null");
            mapData.SpawnTower(sel);
        }
        
    }
}
