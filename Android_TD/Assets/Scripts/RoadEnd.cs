using UnityEngine;
using System.Collections;

public class RoadEnd : MonoBehaviour {

	public enum EndPositions
	{
		None, NWCorner,NECorner,SWCorner,SECorner, East, West
	};

	public MapHexa.Coordinate endCoordinate;

	public EndPositions endPos;

	public int hp;
}
