using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityOwnership : MonoBehaviour {

    public int Faction;

    /// <summary>
    /// The type of this entity. 
    /// </summary>
    public string Type;

    /// <summary>
    /// The HP of this entity. 
    /// </summary>
    public int HP;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //kills the entity. 
        if (HP < 0)
            Destroy(gameObject); 
	}
}
