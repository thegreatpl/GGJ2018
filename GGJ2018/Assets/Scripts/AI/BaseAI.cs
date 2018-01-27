using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAI : MonoBehaviour {

    public static MapGenerator MapGenerator;

    protected EntityMovement Movement;

    protected Direction PreviousDirection = Direction.None;

    protected Vector3Int TargetTile = Vector3Int.zero;

    /// <summary>
    /// Current tile that this is on. 
    /// </summary>
    protected Vector3Int CurrentTile;

    /// <summary>
    /// Chance when wandering that it will change direction. 
    /// </summary>
    public float ChangeDirectionChance = 0.2f;


    // Use this for initialization
    void Start () {
        Movement = GetComponent<EntityMovement>();
    }

    // Update is called once per frame
    void Update () {
        BaseUpdate(); 
	}

    protected virtual void BaseUpdate()
    {
        CurrentTile = MapGenerator.Base.WorldToCell(transform.position);

        if (CurrentTile == TargetTile)
        {
            TargetTile = Vector3Int.zero;
            PreviousDirection = Movement.Direction;
            Movement.Direction = Direction.None;
        }
        else
        {
            MoveToTarget(TargetTile);
        }
    }

    /// <summary>
    /// Attempts to move to the target. 
    /// </summary>
    protected void MoveToTarget(Vector3Int target)
    {
        if (Random.value <= 0.5)
        {
            if (TryMoveX(target))
                return;
            if (TryMoveY(target))
                return;
        }
        else
        {
            if (TryMoveY(target))
                return;
            if (TryMoveX(target))
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
    protected bool TryMoveX(Vector3Int target)
    {
        if (CurrentTile.x < target.x && Movement.Direction != Direction.West)
        {
            //move east. 
            if (Passable(CurrentTile.XAdd(1)))
            {
                Movement.Direction = Direction.East;
                return true;
            }
        }
        else if (CurrentTile.x > target.x && Movement.Direction != Direction.East)
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
    protected bool TryMoveY(Vector3Int target)
    {
        if (CurrentTile.y < target.y && Movement.Direction != Direction.South)
        {
            // move north.
            if (Passable(CurrentTile.YAdd(1)))
            {
                Movement.Direction = Direction.North;
                return true;
            }
        }
        else if (CurrentTile.y > target.y && Movement.Direction != Direction.North)
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
    protected void Wander()
    {
        if (Random.value > ChangeDirectionChance)
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

    /// <summary>
    /// Sets a random target to move to. 
    /// </summary>
    protected void SetRandomTarget()
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
    /// Moves in the direction given. 
    /// </summary>
    /// <param name="direction"></param>
    protected void MoveInDirection(Vector3 direction)
    {
        float xMovement, yMovement;
        Direction xDirect, yDirect;
        if (direction.x < 0)
        {
            xMovement = direction.x * -1;
            xDirect = Direction.West;
        }
        else
        {
            xMovement = direction.x;
            xDirect = Direction.East;
        }

        if (direction.y < 0)
        {
            yMovement = direction.y * -1;
            yDirect = Direction.South;
        }
        else
        {
            yMovement = direction.y;
            yDirect = Direction.North;
        }

        if (xMovement > yMovement && Passable(CurrentTile, xDirect))
        { 
            Movement.Direction = xDirect;
            return; 
        }
        if (yMovement > xMovement && Passable(CurrentTile, yDirect))
        {
            Movement.Direction = yDirect;
            return; 
        }
        if (yMovement == xMovement)
        {
            if (Random.value < 0.5f)
                Movement.Direction = xDirect;
            else
                Movement.Direction = yDirect;

            return; 
        }

        Wander(); 

    }

        /// <summary>
        /// Whether or not a tile is passable. 
        /// </summary>
        /// <param name="tilePos"></param>
        /// <returns></returns>
        protected bool Passable(Vector3Int tilePos)
    {
        return !MapGenerator.Walls.HasTile(tilePos);
    }

    /// <summary>
    /// Whether or not a tile in a given direction is passable. 
    /// </summary>
    /// <param name="tilePos"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    protected bool Passable(Vector3Int tilePos, Direction direction)
    {
        return Passable(tilePos.Direction(direction));
    }
}
