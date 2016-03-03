using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapRow : MonoBehaviour {

	public int rowID;
	public bool offSetRow;

	public GameObject hexaPrefab;

	public List<GameObject> hexList;

	void Awake() {
		hexList = new List<GameObject> ();
	}

	public void AddHex(GameObject parent, int id, float xPos,float yPos,float zPos) {
		Vector3 position = new Vector3(xPos,yPos,zPos);
		Quaternion rot = this.transform.rotation;
		rot.Set(0,180,0,0);
		GameObject hexa = (GameObject)Instantiate (hexaPrefab, position, rot);
		hexa.transform.SetParent (parent.transform);
		hexa.transform.localPosition = position;
		hexa.name = "Hexa" + id;
		hexa.GetComponent<MapHexa> ().hexaID = id;
		hexList.Add (hexa);
	}
	// Get hexagon from this row.
	// Complexity: Constant if hexas are in order, linear otherwise
	public GameObject getHexagon(int id) {
		if (id > hexList.Count-1 || id < 0)
			return null;
		if (hexList [id].GetComponent<MapHexa> ().hexaID == id) {
			return hexList [id];
		} else {
			Debug.LogWarning ("Hexagon ids messed. Looking for id: "+ id);
			for (int i = 0; i < hexList.Count; i++) {
				if (hexList [i].GetComponent<MapHexa> ().hexaID == id) 
					return hexList [i];
			}
		}
		Debug.LogError ("getHexagon failed to find hexagon with id: "+ id);
		return null;
	}

}
