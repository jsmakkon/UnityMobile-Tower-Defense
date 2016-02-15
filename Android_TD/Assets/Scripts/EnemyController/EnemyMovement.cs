using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

    public float minDistance = 0.5f;
    public float xSpeed;
    public float ySpeed;
    public float zSpeed;

    GameObject currentTarget;
    public GameObject gameController;
    MapData mapData;
    EnemyScript enemyScript;

    void Awake()
    {
        gameController = GameObject.Find("GameController");
        mapData = gameController.GetComponent<MapData>();
        enemyScript = GetComponent<EnemyScript>();
    }

    public void SetInits(Vector3 startPoint, GameObject nextTarget)
    {
        currentTarget = nextTarget;
        transform.position = new Vector3(startPoint.x, startPoint.y, -1.1f);
        setVelocity();
        Debug.Log("Set speed to: " + GetComponent<Rigidbody>().velocity);
        Debug.Log("Pos of target: " + currentTarget.transform.position + " and our pos: " + transform.position);
    }

	// Update is called once per frame
	void Update () {
       // xSpeed = GetComponent<Rigidbody>().velocity.x;
        //ySpeed = GetComponent<Rigidbody>().velocity.y;
        //zSpeed = GetComponent<Rigidbody>().velocity.z;
        if (isTargetPointNear(currentTarget.transform.position))
        {
            int roadid = currentTarget.GetComponent<MapHexa>().roadBlock.getRoadId();
            int blockid = currentTarget.GetComponent<MapHexa>().roadBlock.getBlockId();
            currentTarget = mapData.getNextHexaInRoad(roadid, blockid);
            setVelocity();

        }
	}

    private void setVelocity()
    {
        GetComponent<Rigidbody>().velocity = (currentTarget.transform.position - transform.position).normalized * enemyScript.Speed;
        Debug.Log("EnemySpeed " + enemyScript.Speed);
        GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x,
            GetComponent<Rigidbody>().velocity.y,
            0.0f);
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
