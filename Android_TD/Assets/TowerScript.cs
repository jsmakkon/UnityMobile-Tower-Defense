using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerScript : MonoBehaviour {

    GameObject gameController;
    public static GameObject towerPrefab = (GameObject)Resources.Load("TowerPrefab", typeof(GameObject));
    public static GameObject towerParent;

    public GameObject shootTarget;

    public List<GameObject> targetsInArea;

	// Use this for initialization
	void Awake () {
        gameController = GameObject.Find("GameController");
        towerParent = GameObject.Find("Towers");
        shootTarget = null;
        targetsInArea = new List<GameObject>();
    }

    public static void CreateTower(Vector3 position)
    {
        Vector3 newPos = new Vector3(position.x, position.y,Constants.towerDepth);
        GameObject newObject = (GameObject)Instantiate(towerPrefab,newPos,towerPrefab.transform.rotation);
        newObject.transform.SetParent(towerParent.transform);
    }

    void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.transform.parent.name == "Enemies")
        {
            // TODO: Handle second trigger differently
            if (targetsInArea.Contains(other.gameObject))
                return;
            Debug.Log("OnTrigEnter");
            targetsInArea.Add(other.gameObject);
            if (shootTarget == null)
            {
                shootTarget = other.gameObject;
                InvokeRepeating("ShootTarget", 0.5f, 1.0f);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform.parent.name == "Enemies")
        {
            // TODO: Handle second trigger differently
            if (!targetsInArea.Contains(other.gameObject))
                return;
            Debug.Log("OnTrigExit");
            targetsInArea.Remove(other.gameObject);
            if (other.gameObject == shootTarget)
            {
                if (targetsInArea.Count != 0)
                    shootTarget = targetsInArea[targetsInArea.Count - 1];
                else
                {
                    shootTarget = null;
                    CancelShooting();
                }
                    
            }
            InvokeRepeating("ShootTarget", 0.5f, 1.0f);
        }
    }

    void ShootTarget()
    {
        if (shootTarget != null)
            Debug.Log("Shooting" + shootTarget.name);
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
        }
    }
    
    public void CancelShooting()
    {
        CancelInvoke("ShootTarget");
    }
}
