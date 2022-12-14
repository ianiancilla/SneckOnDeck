using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using SnakeGame.GameLoop;

using SnakeGame.GridSpace;
using Grid = SnakeGame.GridSpace.Grid;

namespace SnakeGame.Actors
{
    public class AppleSpawner : MonoBehaviour
    {
        [SerializeField] GameObject applePrefab;
        [SerializeField] int numConcurrentApples = 2;
        
        // variables
        private List <TilePiece> apples = new List<TilePiece>();

        // cache
        private Snake snake;

        private void Start()
        {
            // cache
            snake = FindObjectOfType<Snake>();

            // registering to events
            GameManager.Instance.OnTick += GameManager_OnTick;
            GameManager.Instance.OnEat += GameManager_OnEat;

        }

        private void GameManager_OnTick(object sender, EventArgs e)
        {
            if (apples.Count < numConcurrentApples)
            {
                TilePiece newApple = SpawnAppleAtValidPos();
                
                if (newApple != null)
                { 
                    apples.Add(newApple);
                }
            }
        }

        private void GameManager_OnEat(object sender, GridPosEventArgs args)
        {
            DestroyAppleAtPos(args.Data);
        }


        private TilePiece SpawnApple(GridPosition gridPosition)
        {
            Vector3 spawnPos = Grid.GridPosToWorldPos(gridPosition);
            GameObject appleGO = Instantiate(applePrefab,
                                             spawnPos,
                                             Quaternion.identity,
                                             this.transform);

            return new TilePiece(appleGO.transform);
        }

        private TilePiece SpawnAppleAtValidPos()
        {
            GridPosition spawnPos = Grid.GetRandomGridPosition();
            int remainingAttempts = 200;    // number of attempts to find a random free spot
            while (snake.IsGridPosOnSnake(spawnPos) && remainingAttempts > 0)
            {
                remainingAttempts--;
                spawnPos = Grid.GetRandomGridPosition();
            }

            if (!snake.IsGridPosOnSnake(spawnPos))
            {
                return SpawnApple(spawnPos);
            }
            else return null;
        }

        public bool IsCollidingWithApple(GridPosition gridPos)
        {
            foreach (TilePiece apple in apples)
            {
                if (apple.IsColliding(gridPos)) return true;
            }

            return false;
        }
        public bool IsCollidingWithApple(TilePiece tile)
        {
            foreach (TilePiece apple in apples)
            {
                if (apple.IsColliding(tile)) return true;
            }

            return false;
        }
        
        private void DestroyAppleAtPos(GridPosition gridPos)
        {
            foreach (TilePiece apple in apples)
            {
                if (apple.IsColliding(gridPos))
                {
                    Destroy(apple.GetTransform().gameObject);
                    apples.Remove(apple);
                    return;
                }
            }
        }
    }
}

