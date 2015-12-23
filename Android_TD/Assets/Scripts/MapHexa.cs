using UnityEngine;
using System.Collections;

public class MapHexa : MonoBehaviour {

	public int hexaID;

	public int getParentRowId() {
		return transform.GetComponentInParent<MapRow> ().rowID;
	}
}
