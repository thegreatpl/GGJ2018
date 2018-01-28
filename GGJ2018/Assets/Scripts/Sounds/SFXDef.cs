using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class SFXDef
{
    public string Name;

    public SFXType Type;

    public AudioClip AudioClip; 
}


public enum SFXType
{
    Zombie, 
    Hit, 
    Transform, 
    NONE
}
