using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* Road map generating algorithm. */

public class GenerateMap : MonoBehaviour {

	static int rowIDs = 0;

	public GameObject rowPrefab;
	public GameObject gameController;

	private MapData mapData;
    
	// Road generator stuff
	public int roadMinLength = 20;

	void Awake() {
		mapData = gameController.GetComponent<MapData> ();
	}
	void Start () {
        //generateMap();
		
	}
    // Call this to (re)generate map, nothing else is needed here
    public void generateMap(int seed = -1, int numOfRoads = 3, int amountOfMountains = 3)
    {
        if (seed == -1)
            //Random.seed = 468466;
            setRandomSeed();
        else
            Random.seed = seed;

        // Reset for new map
        //rowIDs = 0; 
        // Generate hexas to map
        // generateMapBlocks();
        // Create end of the road (players base)
        //createRoadEnd();
        
        bool success = false;
        while (!success)
        {
            mapData.deleteRoads();
            rowIDs = 0;
            // Recreate everything on fail
            generateMapBlocks();
            createRoadEnd();
            // Generate roads
            if (generateRoads(numOfRoads) == 0)
            {
                setRandomSeed();
                continue;
            }
            // Roads generated, generate mountains
            if (generateMountains(amountOfMountains) == 0)
            {
                setRandomSeed();
                continue;
            }
            success = true;
        }
        

    }

    // Generates hexagons to map
	private void generateMapBlocks() {
		Vector3 position;
		bool offsetRowFlag;
        float rowH = Constants.rowHeight;
        float wDist = Constants.widthDistance;
        float rowOffset = Constants.rowOffset;
        List<GameObject> rowList= gameController.GetComponent<MapData>().getRowList();

		// Clear if we are recreating map
		rowList.Clear ();
		GameObject map = GameObject.Find ("Map");
		for (int i = map.transform.childCount - 1; i >= 0; i--) {
			GameObject child = map.transform.GetChild (i).gameObject;
			for (int a = child.transform.childCount - 1; a >= 0; a--) {
				Destroy(child.transform.GetChild (a).gameObject);
			}
			Destroy(child);
		}

		// Create rows and hexagons
		for (int i = 0; i < MapData.rows; i++){
			
			if (i % 2 == 0) {
				position = new Vector3 (0.0f, i * rowH, 0.0f);
				offsetRowFlag = false; 
			}
			else {
				position = new Vector3 (rowOffset, i * rowH, 0.0f);
				offsetRowFlag = true; // Offset true if row is indented
			}
			// Create row, with id 0,1,2,3... rows -1;
			GameObject row = (GameObject)Instantiate (rowPrefab, position, this.transform.rotation);
			row.name = "Row" + rowIDs;
			row.transform.SetParent (this.transform);
			row.GetComponent<MapRow> ().rowID = rowIDs;
			row.GetComponent<MapRow> ().offSetRow = offsetRowFlag;
			rowIDs++;
			// Add hexagons with id 0,1,2,3,4... column - 1.
			for (int a = 0; a < MapData.columns; a++) {
				row.GetComponent<MapRow> ().AddHex (row, a, a*wDist,0,0);
			}
			rowList.Add (row);
		}
	}



