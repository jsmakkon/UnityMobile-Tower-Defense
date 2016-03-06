using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerParentScript : MonoBehaviour {

    GameObject[,] towers;

    void Awake()
    {
        towers = new GameObject[MapData.rows,MapData.columns];
        for (int i = 0; i < MapData.rows; i++)
            for (int a = 0; a < MapData.columns; a++)
                towers[i, a] = null;
    }

    public bool isSpotFree(int x, int y)
    {
        Debug.Log("hoi3");
        if (towers[x, y] == null)
            return true;
        else
            return false;
    }

    public bool setTowerToSpot(MapHexa.Coordinate coords, GameObject tower)
    {
        if (towers[coords.rowId, coords.hexaId] == null && tower.tag == "Tower")
        {
            towers[coords.rowId, coords.hexaId] = tower;
            return true;
        }
        return false;
    }


}
