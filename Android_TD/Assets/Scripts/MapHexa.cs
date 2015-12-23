using UnityEngine;
using System.Collections;

public class MapHexa : MonoBehaviour {

	public enum HexDir{W, NW, NE, E, SE, SW};

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
		case HexDir.NE:
			return getNWNeighbour ();
		case HexDir.NW:
			
		default:
			break;
		}
		return null;
	}

	private GameObject getNWNeighbour() {
		int row = getParentRowId () - 1;
		GameObject rowObject = GameObject.Find ("Map").GetComponent<GenerateMap> ().getRow (row);
		int id = 
		rowObject.GetComponent<MapRow> ().getHexagon ();
	}
}