    private int generateRoads(int numOfRoads = 3) {
        //Roads.RoadEnd roadEnd = mapData.getRoadEnd();

        // Generate road starting hexas
        // TODO: get number of roads from playerprefs
        
        List<MapHexa.Coordinate> startCoords = getStartPointCoords(numOfRoads);

        // Set starting hexa type as a Road
        mapData.getHexa(startCoords[0]).GetComponent<MapHexa>().setType(MapHexa.HexType.Road);
		// Create first road and set first block to it
		Roads.Road firstRoad = new Roads.Road (0); // First road with id 0
		mapData.getRoads().addRoad(firstRoad); // TODO: move to constructor
		Roads.Road.RoadBlock firstBlock = new Roads.Road.RoadBlock(startCoords[0], 0);
		firstBlock.finalRoad = true;
		firstRoad.addRoadBlock(firstBlock);
        
		System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
		sw.Start ();
		int roadOneSuccess = buildMainRoad(ref firstRoad, mapData.getHexa(startCoords[0]).GetComponent<MapHexa>(),firstBlock,1,MapHexa.HexDir.W);
		sw.Stop ();
        int roadTwoSuccess = 0;
        if (numOfRoads > 1) { 
		    // Same to second road
		    mapData.getHexa(startCoords[1]).GetComponent<MapHexa>().setType(MapHexa.HexType.Road);

		    Roads.Road secondRoad = new Roads.Road (1); // Second road with id 1
		    mapData.getRoads().addRoad(secondRoad); // TODO: move to constructor
		    Roads.Road.RoadBlock secondBlock = new Roads.Road.RoadBlock(startCoords[1], 0);
		    secondBlock.finalRoad = true;
		    secondRoad.addRoadBlock(secondBlock);
		    // Check if the road is connected with one block already, we discard these roads at the moment TODO: fix
		    if (isOtherRoadNearby (mapData.getHexa(startCoords[1]).GetComponent<MapHexa>(), secondRoad.roadId))
			    roadTwoSuccess = 0;
		    else 
			    // Generate second road
			    roadTwoSuccess = buildSecRoads (ref secondRoad, mapData.getHexa(startCoords[1]).GetComponent<MapHexa>(),secondBlock,1,MapHexa.HexDir.NE);
        }
        int roadThreeSuccess = 0;
        if (numOfRoads > 2) { 
            // ... And to third road TODO: Function this
            mapData.getHexa(startCoords[2]).GetComponent<MapHexa>().setType(MapHexa.HexType.Road);

		    Roads.Road thirdRoad = new Roads.Road (2); // Third road with id 2
		    mapData.getRoads().addRoad(thirdRoad); // TODO: move to constructor
		    Roads.Road.RoadBlock thirdBlock = new Roads.Road.RoadBlock(startCoords[2], 0);
		    thirdBlock.finalRoad = true;
		    thirdRoad.addRoadBlock(thirdBlock);
		    // Check if the road is connected with one block already
		    
		    if (isOtherRoadNearby (mapData.getHexa(startCoords[2]).GetComponent<MapHexa>(), thirdRoad.roadId))
                roadThreeSuccess = 0;
		    else 
		    	// Generate third road
		    	roadThreeSuccess = buildSecRoads (ref thirdRoad, mapData.getHexa(startCoords[2]).GetComponent<MapHexa>(),thirdBlock,1,MapHexa.HexDir.SE);
        }
        if (   roadOneSuccess == 0 
			|| roadTwoSuccess == 0
			|| roadThreeSuccess == 0
		) {
			Debug.Log ("Road build fail");
			Debug.Log ("Total time: "+ sw.ElapsedMilliseconds);
			return 0;
		} else {
			Debug.Log ("Road build success");
			Debug.Log ("Total time: "+ sw.ElapsedMilliseconds);
			return 1;
		}
	}

