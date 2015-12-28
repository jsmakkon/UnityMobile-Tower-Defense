using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using RowList = System.Collections.Generic.List<UnityEngine.GameObject>;

// Data holder class for map hexas, roads end of road...

public class MapData : MonoBehaviour {

	public GameObject gameController;

	public class RoadEnd{

		public GameObject gameController;

		public RoadEnd(GameObject gc) {
			gameController = gc;
		}

		public enum EndPositions
		{
			None, NWCorner,NECorner,SWCorner,SECorner, East, West
		};

		private MapHexa.Coordinate endCoordinate;
		private EndPositions endPos;

		public void setEndPos (EndPositions end) {
			Debug.Log ("Setting endposition to "+end);
			endPos = end;
		}

		public EndPositions getEndPos() {
			return endPos;
		}

		public int hp;

		public MapHexa.Coordinate getCoords() {
			return endCoordinate;
		}

		public void setCoords(MapHexa.Coordinate coords) {
			RowList rows=gameController.GetComponent<MapData> ().getRowList ();
			rows [coords.rowId].GetComponent<MapRow> ().getHexagon (coords.hexaId).GetComponent<MapHexa> ().setType(MapHexa.HexType.End);
			endCoordinate = coords;
		}
	}

	private RowList rowList;
	private Roads roads;
	private RoadEnd roadEnd;


	void Awake() {
		//inits
		//Debug.Log("Runninng inits");
		rowList = new List<GameObject> ();
		roadEnd = new RoadEnd (gameController);
		roads = new Roads ();
	}

	public List<GameObject> getRowList() {
		return rowList;
	}

	public RoadEnd getRoadEnd() {
		return roadEnd;
	}
		
	public Roads getRoads() {
		return roads;
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
