using UnityEngine;
using System.Collections;

public static class Constants {

	public const int RoadEndId = -2;
    public const float towerDepth = -1.2f;
    public const float enemyDepth = towerDepth;
    public const float projectileDepth = towerDepth;
    public static Vector3 posBehindCamera { get { return new Vector3(0, 0, -100); } }

    // Maphexa position constants
    public const float rowOffset = 0.866025f;
    public const float widthDistance = 1.73205f;
    public const float rowHeight = -1.5f;

}
