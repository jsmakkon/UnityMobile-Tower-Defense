using UnityEngine;
using System.Collections;

public class GenerateMap : MonoBehaviour {

	static int rowIDs = 0;

	public GameObject rowPrefab;
	public GameObject hexaPrefab;

	public int rows;
	public int columns;

	private float wDist = 1.73205f;

	private float rowH = 1.5f;
	private float rowOffset = 0.866025f;

	// Use this for initialization
	void Start () {
		Vector3 position;
		// Create rows and hexagons
		for (int i = 0; i < rows; i++){
			if (i%2 == 0)
				position = new Vector3(0.0f,i*rowH,0.0f);
			else 
				position = new Vector3(rowOffset,i*rowH,0.0f);

			GameObject row = (GameObject)Instantiate (rowPrefab, position, this.transform.rotation);
			row.transform.SetParent (this.transform);
			row.GetComponent<MapRow> ().rowID = rowIDs;
			rowIDs++;

			for (int a = 0; a < columns; a++) {
				position = new Vector3(a*wDist,0.0f,0.0f);
				Quaternion rot = this.transform.rotation;
				rot.eulerAngles.Set(0,180,0);
				GameObject hexa = (GameObject)Instantiate (hexaPrefab, position, rot);
				hexa.transform.SetParent (row.transform);
				hexa.GetComponent<MapHexa> ().hexaID = a;
			}
		}
	}
}
