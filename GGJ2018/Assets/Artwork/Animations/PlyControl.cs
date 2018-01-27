using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlyControl : MonoBehaviour {

    public string Horizontal;
    public string Vertical;
    public string Fire;
    Animator VisualControl;  
	void Start () {
        VisualControl = gameObject.GetComponent<Animator>();

    
    }

    // Update is called once per frame
    void Update() {

        
        float moveX = Input.GetAxis(Horizontal);
        float moveY = Input.GetAxis(Vertical);
        
        if (moveX < 0)
        {
            VisualControl.SetInteger("Direction", 3);
            VisualControl.SetBool("Walk", true);
            transform.position += new Vector3(-1, 0, 0) * Time.deltaTime;
        }

        else if (moveX > 0)
        {
            VisualControl.SetInteger("Direction", 4);
            VisualControl.SetBool("Walk", true);
            transform.position += new Vector3(1, 0, 0) * Time.deltaTime;
        }
        
        else if (moveY < 0)
        {
            VisualControl.SetInteger("Direction", 2);
            VisualControl.SetBool("Walk", true);
            transform.position += new Vector3(0, -1, 0) * Time.deltaTime;
        }

        else if (moveY > 0)
        {
            VisualControl.SetInteger("Direction", 1);
            VisualControl.SetBool("Walk", true);
            transform.position += new Vector3(0, 1, 0) * Time.deltaTime;
        }

        else
        {
            VisualControl.SetBool("Walk", false);
        }


	}
}
