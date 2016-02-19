using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EnemyScript : MonoBehaviour {

	// Add new enemytypes here, also add new case to switch below (to Create() ) 
	// and function to initialize values
	public enum EnemyType {
		Basic // TODO:
	}
	public enum EnemyStatusEffect {
		Poison, Shield // TODO:
	}

	public struct StatusEffect{
		EnemyStatusEffect effect;
		float value;
	}

	public struct EnemyStats
	{
		public float hp;
        public float speed;
		public float strength;
		public float morale;
	}
    // Using get set properties..
    public float Speed
    {
        get
        {
            return stats.speed;
        }
        set
        {
            stats.speed = value;
        }
    }
	public struct EnemyResistances
	{
		public float physical;
		public float magic;
	}
	public static GameObject enemyPrefab = (GameObject)Resources.Load("EnemyPrefab", typeof(GameObject));

    public static GameObject enemiesGroup;

    public static int ID = 0;

    private int enemyID;
	public string enemyName;
	public EnemyStats stats;
	public EnemyResistances resistances;
	public List<StatusEffect> statusEffects = new List<StatusEffect>();
	// TODO: add rest of the variables

    void Awake()
    {
        enemiesGroup = GameObject.Find("Enemies");
    }

    void Start()
    {

    }

	// Use this to create enemy TODO: add animation to spawning here
	public static EnemyScript CreateEnemy(EnemyType type, GameObject startPosition, GameObject nextBlock) {
		GameObject newObject = (GameObject)Instantiate(enemyPrefab);
        newObject.transform.SetParent(enemiesGroup.transform);
        newObject.name = "Enemy " + ID;
        newObject.GetComponent<EnemyScript>().SetEnemyId(ID);
        ID++;
		EnemyScript script = newObject.GetComponent<EnemyScript>();
		// Specific settings for different enemy types TODO
		switch (type) {
		case EnemyType.Basic:
			script.SetBasicEnemy ();
			break;
		default:
			script.SetBasicEnemy ();
			Debug.Log ("Set Basic enemy in case Default");
			break;
		}

        newObject.GetComponent<EnemyMovement>().SetInits(startPosition.transform.position, nextBlock);
        return script;
	}

    private void SetEnemyId(int id)
    {
        enemyID = id;
    }

    private int GetEnemyId()
    {
        return enemyID;
    }
    private void SetBasicEnemy() {
		enemyName = "Basic Soldier";
		stats.hp = 100;
		stats.strength = 1;
		stats.speed = 1;
		stats.morale = 100;
		resistances.physical = 0;
		resistances.magic = 0;

	}
    
    public void TakeDamage()
    {
        Destroy(gameObject);
       // Debug.Log("I died");
    }

	// Update is called once per frame
	void Update () {
	
	}
}
