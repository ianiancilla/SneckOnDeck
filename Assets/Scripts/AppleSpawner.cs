using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using SnakeGame.GameLoop;

using SnakeGame.GridSpace;
using Grid = SnakeGame.GridSpace.Grid;

namespace SnakeGame.Actors
{
    public class AppleSpawner : ItemSpawner
    {

        private void Start()
        {
            base.Start();

            GameManager.Instance.OnEat += GameManager_OnEat;
        }

        private void GameManager_OnEat(object sender, GridPosEventArgs args)
        {
            DestroyItemAtPos(args.Data);
        }

        public override void HandleItemCollidingWithSnake (TilePiece tile)
        {
            GameManager.Instance.Eat(tile.GetGridPosition());
        }
    }
}

