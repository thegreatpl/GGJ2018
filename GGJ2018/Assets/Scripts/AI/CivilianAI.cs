using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianAI : BaseAI {

    

    /// <summary>
    /// Chance to chose a target to more to, as opposed to just wandering randomly. 
    /// </summary>
    public float MoveChance = 0.05f;



    // Use this for initialization
    void Start () {
        Movement = GetComponent<EntityMovement>();

    }

    // Update is called once per frame
    void Update () {
        CurrentTile = MapGenerator.Base.WorldToCell(transform.position);

        if (TargetTile == Vector3Int.zero)
        {
            if (Random.value < MoveChance)
            {
                SetRandomTarget();
                return; 
            }

            Wander();
            return; 
        }

        BaseUpdate(); 
	}


}
