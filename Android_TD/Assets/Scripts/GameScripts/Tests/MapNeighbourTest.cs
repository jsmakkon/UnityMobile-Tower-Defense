using UnityEngine;
using System.Collections;

public class MapNeighbourTest : MonoBehaviour {

	public GameObject testHexa;

	public Material ground;

	void Update() {
		if (testHexa == null)
			return;
		int hexid = -1; int rowid = -1;
		testHexa.GetComponent<MapHexa> ().getIds (ref hexid,ref rowid);
		testHexa.GetComponent<Renderer> ().sharedMaterial = ground;
		Debug.Log ("TestHexa Found with id: "+hexid+ " and rowid: "+rowid);
		MapHexa script = testHexa.GetComponent<MapHexa> ();
		GameObject hex = script.getNeighbour (MapHexa.HexDir.W);
		if (hex != null)
			Debug.Log ("W neighbour is : "+hex.GetComponent<MapHexa> ().hexaID);
		hex = script.getNeighbour (MapHexa.HexDir.NW);
		if (hex != null)
			Debug.Log ("NW neighbour is : "+hex.GetComponent<MapHexa> ().hexaID);
		hex = script.getNeighbour (MapHexa.HexDir.NE);
		if (hex != null)
			Debug.Log ("NE neighbour is : "+hex.GetComponent<MapHexa> ().hexaID);
		hex = script.getNeighbour (MapHexa.HexDir.E);
		if (hex != null)
			Debug.Log ("E neighbour is : "+hex.GetComponent<MapHexa> ().hexaID);
		hex = script.getNeighbour (MapHexa.HexDir.SE);
		if (hex != null)
			Debug.Log ("SE neighbour is : "+hex.GetComponent<MapHexa> ().hexaID);
		hex = script.getNeighbour (MapHexa.HexDir.SW);
		if (hex != null)
			Debug.Log ("SW neighbour is : "+hex.GetComponent<MapHexa> ().hexaID);
		
		


	}
}
