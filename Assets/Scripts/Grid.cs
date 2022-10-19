using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snake.Grid 
{
    public class Grid : MonoBehaviour
    {
        const int SIZE_X = 1280;
        const int SIZE_Y = 800;
        public enum Direction { North, East, South, West };

        private bool IsValidGridPos(GridPosition gridPosition)
        {
            if (Mathf.Abs(gridPosition.GetX()) > ((SIZE_X / 2) - 1)){ return false; }
            if (Mathf.Abs(gridPosition.GetY()) > ((SIZE_Y / 2) - 1)){ return false; }

            return true;
        }

        private GridPosition GetNeighbour(GridPosition gridPosition, Direction direction)
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

    }
}

