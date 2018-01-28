using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZombieAI : BaseAI {


    protected EntityOwnership Ownership; 

    protected GameObject Target;

    public float SightRange = 20;

    /// <summary>
    /// How far away they have to be to attack them. 
    /// </summary>
    public float AttackRadius = 1f; 


    public float ChanceToGoForStrongest = 0.3f;

    public int AttackVal = 1;


    public float ChanceToHunt = 0.4f;

    public int CoolDownPeriod = 10; 

    private int _attckCoolDown = 0; 

	// Use this for initialization
	void Start () {
        Movement = GetComponent<EntityMovement>();
        Ownership = GetComponent<EntityOwnership>(); 
    }

    // Update is called once per frame
    void Update () {
        _attckCoolDown--; 
        CurrentTile = MapGenerator.Base.WorldToCell(transform.position);
        if (Target == null)
        {

            if (Random.value < ChanceToHunt && FindTarget())
                return;

            Wander(); 
        }

        if (Target != null)
        {
            MoveToTarget(MapGenerator.Base.WorldToCell(Target.transform.position));
            Attack(); 
        }
	}

    /// <summary>
    /// This finds a target for this zombie to hunt. 
    /// </summary>
    /// <returns></returns>
    protected bool FindTarget()
    {
        var targetListo = new List<GameObject>();
        targetListo.AddRange(SpawnerController.Civilians);
        targetListo.AddRange(SpawnerController.LivingZombies);
        targetListo.AddRange(SpawnerController.Players); 
        var targetList = targetListo
        //Physics2D.OverlapCircleAll(transform.position, SightRange)
        //    .Where(x => x.gameObject.HasComponent<EntityOwnership>())
            .Select(x => new TargetCheck(x.gameObject));

        var ordered =
            Random.value < ChanceToGoForStrongest ? 
            (from t in targetList
            where t.EntityOwnership.Faction != Ownership.Faction
            && Vector3.Distance(transform.position, t.GameObject.transform.position) < SightRange
            orderby Vector3.Distance(transform.position, t.GameObject.transform.position) ascending,
            t.EntityOwnership.Type descending
            select t)
            :
            (from t in targetList
            where t.EntityOwnership.Faction != Ownership.Faction
            && Vector3.Distance(transform.position, t.GameObject.transform.position) < SightRange
            orderby Vector3.Distance(transform.position, t.GameObject.transform.position) ascending,
            t.EntityOwnership.Type ascending
            select t);

        foreach(var target in ordered)
        {
            if (target.EntityOwnership.HP < 0)
                continue;
            //shouldn't happen, but might. 
            if (target.EntityOwnership.Faction == Ownership.Faction)
                continue;

            Target = target.GameObject;
            return true; 
        }
        return false;  
    }

    protected void Attack()
    {
        if (_attckCoolDown > 0)
            return; 

        var distance = Vector3.Distance(transform.position, Target.transform.position);

        if (distance > AttackRadius)
            return;

        var otherOwner = Target.GetComponent<EntityOwnership>(); 
        if (otherOwner.Type == EntityType.Civilian)
        {
            SpawnerController.SpawnZombie(Ownership.Faction, Target);
            return; 
        }

        otherOwner.HP -= AttackVal;
        _attckCoolDown = CoolDownPeriod; 
    }
}
