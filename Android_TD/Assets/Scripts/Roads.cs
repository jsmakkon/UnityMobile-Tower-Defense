using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Class to contain road information of the map

public class Roads {
	// Small class for one road
	public class Road
	{
		public List<RoadBlock> roadBlocks;
		public int roadId;

		public class RoadBlock
		{
			public MapHexa.Coordinate coord;
			public MapHexa.HexDir roadDirection;
			public int blockId;

			public RoadBlock(MapHexa.Coordinate coords, int id) {
				coord = coords;
				//roadDirection = dir;
				blockId=id;
			}
		}

		public Road(int id) {
			roadBlocks = new List<RoadBlock>();
			roadId = id;
		}
		// Roadblock must be initiated with proper values before this, as the block is tied to road and the type of hexa is being changed
		public void addRoadBlock(RoadBlock block) {
			if (block == null)
				return;
			roadBlocks.Add (block);
			//GameObject.Find ("GameController").GetComponent<MapData> ().getHexa (block.coord).GetComponent<MapHexa> ().roadBlock = block;
			GameObject.Find ("GameController").GetComponent<MapData> ().getHexa (block.coord).GetComponent<MapHexa> ().rbid = block.blockId;
			GameObject.Find ("GameController").GetComponent<MapData> ().getHexa (block.coord).GetComponent<MapHexa>().setType(MapHexa.HexType.Road);
		}
		/*
		public void deleteRoadBlock(MapHexa.Coordinate coord) {
			for (int i = roadBlocks.Count - 1; i >= 0; i--) {
				if (coord.rowId == roadBlocks [i].coord.rowId && coord.hexaId == roadBlocks [i].coord.hexaId) {
					roadBlocks.RemoveAt (i);
					break;
				}
			}
			GameObject.Find ("GameController").GetComponent<MapData> ().getHexa (coord).GetComponent<MapHexa>().setType(MapHexa.HexType.Grass);
		}*/

		public void deleteRoadBlock(int blockid) {
			//Debug.Log ("Delete block with id "+blockid);
			for (int i = roadBlocks.Count - 1; i >= 0; i--) {
				if (roadBlocks [i].blockId ==blockid) {
					//Debug.Log ("Removing blockid "+ blockid + " from "+ roadBlocks [i].coord.rowId+" "+roadBlocks [i].coord.hexaId);
					//GameObject.Find ("GameController").GetComponent<MapData> ().getHexa (roadBlocks [i].coord).GetComponent<MapHexa> ().roadBlock = null;
					GameObject.Find ("GameController").GetComponent<MapData> ().getHexa (roadBlocks [i].coord).GetComponent<MapHexa> ().rbid = 0;
					GameObject.Find ("GameController").GetComponent<MapData> ().getHexa (roadBlocks [i].coord).GetComponent<MapHexa>().setType(MapHexa.HexType.Grass);
					roadBlocks.RemoveAt (i);
					break;
				}
			}
			//Debug.Log ("Failed to remove block "+ blockid);
		}

		public RoadBlock getRoadBlock(int id) {
			for (int i = 0; i < roadBlocks.Count; i++) {
				if (roadBlocks [i].blockId == id)
					return roadBlocks [i];
			}
			return null;
		}
	}

	private List<Road > roads;

	public Roads () {
		roads = new List<Road> ();
	}

	//TODO: remove by id
	public void deleteRoad(int index) {
		roads.RemoveAt (index);
	}

	public void addRoad(Road road) {
		roads.Add (road);
	}

	public Road getRoad(int id) {
		for (int i = 0; i < roads.Count; i++) {
			if (roads [i].roadId == id)
				return roads [i];
		}
		Debug.Log ("No road was found");
		return null;
	}

	//public RoadBlock getRoadBlock() {
	//
	//}


}
