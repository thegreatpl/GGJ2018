using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianAI : MonoBehaviour {

    public static MapGenerator MapGenerator;

    EntityMovement Movement;

    Direction PreviousDirection = Direction.None;

    Vector3Int TargetTile = Vector3Int.zero;


    Vector3Int CurrentTile;

    /// <summary>
    /// Chance to chose a target to more to, as opposed to just wandering randomly. 
    /// </summary>
    public float MoveChance = 0.05f;


    public float ChangeDirectionChance = 0.2f; 

    // Use this for initialization
    void Start () {
        Movement = GetComponent<EntityMovement>(); 
	}
	
	// Update is called once per frame
	void Update () {
        CurrentTile = MapGenerator.Base.WorldToCell(transform.position);

        if (TargetTile == Vector3Int.zero)
        {
            if (Random.value < MoveChance)
            {
                SetRandomTarget();
                return; 
            }

            Wander();
            return; 
        }

        if (CurrentTile == TargetTile)
        {
            TargetTile = Vector3Int.zero;
            PreviousDirection = Movement.Direction; 
            Movement.Direction = Direction.None; 
        }
        else
        {
            MoveToTarget(); 
        }
	}

    /// <summary>
    /// Attempts to move to the target. 
    /// </summary>
    public void MoveToTarget()
    {
        if (Random.value <= 0.5)
        {
            if (TryMoveX())
                return;
            if (TryMoveY())
                return; 
        }
        else
        {
            if (TryMoveY())
                return;
            if (TryMoveX())
                return;
        }

        

        //Cannot move closer to goal? Try moving in another direction.

        //Start with the current direction if there is one. 
        if (Movement.Direction != Direction.None && Passable(CurrentTile, Movement.Direction))
        {
            return; //keep moving in that direction for now. 
        }

        // try to move in a random direction? 
        Wander(); 

        

        Movement.Direction = Direction.None; 
    }

    /// <summary>
    /// Tries to move in the x direction. Returns true if it can. 
    /// </summary>
    /// <returns></returns>
    bool TryMoveX()
    {
        if (CurrentTile.x < TargetTile.x && Movement.Direction != Direction.West)
        {
            //move east. 
            if (Passable(CurrentTile.XAdd(1)))
            {
                Movement.Direction = Direction.East;
                return true;
            }
        }
        else if (CurrentTile.x > TargetTile.x && Movement.Direction != Direction.East)
        {
            //move west. 
            if (Passable(CurrentTile.XAdd(-1)))
            {
                Movement.Direction = Direction.West;
                return true;
            }
        }

        return false; 
    }
    /// <summary>
    /// Tries to move in the Y direction. Returns true if it is. 
    /// </summary>
    /// <returns></returns>
    bool TryMoveY()
    {
        if (CurrentTile.y < TargetTile.y && Movement.Direction != Direction.South)
        {
            // move north.
            if (Passable(CurrentTile.YAdd(1)))
            {
                Movement.Direction = Direction.North;
                return true;
            }
        }
        else if (CurrentTile.y > TargetTile.y && Movement.Direction != Direction.North)
        {
            // move south. 
            if (Passable(CurrentTile.YAdd(-1)))
            {
                Movement.Direction = Direction.South;
                return true;
            }
        }

        return false; 
    }

    /// <summary>
    /// Wanders in a random direction. 
    /// </summary>
    void Wander()
    {
        if (Random.value > ChangeDirectionChance )
        {
            if (Movement.Direction != Direction.None && Passable(CurrentTile, Movement.Direction))
            {
                //keep moving in that direction. 
                return; 
            }
        }


        Direction direct = (Direction)Random.Range(1, 5);

        if (Passable(CurrentTile, direct))
        {
            Movement.Direction = direct;
            return;
        }

        Movement.Direction = Direction.None; 
    }

    void SetRandomTarget()
    {
        for (int idx = 0; idx < 5; idx++)
        {
            var ranPos = new Vector3Int(Random.Range(0, MapGenerator.XSize), Random.Range(0, MapGenerator.YSize), 0);
            if (Passable(ranPos))
            {
                TargetTile = ranPos;
                return;
            }
        }
    }

    /// <summary>
    /// Whether or not a tile is passable. 
    /// </summary>
    /// <param name="tilePos"></param>
    /// <returns></returns>
    bool Passable(Vector3Int tilePos)
    {
        return !MapGenerator.Walls.HasTile(tilePos); 
    }

    /// <summary>
    /// Whether or not a tile in a given direction is passable. 
    /// </summary>
    /// <param name="tilePos"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    bool Passable(Vector3Int tilePos, Direction direction)
    {
        return Passable(tilePos.Direction(direction));
    }
}
