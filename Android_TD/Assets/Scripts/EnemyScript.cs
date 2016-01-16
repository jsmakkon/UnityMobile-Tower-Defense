using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

	// Add new enemytypes here, also add new case to switch below (to Create() ) 
	// and function to initialize values
	public enum EnemyType {
		Basic
	}

	public static Object enemyPrefab = Resources.Load ("Prefabs/EnemyPrefab");

    public int enemyID;
	public string enemyName;
	public float hp;
	public float strength;
	public float speed;
	public float armor;
	public float mResist;
    //morale
	// TODO: add rest of the variables

	// Use this function to create enemy TODO: add animation to spawning here
	public static EnemyScript Create(EnemyType type, Vector3 position) {
		GameObject newObject = (GameObject)Instantiate(enemyPrefab);
		EnemyScript script = newObject.GetComponent<EnemyScript>();
		// Specific settings for different enemy types
		switch (type) {
		case EnemyType.Basic:
			script.setBasicEnemy ();
			break;
		default:
			script.setBasicEnemy ();
			Debug.Log ("Set Basic enemy in case Default");
			break;
		}
		return script;
	}

	private void setBasicEnemy() {
		enemyName = "Basic Soldier";
		hp = 100;
		strength = 1;
		speed = 1;
		armor = 0;
		mResist = 0;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
