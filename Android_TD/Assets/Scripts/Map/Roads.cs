using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using RowList = System.Collections.Generic.List<UnityEngine.GameObject>;
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
			public int roadId = -1;
			public bool finalRoad=false;
			// Next block in the road. Enemies can use this to move
			// through the road.


			public struct RoadBlockData
			{
				public int roadId;
				public int roadBlockId;
			};

			public RoadBlockData nextRoadBlock;

			public RoadBlock(MapHexa.Coordinate coords, int id) {
				coord = coords;
				//roadDirection = dir;
				blockId=id;
				nextRoadBlock.roadId = -1;
				nextRoadBlock.roadBlockId = -1;
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
			block.roadId = this.roadId;
			roadBlocks.Add (block);
			GameObject.Find ("GameController").GetComponent<MapData> ().getHexa (block.coord).GetComponent<MapHexa> ().roadBlock = block;
			GameObject.Find ("GameController").GetComponent<MapData> ().getHexa (block.coord).GetComponent<MapHexa> ().rbid = block.blockId;
			GameObject.Find ("GameController").GetComponent<MapData> ().getHexa (block.coord).GetComponent<MapHexa> ().setType (MapHexa.HexType.Road);
		}

		public void deleteRoadBlock(MapHexa.Coordinate coord) {
			for (int i = roadBlocks.Count - 1; i >= 0; i--) {
				if (coord.rowId == roadBlocks [i].coord.rowId && coord.hexaId == roadBlocks [i].coord.hexaId) {
					roadBlocks.RemoveAt (i);
					break;
				}
			}
			GameObject.Find ("GameController").GetComponent<MapData> ().getHexa (coord).GetComponent<MapHexa> ().roadBlock = null;
			GameObject.Find ("GameController").GetComponent<MapData> ().getHexa (coord).GetComponent<MapHexa> ().rbid = 0;
			GameObject.Find ("GameController").GetComponent<MapData> ().getHexa (coord).GetComponent<MapHexa>().setType(MapHexa.HexType.Grass);
		}

		public void deleteRoadBlock(int blockid) {
			//Debug.Log ("Delete block with id "+blockid);
			for (int i = roadBlocks.Count - 1; i >= 0; i--) {
				if (roadBlocks [i].blockId ==blockid) {
					//Debug.Log ("Removing blockid "+ blockid + " from "+ roadBlocks [i].coord.rowId+" "+roadBlocks [i].coord.hexaId);
					GameObject.Find ("GameController").GetComponent<MapData> ().getHexa (roadBlocks [i].coord).GetComponent<MapHexa> ().roadBlock = null;
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

	public void deleteAllRoads() {
		roads.Clear ();
	}

	public int getCount() {
		return roads.Count;
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
	// Assuming the roadId is the same as the position in arrays
	public Road.RoadBlock getRoadBlock(int rId, int rBlockId) {
		if (roads [rId].roadId == rId)
			return roads [rId].getRoadBlock (rBlockId);
		else {
			for (int i = 0; i < roads.Count; i++) {
				if (roads [i].roadId == rId)
					return roads [i].getRoadBlock (rBlockId);
			}
		}
		Debug.Log ("Returning null from getRoadBlock with search of "+rId +" "+ rBlockId);
		return null;
	}

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
}
