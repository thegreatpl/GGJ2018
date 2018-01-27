using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlyControl : MonoBehaviour {

    public string Horizontal;
    public string Vertical;
    public string Fire;

    EntityMovement EntityMovement; 

	void Start () {
        EntityMovement = gameObject.GetComponent<EntityMovement>();

    
    }

    // Update is called once per frame
    void Update() {

        
        float moveX = Input.GetAxis(Horizontal);
        float moveY = Input.GetAxis(Vertical);
        
        if (moveX < 0)
        {
            EntityMovement.Direction = Direction.West; 
        }

        else if (moveX > 0)
        {
            EntityMovement.Direction = Direction.East;
        }
        
        else if (moveY < 0)
        {
            EntityMovement.Direction = Direction.South; 
        }

        else if (moveY > 0)
        {
            EntityMovement.Direction = Direction.North; 
        }

        else
        {
            EntityMovement.Direction = Direction.None; 
        }


	}
}
