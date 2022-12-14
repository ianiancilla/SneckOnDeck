using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using SnakeGame.GameLoop;

using SnakeGame.GridSpace;
using Grid = SnakeGame.GridSpace.Grid;

namespace SnakeGame.Actors
{
    public class HazardSpawner : ItemSpawner
    {
        public override void HandleItemCollidingWithSnake (TilePiece tile)
        {
            GameManager.Instance.GameOver();
        }
    }
}

