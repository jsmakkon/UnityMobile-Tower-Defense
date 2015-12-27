using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateMap : MonoBehaviour {



	static int rowIDs = 0;

	public GameObject rowPrefab;

	public static int rows = 18;
	public static int columns = 25;

	public int numOfRoadStarts = 3;

	private float wDist = 1.73205f;

	private float rowH = -1.5f;
	private float rowOffset = 0.866025f;

	public List<GameObject> rowList;

	public List<MapHexa.Coordinate> roadStarts;
	private RoadEnd roadEnd;


	public List<List<MapHexa.Coordinate> > roads;
	// Road generator stuff
	public int roadMinLength = 10;



	void Start () {
		// Inits
		rowList = new List<GameObject> ();
		roadStarts = new List<MapHexa.Coordinate> ();
		rowIDs = 0; // Reset for possible new map

		// TEMP: roadend init
		roadEnd = GameObject.Find("GameController").GetComponent<RoadEnd>();
		roadEnd.endPos = RoadEnd.EndPositions.East;
		roadEnd.endCoordinate.hexaId = columns-1;
		roadEnd.endCoordinate.rowId = rows - 1;
		GenerateMapBlocks();
		GenerateRoads ();
	}

	private void GenerateMapBlocks() {
		Vector3 position;
		bool offsetRowFlag;
		// Create rows and hexagons
		for (int i = 0; i < rows; i++){
			Debug.Log ("Starting row adding");
			if (i % 2 == 0) {
				position = new Vector3 (0.0f, i * rowH, 0.0f);
				offsetRowFlag = false; // Offset false if starting from 0
			}
			else {
				position = new Vector3 (rowOffset, i * rowH, 0.0f);
				offsetRowFlag = true; // Offset true if moving the row to the right
			}
			// Create row, with id 0,1,2,3... rows -1;
			GameObject row = (GameObject)Instantiate (rowPrefab, position, this.transform.rotation);
			row.name = "Row" + rowIDs;
			row.transform.SetParent (this.transform);
			row.GetComponent<MapRow> ().rowID = rowIDs;
			row.GetComponent<MapRow> ().offSetRow = offsetRowFlag;
			rowIDs++;
			// Add hexagons with id 0,1,2,3,4... column - 1.
			for (int a = 0; a < columns; a++) {
				row.GetComponent<MapRow> ().AddHex (row, a, a*wDist,0,0);
			}
			rowList.Add (row);
			Debug.Log ("Finishing");
		}
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

	private void GenerateRoads() {
		// Generate road starts
		MapHexa.Coordinate firstCoords;
		switch (roadEnd.endPos) {
		case RoadEnd.EndPositions.East:
			firstCoords.hexaId = 0;
			firstCoords.rowId = Random.Range (1, rows - 1);
			break;
		case RoadEnd.EndPositions.West:
			firstCoords.hexaId = columns;
			firstCoords.rowId = Random.Range (1, rows - 1);
			break;
		default:
			firstCoords.hexaId = 0;
			firstCoords.rowId = Random.Range (1, rows - 1);
			roadEnd.endCoordinate.hexaId = columns - 1;
			roadEnd.endCoordinate.rowId = rows - 1;
			break;
		}

		// Generate first road

		// Generate rest of the roads
	}
}