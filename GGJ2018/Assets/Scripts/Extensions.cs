using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public static class Extensions
{
    /// <summary>
    /// Get a random element from a list. Because I keep doing it. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static T RandomElement<T>(this IEnumerable<T> list)
    {
        return list.ElementAt(UnityEngine.Random.Range(0, list.Count())); 
    }

    public static Vector3Int XAdd(this Vector3Int v, int X)
    {
        return new Vector3Int(v.x + X, v.y, v.z);
    }
    public static Vector3Int YAdd(this Vector3Int v, int Y)
    {
        return new Vector3Int(v.x, v.y + Y, v.z);
    }


    public static Vector3Int Direction(this Vector3Int v, Direction direction)
    {
        switch(direction)
        {
            case global::Direction.None:
                return v;
            case global::Direction.North:
                return v.YAdd(1);
            case global::Direction.South:
                return v.YAdd(-1);

            case global::Direction.East:
                return v.XAdd(1);

            case global::Direction.West:
                return v.XAdd(-1);

            default:
                throw new Exception("Someone fucked up with an unknown direction"); 
        }
    }
}

