using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snake.Grid 
{
    public class GridPosition : MonoBehaviour
    {
        int x;
        int y;

        public GridPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int GetX() => x;
        public int GetY() => y;
    }
}
