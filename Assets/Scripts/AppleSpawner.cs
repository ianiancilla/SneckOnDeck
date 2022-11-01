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
        private List <Transform> apples = new List<Transform>();

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
                Transform newApple = SpawnAppleInValidPos();
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


        private Transform SpawnApple(GridPosition gridPosition)
        {
            return Instantiate(applePrefab,
                               Grid.GridPosToWorldPos(gridPosition),
                               Quaternion.identity,
                               this.transform).transform;
        }

        private Transform SpawnAppleInValidPos()
        {
            GridPosition spawnPos = Grid.GetRandomGridPosition();
            int remainingAttempts = 200;
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

        public bool IsApplePos(GridPosition gridPos)
        {
            return Grid.IsPosOnList(gridPos, apples);
        }

        private void DestroyAppleAtPos(GridPosition gridPos)
        {
            foreach (Transform apple in apples)
            {
                if (Grid.WorldPosToGridPos(apple.position) == gridPos)
                {
                    Destroy(apple.gameObject);
                    apples.Remove(apple);
                    return;
                }
            }
        }

    }

}