	private int buildSecRoads(ref Roads.Road road,MapHexa currentHexa, Roads.Road.RoadBlock currentBlock,int id,MapHexa.HexDir incDir) {
		
		Roads.Road.RoadBlock newBlock;
		List <MapHexa.HexDir> possibleDirections = initDirectionsExcNbours (incDir);
        // TODO: add special case when the first hexa is already the nearest

		// Randomize next direction
		for (int i = possibleDirections.Count-1; i >= 0; i--) {

			// Randomize direction
			MapHexa.HexDir dir = possibleDirections [Random.Range (0, i+1)];
			possibleDirections.Remove (dir);
			// Take hexa from that direction
			if (currentHexa.getNeighbour (dir) == null) continue;
			MapHexa hexa = currentHexa.getNeighbour (dir).GetComponent<MapHexa>();
			// Check if we have reached end OR we are colliding other road here
			
			if (isHexaAtTheEdge (hexa.getCoords ())) {
				// Continue to next direction, we are at the edge
				continue;
			}
			MapHexa nearbyHexa = isOtherRoadNearby (hexa, road.roadId);
			if (nearbyHexa != null) {
				// Other road is nearby, we join to it and start finishing the road
				newBlock = new Roads.Road.RoadBlock (hexa.getCoords (),id);
                currentBlock.setNextRoadBlock(newBlock);
				road.addRoadBlock (newBlock);
				newBlock.finalRoad = true;
                newBlock.setNextRoadBlock(nearbyHexa.roadBlock);
				hexa.finalR = true;
				return 1;
			}

			if (isEndNear(hexa)) {
                // The end of the road is near, we join to it and start finishing the road
                newBlock = new Roads.Road.RoadBlock (hexa.getCoords (),id);
                currentBlock.setNextRoadBlock(newBlock);
				road.addRoadBlock (newBlock);
				newBlock.finalRoad = true;
                newBlock.isLastBlock = true; // set so we know next is endroad
				hexa.finalR = true;
				return 1;
			}
			// Check if we can go there

			if (isHexaNearThisRoad(hexa.getCoords(),road.roadId,getCounterDir(dir)) ){
				// Continue to next direction if we can't
				continue;
			}

			// Add new roadBlock with given id
			newBlock = new Roads.Road.RoadBlock (hexa.getCoords (),id);
			road.addRoadBlock (newBlock);
			// Call next roadblock
			int ret = buildSecRoads(ref road, hexa,newBlock,id+1,getCounterDir(dir));
			// Check if we have been successful reaching the end, set hexa to road
			if (ret == 1) {
                currentBlock.setNextRoadBlock(newBlock);
				newBlock.finalRoad = true;
				hexa.finalR = true;
				// Finally, remove all the extra blocks
				if (id == 1) {
					for (int a = road.roadBlocks.Count-1; a >=0; a--) {
						if (!road.roadBlocks [a].finalRoad) {
							road.deleteRoadBlock (road.roadBlocks[a].coord);
						}
					}
                    // check if road is long enough to goal

                    if (isRoadTooShort(road))
                    {
                        Debug.Log("Road " + road.roadId + " is too short..");
                        return 0;
                    }
                        
				}
				return 1;
			}
			// Leave the block only if ret is 1
			// Else we have to continue if we can reach the end from here
		}
		// If we fail at this point, clean made block and return to previous block
		return 0;
	}

    private int expandEnough = 3;
    private int expandDirs = 2;
    private int minSizeOfMountain = 5;

    private int generateMountains(int amountOf = 3)
    {
        int counter = 0;
        for (int i = 0; i < amountOf; i++)
        {
            if (counter > 10) return 0;
            List<GameObject> gatheredHexas = new List<GameObject>();
            //First, pick random spot to generate mountain to
            GameObject hexa = pickMountainStart();
            gatheredHexas.Add(hexa);
            MapHexa mapHex = hexa.GetComponent<MapHexa>();
            // Start expanding to every direction
            List<MapHexa.HexDir> directions = initDirections();
            for (int a = 0; a < directions.Count; a++)
            {
                if (getNeighbourHexType(hexa, directions[a]) == MapHexa.HexType.Grass)
                    gatheredHexas.Add(mapHex.getNeighbour(directions[a]));
            }
            if (gatheredHexas.Count < expandEnough)
            {
                counter++;
                // Try again if we couldn't expand enough
                i--;
                continue;
            }
            // Expand to 2 random direction a bit further TODO:
            /*for (int a = 0; a < expandDirs; a++)
            {
                gatheredHexas
            }*/
            for (int a = 0;a < gatheredHexas.Count; a++)
            {
                gatheredHexas[a].GetComponent<MapHexa>().setType(MapHexa.HexType.Mountain);
            }
            counter = 0;
        }
        

        return 1;
    }

    private MapHexa.HexType getNeighbourHexType(GameObject hex, MapHexa.HexDir dir)
    {
        return hex.GetComponent<MapHexa>().getNeighbour(dir).GetComponent<MapHexa>().getHexType();
    }

    // TODO: Optimize by tracking previous misses
    private GameObject pickMountainStart()
    {
        int safeCounter = 0;
        int col = Random.Range(1, MapData.columns-1);
        int row = Random.Range(1, MapData.rows-1);
        GameObject hexa = mapData.getHexa(row,col);
        while (hexa.GetComponent<MapHexa>().getHexType() != MapHexa.HexType.Grass)
        {
            col = Random.Range(1, MapData.columns-1);
            row = Random.Range(1, MapData.rows-1);
            hexa = mapData.getHexa(row, col);
            safeCounter++;
            if (safeCounter > 10000)
            {
                Debug.LogError("Infinite loop at pickMountainStart()");
                return null;
            }
                
        }
        return hexa;
    }

