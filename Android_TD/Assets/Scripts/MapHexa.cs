using UnityEngine;
using System.Collections;

public class MapHexa : MonoBehaviour {

	public enum HexDir{W, NW, NE, E, SE, SW};
	[HideInInspector] 
	public int hexaID;

	public int getParentRowId() {
		return transform.GetComponentInParent<MapRow> ().rowID;
	}
	public bool getParentRowOffset() {
		return transform.GetComponentInParent<MapRow> ().offSetRow;
	}

	public GameObject getNeighbour(HexDir direction) {
		switch(direction) {
		case HexDir.E:
			return transform.GetComponentInParent<MapRow> ().getHexagon (hexaID - 1);
		case HexDir.W:
			return transform.GetComponentInParent<MapRow> ().getHexagon (hexaID + 1);
		case HexDir.NW:
			return getNWNeighbour ();
		case HexDir.NE:
			
		default:
			break;
		}
		return null;
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
