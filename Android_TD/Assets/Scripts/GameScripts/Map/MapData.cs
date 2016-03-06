using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using RowList = System.Collections.Generic.List<UnityEngine.GameObject>;

// Data holder class for map hexas, roads end of road. 
// Works also as a interface to manipulate everything in a map

public class MapData : MonoBehaviour {

    //public GameObject gameController;
    public GameObject map;

	private RowList rowList; // Hexagons, currently as gameobjectlist TODO: create class
	private Roads roads; // Roads 
	private Roads.RoadEnd roadEnd; // End of the road

    public static int rows = 18;
    public static int columns = 25;

    public int testingSave;

	void Awake() {
		//inits
		rowList = new List<GameObject> ();
		roadEnd = new Roads.RoadEnd (this.gameObject);
		roads = new Roads ();
	}
    // Starts map generation
    void Start()
    {
        GameObject datacarrier = GameObject.Find("DataCarrier");
        if (datacarrier != null)
            map.GetComponent<GenerateMap>().generateMap(datacarrier.GetComponent<DataCarrierScript>().seed);
        else
            map.GetComponent<GenerateMap>().generateMap();
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

    public GameObject getHexa(int row, int col)
    {
        MapHexa.Coordinate coords;
        coords.rowId = row;
        coords.hexaId = col;
        return getHexa(coords);
    }

    public GameObject getNextHexaInRoad(int roadId, int blockId)
    {
        Roads.Road.RoadBlock nextBlock = roads.getRoad(roadId).roadBlocks[blockId].getNextRoadBlock();
        MapHexa.Coordinate nextCoords = nextBlock.coord;
        return getHexa(nextCoords);
    }

    public GameObject getRoadHexa(int roadId, int blockId)
    {
        MapHexa.Coordinate coords = roads.getRoad(roadId).getRoadBlock(blockId).coord;
        return getHexa(coords);
    }

    // Spawns enemy to beginning of the road
    public void SpawnEnemyToRoad(int roadId)
    {
        int firstBlockId = roads.getRoad(roadId).getRoadBlockByIndex(0).getBlockId();
        int secondBlockId = roads.getRoad(roadId).getRoadBlockByIndex(0).getNextRoadBlock().getBlockId();

        EnemyScript.CreateEnemy(EnemyScript.EnemyType.Basic, getRoadHexa(roadId, firstBlockId), getRoadHexa(roadId, secondBlockId));
    }

    public void SpawnTower(int row, int column)
    {
        GameObject hex = getHexa(row, column);
        TowerScript.CreateTower(hex);
    }
    public void SpawnTower(GameObject hexa)
    {
        if (hexa.tag == "MapHexa")
        {
            TowerScript.CreateTower(hexa);
        }
    }

    public void setHexaType(int row, int hexa, MapHexa.HexType type)
    {
        GameObject hex = getHexa(row, hexa);
        hex.GetComponent<MapHexa>().setType(type);
    }
    
}