	// Checks if there is road with some other id than the given one and returns that hexa.
	private MapHexa isOtherRoadNearby(MapHexa hexa, int myRoadId) {
		List <MapHexa.HexDir> directions = initDirections ();
		if (hexa == null)
			return null;
		for (int i = 0; i < directions.Count; i++) {
			if (hexa.getNeighbour (directions [i]) == null)
				continue;
			MapHexa nearby = hexa.getNeighbour (directions [i]).GetComponent<MapHexa>();

			if (nearby.getHexType () == MapHexa.HexType.Road && nearby.roadBlock != null && nearby.roadBlock.getRoadId() != myRoadId) {
				return nearby;
			}

		}
		return null;
	}
    // Return true if road too short.
    // Only takes account the given road and the next road, so we are assuming
    // there is no 3+ road chains
    private bool isRoadTooShort(Roads.Road road)
    {
        Roads.Road.RoadBlock nextBlock = road.roadBlocks[road.roadBlocks.Count - 1].getNextRoadBlock();
        if (nextBlock == null)
        {
            if (road.roadBlocks.Count < roadMinLength)
                return true;
            else
                return false;
        }
        if (road.roadBlocks.Count + mapData.getRoads().getDistanceToEndOfRoad(nextBlock.getRoadId(), nextBlock.getBlockId()) < roadMinLength)
        {
            return true;
        }
        else return false;
    }

