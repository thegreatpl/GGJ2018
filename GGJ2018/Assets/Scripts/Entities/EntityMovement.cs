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
        if (VisualControl == null)
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

    /// <summary>
    /// Sets the animator on this entity. 
    /// </summary>
    /// <param name="animator"></param>
    public void SetAnimator(RuntimeAnimatorController animator)
    {
        if (VisualControl == null)
        {
            VisualControl = GetComponent<Animator>();
            if (VisualControl == null)
                return; 
        }

        VisualControl.runtimeAnimatorController = animator;
        VisualControl.SetInteger("Direction", 1);
        VisualControl.SetBool("Walk", true);
    }
}
