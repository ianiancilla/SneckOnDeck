using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnakeGame.GridSpace
{
    public enum Direction { North, East, South, West };

    public static class Grid
    {

        const int SIZE_X = 30;
        const int SIZE_Y = 18;

        public static bool IsValidGridPos(GridPosition gridPosition)
        {
            if (Mathf.Abs(gridPosition.GetX()) > ((SIZE_X / 2) - 1)){ return false; }
            if (Mathf.Abs(gridPosition.GetY()) > ((SIZE_Y / 2) - 1)){ return false; }

            return true;
        }

        public static GridPosition GetRandomGridPosition()
        {
            int x = Random.Range(-(SIZE_X/2-1), (SIZE_X/2));
            int y = Random.Range(-(SIZE_Y/2-1), (SIZE_Y/2));

            return new GridPosition(x,y);
        }

        public static GridPosition GetNeighbour(GridPosition gridPosition, Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return new GridPosition(gridPosition.GetX(),
                                            gridPosition.GetY()+1);
                case Direction.South:
                    return new GridPosition(gridPosition.GetX(),
                                            gridPosition.GetY()-1);
                case Direction.West:
                    return new GridPosition(gridPosition.GetX()-1,
                                            gridPosition.GetY());
                case Direction.East:
                    return new GridPosition(gridPosition.GetX()+1,
                                            gridPosition.GetY());
                default:
                    return new GridPosition(0,0);
            }
        }

        public static GridPosition WorldPosToGridPos(Vector3 worldPos)
        {
            return new GridPosition((int)(worldPos.x), (int)(worldPos.y));
        }

        public static Vector3 GridPosToWorldPos (GridPosition gridPosition)
        {
            return new Vector3(gridPosition.GetX(),
                               gridPosition.GetY(),
                               0);
        }

    }
}

