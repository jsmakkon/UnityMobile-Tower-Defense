using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using RowList = System.Collections.Generic.List<UnityEngine.GameObject>;

// Data holder class for map hexas, roads end of road...

public class MapData : MonoBehaviour {

	public GameObject gameController;

	private RowList rowList; // Hexagons
	private Roads roads; // Roads 
	private Roads.RoadEnd roadEnd; // End of the road

    public int testingSave;


	void Awake() {
		//inits
		rowList = new List<GameObject> ();
		roadEnd = new Roads.RoadEnd (gameController);
		roads = new Roads ();
	}

	public List<GameObject> getRowList() {
		return rowList;
	}

	public Roads.RoadEnd getRoadEnd() {
		return roadEnd;
	}
		
	public Roads getRoads() {
		return roads;
	}
	public void deleteRoads() {
		roads.deleteAllRoads ();

	}

	public GameObject getHexa(MapHexa.Coordinate coords) {
		return getRow (coords.rowId).GetComponent<MapRow> ().getHexagon (coords.hexaId);
	}

	public GameObject getRow(int id) {
		//Debug.Log ("getRow id: "+id+ " and count is " + rowList.Count);

		if (id >= rowList.Count || id < 0)
			return null;
		if (rowList [id].GetComponent<MapRow> ().rowID == id) {
			return rowList [id];
		} else {
			Debug.LogWarning ("Row ids messed. Looking for id: "+ id);
			for (int i = 0; i < rowList.Count; i++) {
				if (rowList [i].GetComponent<MapRow> ().rowID == id) 
					return rowList[i];
			}
		}
		Debug.LogError ("getRow failed to find row with id: "+ id);
		return null;
	}
}
