using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CivilianAI : BaseAI {

    

    /// <summary>
    /// Chance to chose a target to more to, as opposed to just wandering randomly. 
    /// </summary>
    public float MoveChance = 0.05f;

    public float SightRange = 0.7f; 


    // Use this for initialization
    void Start () {
        Movement = GetComponent<EntityMovement>();

    }

    // Update is called once per frame
    void Update () {
        CurrentTile = MapGenerator.Base.WorldToCell(transform.position);

        if (Flee())
            return; 

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


    bool Flee()
    {
        var view = Physics2D.OverlapCircleAll(transform.position, SightRange).Select(x => new TargetCheck(x.gameObject));

        var zombs = view.Where(x => x.EntityOwnership?.Type == EntityType.Zombie);

        if (zombs.Count() < 1)
            return false;

        var direction = new Vector3(0, 0); 
        foreach (var z in zombs)
        {
            var d = transform.position - z.GameObject.transform.position;
            direction += d; 
        }

        //direction *= -1;
        MoveInDirection(direction); 
        return true; 
    }


}
