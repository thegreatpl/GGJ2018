using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class TargetCheck
{
    /// <summary>
    /// The connected gameobject. 
    /// </summary>
    public GameObject GameObject;

    /// <summary>
    /// The connected EntityOwnership. 
    /// </summary>
    public EntityOwnership EntityOwnership; 


    public TargetCheck(GameObject gameObject)
    {
        GameObject = gameObject;
        EntityOwnership = GameObject.GetComponent<EntityOwnership>(); 
    }
}

