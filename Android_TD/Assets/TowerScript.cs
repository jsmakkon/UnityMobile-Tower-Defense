using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerScript : MonoBehaviour {

    GameObject gameController;
    public static GameObject towerPrefab = (GameObject)Resources.Load("TowerPrefab", typeof(GameObject));
    public static GameObject towerParent;
    

    public GameObject shootTarget;

    private GameObject hexa;

    private float nextShot = 0.0f;

    public List<GameObject> targetsInArea;

    public float firingCooldown = 1.0f;
	// Use this for initialization
	void Awake () {
        gameController = GameObject.Find("GameController");
        towerParent = GameObject.Find("Towers");
        shootTarget = null;
        targetsInArea = new List<GameObject>();
    }

    public static bool CreateTower(GameObject mapHexa)
    {
        GameObject towerP = GameObject.Find("Towers");
        MapHexa.Coordinate coords = mapHexa.GetComponent<MapHexa>().getCoords();
        if (towerP.GetComponent<TowerParentScript>().isSpotFree(coords.rowId, coords.hexaId))
        {
            Debug.Log("hoi2");
            Vector3 newPos = new Vector3(mapHexa.transform.position.x, mapHexa.transform.position.y, Constants.towerDepth);
            GameObject newObject = (GameObject)Instantiate(towerPrefab, newPos, towerPrefab.transform.rotation);
            
            newObject.GetComponent<TowerScript>().setMapHexa(mapHexa);
            newObject.transform.SetParent(towerParent.transform);
            towerParent.GetComponent<TowerParentScript>().setTowerToSpot(coords, newObject);
            return true;
        }
        Debug.Log("Tower creation failed");
        return false;
    }

    public void setMapHexa(GameObject mapHexa)
    {
        hexa = mapHexa;
    }

    public GameObject setMapHexa()
    {
        return hexa;
    }

    void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.transform.parent.name == "Enemies")
        {
            // TODO: Handle second trigger differently
            if (targetsInArea.Contains(other.gameObject))
                return;
            //Debug.Log("OnTrigEnter");
            targetsInArea.Add(other.gameObject);
            if (shootTarget == null)
            {
                shootTarget = other.gameObject;
                //InvokeRepeating("ShootTarget", 0.5f, 1.0f);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform.parent.name == "Enemies")
        {
            if (!targetsInArea.Contains(other.gameObject))
                return;
            targetsInArea.Remove(other.gameObject);
            if (other.gameObject == shootTarget)
            {
                // Set new target here
                if (targetsInArea.Count != 0)
                    shootTarget = targetsInArea[targetsInArea.Count - 1];
                /*else
                {
                    shootTarget = null;
                    CancelShooting();
                }*/
            }
        }
    }

    void ShootTarget()
    {
        if (shootTarget != null)
        {
            ProjectileScript.CreateProjectile(transform.position, shootTarget);
        }
        /*
        else
        {
            // Target is destroyed
            if (targetsInArea.Count != 0)
                shootTarget = targetsInArea[targetsInArea.Count - 1];
            else
            {
                shootTarget = null;
                CancelShooting();
            }
        }*/
    }
    /*
    public void CancelShooting()
    {
        CancelInvoke("ShootTarget");
    }*/

    void Update()
    {
        if (shootTarget == null && targetsInArea.Count != 0)
        {
            shootTarget = targetsInArea[targetsInArea.Count - 1];
        }
        if (Time.time > nextShot && shootTarget != null)
        {
            ShootTarget();
            nextShot = Time.time + firingCooldown;
        }
    }
}
