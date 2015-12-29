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

	public long timeUsedToEndCheck=0;
	public long timeUsedToBaseCheck=0;
	public long timeUsedToDeletes = 0;
	public long timeUsedToBegin = 0;

	void Awake() {
		mapData = gameController.GetComponent<MapData> ();
	}
	void Start () {
		int seed = Random.Range(0,100000);
		Random.seed = 9762;
		Debug.Log ("Seed: "+ seed);
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

		Roads.Road firstRoad = new Roads.Road (0); // First road with id 0
		mapData.getRoads().addRoad(firstRoad); // TODO: move to constructor
		Roads.Road.RoadBlock firstBlock = new Roads.Road.RoadBlock(firstCoords,0);
		firstRoad.addRoadBlock(firstBlock);


		// Generate first road
		System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
		sw.Start ();
		int ret = buildRoad(ref firstRoad, mapData.getHexa(firstCoords).GetComponent<MapHexa>(),firstBlock,1,MapHexa.HexDir.W);
		sw.Stop ();
		if (ret == 0) {
			Debug.Log ("Road build fail");
		} else {
			Debug.Log ("Road build success");
			Debug.Log ("Total time: "+ sw.ElapsedTicks);
			Debug.Log ("Time used to Base: "+timeUsedToBaseCheck);
			Debug.Log ("Time used to Deletes: "+timeUsedToDeletes);
			Debug.Log ("Time used to End: "+timeUsedToEndCheck);
			Debug.Log ("Time used to Begin: "+timeUsedToBegin);
		}


		// Generate rest of the roads
	}
	// Builds first road from start to end.
	private int buildRoad(ref Roads.Road road,MapHexa currentHexa, Roads.Road.RoadBlock currentBlock,int id,MapHexa.HexDir incDir) {
		//if (id == 83)
		//	return 1;
		//MapHexa.Coordinate currentCoords = currentRoadBlock.getCoords ();
		//bool foundNext = false;
		//Debug.Log ("<color=red> Now at "+currentHexa.getCoords().rowId+" "+currentHexa.getCoords().hexaId+" </color> ");
		Roads.Road.RoadBlock newBlock;
		List <MapHexa.HexDir> possibleDirections = initDirectionsExcNbours (incDir);
		//Debug.Log ("Chosen dirs: "+possibleDirections[0]+" "+possibleDirections[1]+" "+possibleDirections[2]);
		System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
		// Randomize next direction
		for (int i = possibleDirections.Count-1; i >= 0; i--) {
			sw.Start ();
			// Randomize direction
			MapHexa.HexDir dir = possibleDirections [Random.Range (0, i+1)];
			possibleDirections.Remove (dir);
			//Debug.Log ("Checking direction "+dir);
			// Take hexa from that direction
			sw.Stop();
			timeUsedToBegin += sw.ElapsedTicks;
			sw.Reset ();
			if (currentHexa.getNeighbour (dir) == null) continue;
			sw.Start ();
			MapHexa hexa = currentHexa.getNeighbour (dir).GetComponent<MapHexa>();
			// Check if we have reached end
			//TODO: check if end is near instead
			sw.Stop();
			timeUsedToBegin += sw.ElapsedTicks;
			sw.Reset();
			sw.Start ();
			if (isEndNear(hexa)) {
				
				sw.Stop ();
				timeUsedToEndCheck += sw.ElapsedTicks;
				sw.Reset ();
				//Debug.Log ("End found");
				newBlock = new Roads.Road.RoadBlock (hexa.getCoords (),id);
				road.addRoadBlock (newBlock);
				return 1;
			}
			sw.Stop ();
			timeUsedToEndCheck += sw.ElapsedTicks;
			//Debug.Log ("<color=red>Going to </color> "+dir);

			// Check if we can go there
			sw.Reset();
			sw.Start ();
			if (isHexaAtTheEdge (hexa.getCoords ())) {
				sw.Stop ();
				timeUsedToBaseCheck += sw.ElapsedTicks;
				sw.Reset();
				//if (id == 1)
					//Debug.Log ("Couldn't go to " + dir+ ", Hexa at the edge");
				// Continue to next direction if we can't
				continue;
			}
			if (isHexaNearThisRoad(hexa.getCoords(),road.roadId,getCounterDir(dir)) ){
				sw.Stop ();
				timeUsedToBaseCheck += sw.ElapsedTicks;
				sw.Reset();
				//Debug.Log ("Couldn't go to "+dir+ ", Hexa near the road counterDir"+getCounterDir(dir)+" id is "+id);
				// Continue to next direction if we can't
					continue;
			}


			//Debug.Log ("<color=red>Found a way to </color> "+dir);
			// Add new roadBlock with given id
			newBlock = new Roads.Road.RoadBlock (hexa.getCoords (),id);
			road.addRoadBlock (newBlock);
			sw.Stop ();
			timeUsedToBaseCheck += sw.ElapsedTicks;
			sw.Reset();
			// Call next roadblock
			int ret = buildRoad(ref road, hexa,newBlock,id+1,getCounterDir(dir));
			// Check if we have been successful reaching the end, set hexa to road
			if (ret == 1) {
				return 1;
			}
			// Leave the block only if ret is 1

			sw.Start ();
			//road.deleteRoadBlock(id);
			sw.Stop ();
			timeUsedToDeletes += sw.ElapsedTicks;
			sw.Reset();
			// Else we have to continue if we can reach the end from here
		}
		// If we fail at this point, clean made block and return to previous block
		Debug.Break();

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

	public bool isEndNear(MapHexa hexa) {
		//MapHexa.Coordinate endCoords = gameController.GetComponent<MapData> ().getRoadEnd ().getCoords ();
		List <MapHexa.HexDir> directions = initDirections ();
		if (hexa == null)
			return false;
		for (int i = 0; i < directions.Count; i++) {
			if (hexa.getNeighbour (directions [i]) == null)
				continue;
			MapHexa nearby = hexa.getNeighbour (directions [i]).GetComponent<MapHexa>();
			if (nearby.getHexType () == MapHexa.HexType.End)
				return true;

		}
		return false;
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
		//Debug.Assert(directions.Count==6);
		directions.Remove (exclude);

		//for (int i = 0; i < directions.Count; i++) 
		//	Debug.Log ("Excluded directions " +directions[i]);
		MapHexa hexa = mapData.getHexa (hexCoords).GetComponent<MapHexa>();
		//bool ret = false;
		// Loop all directions
		//Debug.Assert(directions.Count==5);
		for (int i = 0; i < directions.Count; i++) {
			//Debug.Log ("Checking direction " +directions[i]);
			// Take the nearby hex
			MapHexa nearby = hexa.getNeighbour (directions [i]).GetComponent<MapHexa>();
			// Continue if its no road
			if (nearby.getHexType () != MapHexa.HexType.Road)
				continue;
			// Its a road, now check if it belongs to this road
			else {
				Roads.Road road = mapData.getRoads().getRoad (roadId);
				for (int a = 0; a < road.roadBlocks.Count; a++) {
					if (road.roadBlocks [a].coord.hexaId == nearby.getCoords ().hexaId && road.roadBlocks [a].coord.rowId == nearby.getCoords ().rowId) {
						//Debug.Log ("isHexaNearThisRoad TRUE: row "+ road.roadBlocks [a].coord.rowId+" hex "+road.roadBlocks [a].coord.hexaId+" in direction "+directions[i]);
						return true;
					}
				}
			}
		}
		return false;
	}
	// Check if hex in given coords are in the road already NOT USED
	/*public bool isHexaInThisRoad(MapHexa.Coordinate hexCoords, int roadId) {
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
	}*/

	// Adds direction that excludes given direction and its neighbours
	private List<MapHexa.HexDir> initDirectionsExcNbours(MapHexa.HexDir dir) {
		List<MapHexa.HexDir> directions = new List<MapHexa.HexDir> ();
		switch (dir) {

		case MapHexa.HexDir.E:
			directions.Add (MapHexa.HexDir.NW);
			directions.Add (MapHexa.HexDir.SW);
			directions.Add (MapHexa.HexDir.W);
			return directions;
		
		case MapHexa.HexDir.W:
			directions.Add (MapHexa.HexDir.NE);
			directions.Add (MapHexa.HexDir.SE);
			directions.Add (MapHexa.HexDir.E);
			return directions;
		
		case MapHexa.HexDir.NE:
			directions.Add (MapHexa.HexDir.SE);
			directions.Add (MapHexa.HexDir.SW);
			directions.Add (MapHexa.HexDir.W);
			return directions;
		
		case MapHexa.HexDir.NW:
			directions.Add (MapHexa.HexDir.E);
			directions.Add (MapHexa.HexDir.SW);
			directions.Add (MapHexa.HexDir.SE);
			return directions;
		
		case MapHexa.HexDir.SE:
			directions.Add (MapHexa.HexDir.NW);
			directions.Add (MapHexa.HexDir.NE);
			directions.Add (MapHexa.HexDir.W);
			return directions;
		
		case MapHexa.HexDir.SW:
			directions.Add (MapHexa.HexDir.NW);
			directions.Add (MapHexa.HexDir.NE);
			directions.Add (MapHexa.HexDir.E);
			return directions;
		}
		
		Debug.LogError ("initDirectionsExcNbours error");
		return null;
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