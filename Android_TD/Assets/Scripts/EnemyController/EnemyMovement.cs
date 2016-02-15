using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

    public float minDistance = 0.5f;

    GameObject currentTarget;
    public GameObject gameController;
    MapData mapData;
    EnemyScript enemyScript;

    void Awake()
    {
        mapData = gameController.GetComponent<MapData>();
        enemyScript = GetComponent<EnemyScript>();
    }

    public void SetInits(GameObject startingPoint)
    {
        currentTarget = startingPoint;
        GetComponent<Rigidbody>().velocity = (currentTarget.transform.position - transform.position).normalized
            * enemyScript.Speed;
    }

	// Update is called once per frame
	void Update () {
	    if (isTargetPointNear(currentTarget.transform.position))
        {
            //TODO: set new target
        }
	}

    bool isTargetPointNear(Vector3 target)
    {
        if (Mathf.Abs(target.x - transform.position.x) < minDistance && Mathf.Abs(target.y - transform.position.y) < minDistance)
        {
            return true;
        }
        else return false;
    }
}
