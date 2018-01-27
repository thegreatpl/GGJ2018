using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectBulletScript : MonoBehaviour {
    /// <summary>
    /// The spawner. 
    /// </summary>
    public static SpawnerController SpawnerController; 

    public Vector3 Velocity = Vector3.zero;

    public int Faction = 0; 

    public int Life = 100;

    /// <summary>
    /// How much damage this bullet does. 
    /// </summary>
    public int Damage = 1; 

    /// <summary>
    /// How fast the bullet flies. 
    /// </summary>
    public float BulletSpeed = 1.5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //kill it. 
        if (Life < 0)
            Destroy(gameObject);

        transform.position += Velocity; 

        Life--; 
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var owner = collision.gameObject.GetComponent<EntityOwnership>();

        if (owner == null)
        {
            Destroy(gameObject);
            return; 
        }

        if (owner.Faction != Faction && owner.Type == EntityType.Civilian)
        {
            SpawnerController.SpawnZombie(Faction, collision.gameObject); 
            Destroy(gameObject); 
        }
        else if (owner.Faction != Faction)
        {
            owner.HP -= Damage;
            Destroy(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
            //Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }

    }

    public void SpawnZombie(GameObject civilian)
    {

    }
}
