using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SnakeGame.GridSpace 
{
    public struct GridPosition : IEquatable<GridPosition>
    {
        int x;
        int y;

        public GridPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static bool operator ==(GridPosition a, GridPosition b)
        {
            return a.x == b.x && a.y == b.y;
        }
        public static bool operator !=(GridPosition a, GridPosition b)
        {
            return !(a == b);
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString() => $"({x}, {y})";


        public int GetX() => x;
        public int GetY() => y;

        public bool Equals(GridPosition other)
        {
            return this == other;
        }
    }

}