	// Builds first road from start to end.
	private int buildMainRoad(ref Roads.Road road,MapHexa currentHexa, Roads.Road.RoadBlock currentBlock,int id,MapHexa.HexDir incDir) {
		//Debug.Log ("<color=red> Now at "+currentHexa.getCoords().rowId+" "+currentHexa.getCoords().hexaId+" </color> ");
		Roads.Road.RoadBlock newBlock;
		List <MapHexa.HexDir> possibleDirections = initDirectionsExcNbours (incDir);

		// Randomize next direction
		for (int i = possibleDirections.Count-1; i >= 0; i--) {
			
			// Randomize direction
			MapHexa.HexDir dir = possibleDirections [Random.Range (0, i+1)];
			possibleDirections.Remove (dir);
			// Take hexa from that direction

			if (currentHexa.getNeighbour (dir) == null) continue;
			MapHexa hexa = currentHexa.getNeighbour (dir).GetComponent<MapHexa>();
			// Check if we have reached end
			if (isEndNear(hexa)) {
				
				//Debug.Log ("End found");
				newBlock = new Roads.Road.RoadBlock (hexa.getCoords (),id);
                currentBlock.setNextRoadBlock(newBlock);
				road.addRoadBlock (newBlock);
				newBlock.finalRoad = true;
                newBlock.isLastBlock = true;
				hexa.finalR = true;
				return 1;
			}

			// Check if we can go there
			if (isHexaAtTheEdge (hexa.getCoords ())) {
				// Continue to next direction if we can't
				continue;
			}
			if (isHexaNearThisRoad(hexa.getCoords(),road.roadId,getCounterDir(dir)) ){
				// Continue to next direction if we can't
					continue;
			}
				
			// Add new roadBlock with given id
			newBlock = new Roads.Road.RoadBlock (hexa.getCoords (),id);
			road.addRoadBlock (newBlock);
			// Call next roadblock
			int ret = buildMainRoad(ref road, hexa,newBlock,id+1,getCounterDir(dir));
			// Check if we have been successful reaching the end, set hexa to road
			if (ret == 1) {
                currentBlock.setNextRoadBlock(newBlock);
				newBlock.finalRoad = true;
				hexa.finalR = true;
				// Finally, remove all the extra blocks
				if (id == 1) {
					for (int a = road.roadBlocks.Count-1; a >=0; a--) {
						if (!road.roadBlocks [a].finalRoad) {
							road.deleteRoadBlock (road.roadBlocks[a].coord);
						}
					}
				}
				return 1;
			}
			// Leave the block only if ret is 1
			// Else we have to continue if we can reach the end from here
		}
		// If we fail at this point, we return without marking the road as final road
		// so this road will be deleted later.

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
		if (hexCoords.hexaId == 0 || hexCoords.hexaId == MapData.columns - 1 || hexCoords.rowId == 0 || hexCoords.rowId == MapData.rows - 1)
			return true;
		else
			return false;
	}
	//Check if nearby hexa is near this road, excluding the direction we are coming from
	public bool isHexaNearThisRoad(MapHexa.Coordinate hexCoords, int roadId, MapHexa.HexDir exclude) {
		List <MapHexa.HexDir> directions = initDirections ();
		directions.Remove (exclude);
        
		MapHexa hexa = mapData.getHexa (hexCoords).GetComponent<MapHexa>();
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
					if (road.roadBlocks [a].coord.hexaId == nearby.getCoords ().hexaId && road.roadBlocks [a].coord.rowId == nearby.getCoords ().rowId) {
						return true;
					}
				}
			}
		}
		return false;
	}

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

	private void setRandomSeed() {
		int seed = Random.Range(0,1000000);
		Random.seed = seed;
		Debug.Log ("Seed: "+ Random.seed);
	}

	// TODO: Create randomness
	private void createRoadEnd() {
		Roads.RoadEnd roadEnd = mapData.getRoadEnd();
		roadEnd.setEndPos( Roads.RoadEnd.EndPositions.East);
		MapHexa.Coordinate coords;
		coords.hexaId = MapData.columns -2;
		coords.rowId = MapData.rows - 2;
		roadEnd.setCoords (coords);
	}

    private List<MapHexa.Coordinate> getStartPointCoords(int count = 3)
    {
        List<MapHexa.Coordinate> list = new List<MapHexa.Coordinate>();
        Roads.RoadEnd roadEnd = mapData.getRoadEnd();

        MapHexa.Coordinate coords1 = new MapHexa.Coordinate();
        MapHexa.Coordinate coords2 = new MapHexa.Coordinate();
        MapHexa.Coordinate coords3 = new MapHexa.Coordinate();

        switch (roadEnd.getEndPos())
        {
            case Roads.RoadEnd.EndPositions.East:
                coords1.hexaId = 0;
                coords1.rowId = Random.Range(1, MapData.rows - 1);
                list.Add(coords1);
                if (count > 1)
                {
                    coords2.rowId = 0;
                    coords2.hexaId = Random.Range(1, MapData.columns - 5);
                    list.Add(coords2);
                }
                if (count > 2)
                {
                    coords3.rowId = MapData.rows - 1;
                    coords3.hexaId = Random.Range(1, MapData.columns - 5);
                    list.Add(coords3);
                }
                break;
            case Roads.RoadEnd.EndPositions.West:
                coords1.hexaId = MapData.columns;
                coords1.rowId = Random.Range(1, MapData.rows - 1);
                list.Add(coords1);
                if (count > 1)
                {
                    coords2.rowId = 0;
                    coords2.hexaId = Random.Range(5, MapData.columns - 1);
                    list.Add(coords2);
                }
                if (count > 2)
                {
                    coords3.rowId = MapData.rows - 1;
                    coords3.hexaId = Random.Range(5, MapData.columns - 1);
                    list.Add(coords3);
                }
                break;
            default:
                // Default at East
                Debug.LogWarning("RoadEnd endpos default proc: value is " + roadEnd.getEndPos());
                coords1.hexaId = 0;
                coords1.rowId = Random.Range(1, MapData.rows - 1);
                list.Add(coords1);
                if (count > 1)
                {
                    coords2.rowId = 0;
                    coords2.hexaId = Random.Range(1, MapData.columns - 5);
                    list.Add(coords2);
                }
                if (count > 2)
                {
                    coords3.rowId = MapData.rows - 1;
                    coords3.hexaId = Random.Range(1, MapData.columns - 5);
                    list.Add(coords3);
                }
                break;
        }

        return list;
    }
}