using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectBulletScript : MonoBehaviour {

    public Vector3 Velocity = Vector3.zero;

    public int Faction = 0; 

    public int Life = 100;

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
        //if (Life < 0)
        //    Destroy(gameObject);

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

        if (owner.Faction != Faction)
        {
            SpawnZombie(collision.gameObject); 
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
