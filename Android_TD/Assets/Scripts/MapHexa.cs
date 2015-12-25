using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapHexa : MonoBehaviour {

	public enum HexDir{W, NW, NE, E, SE, SW};

	public struct Coordinate {
		public int rowId;
		public int hexaId;
	}

	//[HideInInspector] 
	public int hexaID;

	public List<Material> materials;

	public int getParentRowId() {
		return transform.GetComponentInParent<MapRow> ().rowID;
	}
	public bool getParentRowOffset() {
		return transform.GetComponentInParent<MapRow> ().offSetRow;
	}

	public GameObject getNeighbour(HexDir direction) {
		switch(direction) {
		case HexDir.E:
			return transform.GetComponentInParent<MapRow> ().getHexagon (hexaID + 1);
		case HexDir.W:
			return transform.GetComponentInParent<MapRow> ().getHexagon (hexaID - 1);
		case HexDir.NW:
			return getNWNeighbour ();
		case HexDir.NE:
			return getNENeighbour ();
		case HexDir.SE:
			return getSENeighbour ();
		case HexDir.SW:
			return getSWNeighbour ();
		default:
			break;
		}
		return null;
	}

	public void getIds(ref Coordinate coord) {
		coord.hexaId = hexaID;
		coord.rowId = getParentRowId ();
	}

	public void getIds(ref int hexId, ref int rowId) {
		hexId = hexaID;
		rowId = getParentRowId ();
	}

	private GameObject getNWNeighbour() {
		int row = getParentRowId () - 1;
		GameObject rowObject = GameObject.Find ("Map").GetComponent<GenerateMap> ().getRow (row);
		if (rowObject == null)
			return null;

		int hexid;
		if (getParentRowOffset ()) {
			hexid = hexaID;
		} else
			hexid = hexaID-1;
		return rowObject.GetComponent<MapRow> ().getHexagon(hexid);
	}

	private GameObject getNENeighbour() {
		int row = getParentRowId () - 1;
		GameObject rowObject = GameObject.Find ("Map").GetComponent<GenerateMap> ().getRow (row);
		if (rowObject == null)
			return null;

		int hexid;
		if (getParentRowOffset ()) {
			hexid = hexaID+1;
		} else
			hexid = hexaID;
		return rowObject.GetComponent<MapRow> ().getHexagon(hexid);
	}

	private GameObject getSWNeighbour() {
		int row = getParentRowId () + 1;
		GameObject rowObject = GameObject.Find ("Map").GetComponent<GenerateMap> ().getRow (row);
		if (rowObject == null)
			return null;

		int hexid;
		if (getParentRowOffset ()) {
			hexid = hexaID;
		} else
			hexid = hexaID-1;
		return rowObject.GetComponent<MapRow> ().getHexagon(hexid);
	}

	private GameObject getSENeighbour() {
		int row = getParentRowId () + 1;
		GameObject rowObject = GameObject.Find ("Map").GetComponent<GenerateMap> ().getRow (row);
		if (rowObject == null)
			return null;

		int hexid;
		if (getParentRowOffset ()) {
			hexid = hexaID+1;
		} else
			hexid = hexaID;
		return rowObject.GetComponent<MapRow> ().getHexagon(hexid);
	}
}
