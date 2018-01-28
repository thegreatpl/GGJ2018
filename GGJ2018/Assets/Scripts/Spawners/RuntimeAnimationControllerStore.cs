using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class RuntimeAnimationControllerStore
{
    public string Name;

    public EntityType Type;

    public RuntimeAnimatorController AnimatorController;

    public int Faction; 

    public float Weight; 
}

