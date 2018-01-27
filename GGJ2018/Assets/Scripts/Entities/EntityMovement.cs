using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovement : MonoBehaviour {

    Animator VisualControl;

    /// <summary>
    /// The movement speed of this entity. 
    /// </summary>
    public float Speed = 1f;

    /// <summary>
    /// Direction this entity is moving in. 
    /// </summary>
    public Direction Direction = Direction.None; 


    // Use this for initialization
    void Start () {

        VisualControl = gameObject.GetComponent<Animator>();
        Direction = Direction.None; 
	}
	
	// Update is called once per frame
	void Update () {

        switch (Direction)
        {
            case Direction.West:
                VisualControl.SetInteger("Direction", 3);
                VisualControl.SetBool("Walk", true);
                transform.position += new Vector3(-1, 0, 0) * Time.deltaTime;
                break;

            case Direction.East:

                VisualControl.SetInteger("Direction", 4);
                VisualControl.SetBool("Walk", true);
                transform.position += new Vector3(1, 0, 0) * Time.deltaTime;
                break;

            case Direction.South:
                VisualControl.SetInteger("Direction", 2);
                VisualControl.SetBool("Walk", true);
                transform.position += new Vector3(0, -1, 0) * Time.deltaTime;
                break;

            case Direction.North:
                VisualControl.SetInteger("Direction", 1);
                VisualControl.SetBool("Walk", true);
                transform.position += new Vector3(0, 1, 0) * Time.deltaTime;
                break;

            default:
                VisualControl.SetBool("Walk", false);
                break;
        }
	}
}
