using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateMap : MonoBehaviour {

	static int rowIDs = 0;

	public GameObject rowPrefab;
	public GameObject gameController;

	private MapData mapData;

	public static int rows = 18;
	public static int columns = 25;

	public int numOfRoadStarts = 3;

	private float wDist = 1.73205f;

	private float rowH = -1.5f;
	private float rowOffset = 0.866025f;

	// Road generator stuff
	public int roadMinLength = 10;

	void Awake() {
		mapData = gameController.GetComponent<MapData> ();
	}
	void Start () {
		
		// Inits
		rowIDs = 0; // Reset for possible new map

		GenerateMapBlocks();


		// TEMP: roadend init
		//Debug.Log("generate inits");
		MapData.RoadEnd roadEnd = gameController.GetComponent<MapData>().getRoadEnd();

		roadEnd.setEndPos( MapData.RoadEnd.EndPositions.East);
		MapHexa.Coordinate coords;
		coords.hexaId = columns-2;
		coords.rowId = rows - 2;
		roadEnd.setCoords (coords);

		GenerateRoads ();

	}

	private void GenerateMapBlocks() {
		Vector3 position;
		bool offsetRowFlag;
		List<GameObject> rowList= gameController.GetComponent<MapData>().getRowList();

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



	private void GenerateRoads() {
		MapData.RoadEnd roadEnd=mapData.getRoadEnd();

		// Generate road starts
		MapHexa.Coordinate firstCoords;

		switch (roadEnd.getEndPos()) {
		case MapData.RoadEnd.EndPositions.East:
			firstCoords.hexaId = 0;
			firstCoords.rowId = Random.Range (1, rows - 1);
			break;
		case MapData.RoadEnd.EndPositions.West:
			firstCoords.hexaId = columns;
			firstCoords.rowId = Random.Range (1, rows - 1);
			break;
		default:
			Debug.LogWarning ("RoadEnd endpos default proc: value is " + roadEnd.getEndPos());
			firstCoords.hexaId = 0;
			firstCoords.rowId = Random.Range (1, rows - 1);
			MapHexa.Coordinate coords;
			coords.hexaId = columns-2;
			coords.rowId = rows - 2;
			roadEnd.setCoords (coords);
			break;
		}

		mapData.getHexa(firstCoords).GetComponent<MapHexa>().GetComponent<MapHexa>().setType(MapHexa.HexType.Road);

		Roads.Road firstRoad = new Roads.Road (0); // First road with id 
		mapData.getRoads().addRoad(firstRoad); // TODO: move to constructor
		// Generate first road
		/*int ret = buildRoad(ref firstRoad, mapData.getHexa(firstCoords).GetComponent<MapHexa>());
		if (ret == 0) {
			Debug.Log ("Road build fail");
		} else {
			Debug.Log ("Road build success");
		}*/


		// Generate rest of the roads
	}
	// Builds first road from start to end.
	private int buildRoad(ref Roads.Road road,MapHexa currentRoadBlock) {
		//MapHexa.Coordinate currentCoords = currentRoadBlock.getCoords ();
		//bool foundNext = false;
		List <MapHexa.HexDir> possibleDirections = initDirections ();
		// Randomize next direction
		for (int i = 5; i >= 0; i--) {
			// Randomize direction
			MapHexa.HexDir dir = possibleDirections [Random.Range (0, i)];
			possibleDirections.Remove (dir);
			// Take hexa from that direction
			if (currentRoadBlock.getNeighbour (dir) == null) continue;
			MapHexa hexa = currentRoadBlock.getNeighbour (dir).GetComponent<MapHexa>();
			// Check if we have reached end
			if (hexa.getHexType () == MapHexa.HexType.End) {
				return 1;
			}
			// Check if we can go there
			if (isHexaAtTheEdge(hexa.getCoords()) || isHexaNearThisRoad(hexa.getCoords(),road.roadId,getCounterDir(dir)) ||
				isHexaInThisRoad(hexa.getCoords(),road.roadId) ){
				// Continue to next direction if we can't
					continue;
			}

			// Call next roadblock
			int ret = buildRoad(ref road, hexa);
			// Check if we have been successful reaching the end, set hexa to road
			if (ret == 1) {
				hexa.setType (MapHexa.HexType.Road);
				return 1;
			}
			// Else we have to continue if we can reach the end from here
		}
		// If we fail at this point, return 0 to fall back one hex
		return 0;
	}

	private MapHexa.HexDir getCounterDir(MapHexa.HexDir hexdir) {
		if (hexdir == MapHexa.HexDir.E)
			return MapHexa.HexDir.W;
		if (hexdir == MapHexa.HexDir.W)
			return MapHexa.HexDir.E;
		if (hexdir == MapHexa.HexDir.NW)
			return MapHexa.HexDir.SE;
		if (hexdir == MapHexa.HexDir.SW)
			return MapHexa.HexDir.NE;
		if (hexdir == MapHexa.HexDir.NE)
			return MapHexa.HexDir.SW;
		if (hexdir == MapHexa.HexDir.SE)
			return MapHexa.HexDir.NW;

		Debug.Log ("Error at getCounterdir");
		return MapHexa.HexDir.E;

	}


	public bool isHexaAtTheEdge(MapHexa.Coordinate hexCoords) {
		if (hexCoords.hexaId == 0 || hexCoords.hexaId == columns - 1 || hexCoords.rowId == 0 || hexCoords.rowId == rows - 1)
			return true;
		else
			return false;
	}
	//Check if nearby hexa is near this road, excluding the direction we are coming from
	public bool isHexaNearThisRoad(MapHexa.Coordinate hexCoords, int roadId, MapHexa.HexDir exclude) {
		List <MapHexa.HexDir> directions = initDirections ();
		directions.Remove (exclude);
		MapHexa hexa = mapData.getHexa (hexCoords).GetComponent<MapHexa>();
		//bool ret = false;
		// Loop all directions
		for (int i = 0; i < directions.Count; i++) {
			// Take the nearby hex
			MapHexa nearby = hexa.getNeighbour (directions [i]).GetComponent<MapHexa>();
			// Continue if its no road
			if (nearby.getHexType () != MapHexa.HexType.Road)
				continue;
			// Its a road, now check if it belongs to this road
			else {
				Roads.Road road = mapData.getRoads().getRoad (roadId);
				for (int a = 0; a < road.roadBlocks.Count; a++) {
					if (road.roadBlocks [a].coord.hexaId == hexCoords.hexaId && road.roadBlocks [a].coord.rowId == hexCoords.rowId)
						return true;
				}
			}
		}
		return false;
	}

	public bool isHexaInThisRoad(MapHexa.Coordinate hexCoords, int roadId) {
		if (mapData.getHexa (hexCoords).GetComponent<MapHexa> ().getHexType () != MapHexa.HexType.Road)
			return false;
		else {
			Roads.Road road = mapData.getRoads().getRoad (roadId);
			for (int i = 0; i < road.roadBlocks.Count; i++){
				if (road.roadBlocks [i].coord.rowId == hexCoords.rowId && road.roadBlocks [i].coord.hexaId == hexCoords.hexaId) {
					return true;
				}
			}
		}
		return false;
	}


	private List<MapHexa.HexDir> initDirections() {
		List<MapHexa.HexDir> directions = new List<MapHexa.HexDir> ();
		directions.Add (MapHexa.HexDir.E);
		directions.Add (MapHexa.HexDir.NW);
		directions.Add (MapHexa.HexDir.NE);
		directions.Add (MapHexa.HexDir.W);
		directions.Add (MapHexa.HexDir.SW);
		directions.Add (MapHexa.HexDir.SE);
		return directions;
	}


}