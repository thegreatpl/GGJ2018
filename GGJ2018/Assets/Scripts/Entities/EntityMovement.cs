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
	}
	
	// Update is called once per frame
	void Update () {

        switch (Direction)
        {
            case Direction.West:
                if (VisualControl != null)
                {
                    VisualControl.SetInteger("Direction", 3);
                    VisualControl.SetBool("Walk", true);
                }
                transform.position += new Vector3(-Speed, 0, 0) * Time.deltaTime;
                break;

            case Direction.East:
                if (VisualControl != null)
                {
                    VisualControl.SetInteger("Direction", 4);
                    VisualControl.SetBool("Walk", true);
                }
                transform.position += new Vector3(Speed, 0, 0) * Time.deltaTime;
                break;

            case Direction.South:
                if (VisualControl != null)
                {
                    VisualControl.SetInteger("Direction", 2);
                    VisualControl.SetBool("Walk", true);
                }
                transform.position += new Vector3(0, -Speed, 0) * Time.deltaTime;
                break;

            case Direction.North:
                if (VisualControl != null)
                {
                    VisualControl.SetInteger("Direction", 1);
                    VisualControl.SetBool("Walk", true);
                }
                transform.position += new Vector3(0, Speed, 0) * Time.deltaTime;
                break;

            default:
                if (VisualControl != null)
                    VisualControl.SetBool("Walk", false);
                break;
        }
	}
}
