using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using RowList = System.Collections.Generic.List<UnityEngine.GameObject>;

// Data holder class for map hexas, roads end of road...

public class MapData : MonoBehaviour {

	public GameObject gameController;

	public class RoadEnd{

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
			RowList rows=null;
			GameObject.Find ("GameController").GetComponent<MapData> ().getRowList (ref rows);
			rows [coords.rowId].GetComponent<MapRow> ().getHexagon (coords.hexaId).GetComponent<MapHexa> ().setTexture (1);
			endCoordinate = coords;
		}
	}

	private RowList rowList;
	private List<MapHexa.Coordinate> roadStarts;
	private RoadEnd roadEnd;
	private List<List<MapHexa.Coordinate> > roads;

	void Awake() {
		//inits
		//Debug.Log("Runninng inits");
		rowList = new List<GameObject> ();
		roadStarts = new List<MapHexa.Coordinate> ();
		roadEnd = new RoadEnd ();
		roads = new List<List<MapHexa.Coordinate>> ();
	}

	public void getRowList(ref List<GameObject> rows) {
		rows = rowList;
	}

	public void getRoadEnd(ref RoadEnd road) {
		road = roadEnd;
	}

	public void getRoadStarts(ref List<MapHexa.Coordinate> roadStart) {
		roadStart = roadStarts;
	}

	public void getRoads(ref List<List<MapHexa.Coordinate> > road) {
		road = roads;
	}

	public void addRoad(List<MapHexa.Coordinate> road) {
		roads.Add (road);
	}

	public void deleteRoad(int index) {
		roads.RemoveAt (index);
	}

	public void getRoad(int index, ref List<MapHexa.Coordinate> road) {
		road = roads [index];
	}
}
