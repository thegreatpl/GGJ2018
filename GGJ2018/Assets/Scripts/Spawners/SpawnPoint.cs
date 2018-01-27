using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SpawnPoint
{
    /// <summary>
    /// Location of this spawn location. 
    /// </summary>
    public Vector3Int SpawnLocation; 


    public virtual void Spawn(GameObject gameObject)
    {
        //do nothing. Cannot instantiate, and no door animation. Just appear. 
    }

    public virtual void Despawn (GameObject gameObject)
    {
        //do nothing. 
        
    }
}

