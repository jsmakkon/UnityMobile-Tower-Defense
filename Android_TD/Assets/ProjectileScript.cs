using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {

    GameObject gameController;
    public static GameObject projectilePrefab = (GameObject)Resources.Load("BasicProjectilePrefab", typeof(GameObject));
    public static GameObject projectilesParent;

    public GameObject currentTarget;

    public float projectileSpeed = 5.0f;

    void Awake()
    {
        gameController = GameObject.Find("GameController");
        projectilesParent = GameObject.Find("Projectiles");
    }

    void Start()
    {
        SetVelocity();
    }

    public static void CreateProjectile(Vector3 position, GameObject target)
    {
        Vector3 newPos = new Vector3(position.x, position.y, Constants.projectileDepth);
        GameObject newObject = (GameObject)Instantiate(projectilePrefab, newPos, projectilePrefab.transform.rotation);

        Quaternion newRotation = Quaternion.LookRotation((target.transform.position - newObject.transform.position).normalized);
        newObject.transform.rotation = newRotation;
        newObject.transform.RotateAround(newObject.transform.position,new Vector3(0,0,1), 90.0f);

        newObject.transform.SetParent(projectilesParent.transform);

        newObject.GetComponent<ProjectileScript>().currentTarget = target;
    }

    private void SetVelocity()
    {
        GetComponent<Rigidbody>().velocity = (currentTarget.transform.position - transform.position).normalized * projectileSpeed;
        //Debug.Log("EnemySpeed " + enemyScript.Speed);
        GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x,
            GetComponent<Rigidbody>().velocity.y,
            0.0f);
    }

    void OnTriggerEnter (Collider other)
    {
       
        if (other.gameObject.transform.parent.name == "Enemies")
        {
            Debug.Log("Projectile OnTriggerEnter: " + other.gameObject.transform.parent.name);
            Destroy(gameObject);
            other.gameObject.GetComponent<EnemyScript>().TakeDamage();
        }
    }
}
