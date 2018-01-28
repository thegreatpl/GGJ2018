using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityOwnership : MonoBehaviour {

    public int Faction;

    /// <summary>
    /// The type of this entity. 
    /// </summary>
    public EntityType Type;

    /// <summary>
    /// The HP of this entity. 
    /// </summary>
    public int MaxHP;

    public int CurrentHP { get; protected set; } 



	// Use this for initialization
	void Start () {
        CurrentHP = MaxHP; 
	}
	
	// Update is called once per frame
	void Update () {
        //kills the entity. 
        if (CurrentHP < 0)
            Destroy(gameObject); 
	}

    public void TakeDamage(int amount)
    {
        CurrentHP -= amount;
        if (Type == EntityType.Player)
        {
            SoundFXManager.GamesSoundFXManager.PlaySound(SFXType.Hit); 
        }
    }
}
