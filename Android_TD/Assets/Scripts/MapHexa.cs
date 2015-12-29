using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapHexa : MonoBehaviour {

	public enum HexDir{W, NW, NE, E, SE, SW};

	public enum HexType{Grass, Road, Mountain,End};

	public struct Coordinate {
		public int rowId;
		public int hexaId;
	}

	//[HideInInspector] 
	public int hexaID;
	public List<Material> materials;
	private HexType hexType;
	public GameObject gameController;
	public Roads.Road.RoadBlock roadBlock = null; // For debug
	public int rbid;

	void Awake() {
		hexType = HexType.Grass;
		setTexture (0);
		gameController = GameObject.Find ("GameController");
	}

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

	public MapHexa.Coordinate getCoords() {
		MapHexa.Coordinate coord;
		coord.hexaId = hexaID;
		coord.rowId = getParentRowId ();
		return coord;
	}

	public void getIds(ref int hexId, ref int rowId) {
		hexId = hexaID;
		rowId = getParentRowId ();
	}

	private GameObject getNWNeighbour() {
		int row = getParentRowId () - 1;
		GameObject rowObject = gameController.GetComponent<MapData> ().getRow (row);
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
		GameObject rowObject = gameController.GetComponent<MapData> ().getRow (row);
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
		GameObject rowObject = gameController.GetComponent<MapData> ().getRow (row);
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
		GameObject rowObject = gameController.GetComponent<MapData> ().getRow (row);
		if (rowObject == null)
			return null;

		int hexid;
		if (getParentRowOffset ()) {
			hexid = hexaID+1;
		} else
			hexid = hexaID;
		return rowObject.GetComponent<MapRow> ().getHexagon(hexid);
	}
	// Sets texture to given id. DO NOT use to set type bound textures, use setType(HexType) instead.
	public void setTexture(int id) {
		GetComponent<Renderer> ().sharedMaterial = materials[id];
	}

	public HexType getHexType() {
		return hexType;
	}
	// Sets the type of the hexa and switces the texture 
	public void setType(HexType type) {
		hexType = type;
		switch (type) {
		case HexType.Grass:
			setTexture (0);
			break;
		case HexType.Road:
			setTexture (1);
			break;
		case HexType.Mountain:
			//setTexture (2);
			break;
		case HexType.End:
			setTexture (3);
			break;
		}
	}
}
