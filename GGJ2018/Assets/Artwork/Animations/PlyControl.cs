using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlyControl : MonoBehaviour {

    public string Horizontal;
    public string Vertical;
    public string Fire;
    public Color Color;


    EntityMovement EntityMovement;
    EntityOwnership Team; 

    public GameObject InfectBullet;

    Direction PrevDirect = Direction.None; 

	void Start () {
        EntityMovement = gameObject.GetComponent<EntityMovement>();
        Team = gameObject.GetComponent<EntityOwnership>(); 
    
    }

    // Update is called once per frame
    void Update() {

        
        float moveX = Input.GetAxis(Horizontal);
        float moveY = Input.GetAxis(Vertical);


        if (moveX < 0)
        {
            PrevDirect = EntityMovement.Direction;

            EntityMovement.Direction = Direction.West; 
        }

        else if (moveX > 0)
        {
            PrevDirect = EntityMovement.Direction;

            EntityMovement.Direction = Direction.East;
        }
        
        else if (moveY < 0)
        {
            PrevDirect = EntityMovement.Direction;

            EntityMovement.Direction = Direction.South; 
        }

        else if (moveY > 0)
        {
            PrevDirect = EntityMovement.Direction;
            EntityMovement.Direction = Direction.North; 
        }

        else
        {
            if (EntityMovement.Direction != Direction.None)
                PrevDirect = EntityMovement.Direction;

            EntityMovement.Direction = Direction.None; 
        }

        if (Input.GetButtonUp(Fire))
        {
            var direct = EntityMovement.Direction;
            if (direct == Direction.None)
                direct = PrevDirect;

            var bullet = Instantiate(InfectBullet, transform.position.Direction(direct), transform.rotation);
            var bulletScript = bullet.GetComponent<InfectBulletScript>();
            var bulletMove = bullet.GetComponent<EntityMovement>();
            bulletMove.Direction = direct;
            bulletMove.Speed = bulletScript.BulletSpeed; 

            //var vel = Vector3.zero.Direction(direct);
            //bulletScript.Velocity = new Vector3(vel.x * bulletScript.BulletSpeed, vel.y * bulletScript.BulletSpeed, 0);
            bulletScript.Faction = Team.Faction;
           // Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());

        }

    }
}
