using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	public static Object enemyPrefab = Resources.Load("EnemyPrefab", typeof(GameObject));

    public int enemyID;
	public string enemyName;
	public EnemyStats stats;
	public EnemyResistances resistances;
	public List<StatusEffect> statusEffects = new List<StatusEffect>();
	// TODO: add rest of the variables

	// Use this to create enemy TODO: add animation to spawning here
	public static EnemyScript CreateEnemy(EnemyType type, GameObject startPosition, GameObject nextBlock) {
		GameObject newObject = (GameObject)Instantiate(enemyPrefab);
		EnemyScript script = newObject.GetComponent<EnemyScript>();
		// Specific settings for different enemy types TODO
		switch (type) {
		case EnemyType.Basic:
			script.setBasicEnemy ();
			break;
		default:
			script.setBasicEnemy ();
			Debug.Log ("Set Basic enemy in case Default");
			break;
		}

        newObject.GetComponent<EnemyMovement>().SetInits(startPosition.transform.position, nextBlock);
        return script;
	}

	private void setBasicEnemy() {
		enemyName = "Basic Soldier";
		stats.hp = 100;
		stats.strength = 1;
		stats.speed = 1;
		stats.morale = 100;
		resistances.physical = 0;
		resistances.magic = 0;

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
