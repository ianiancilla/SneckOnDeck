using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using SnakeGame.GameLoop;

using SnakeGame.GridSpace;
using Grid = SnakeGame.GridSpace.Grid;

namespace SnakeGame.Actors
{
    public abstract class ItemSpawner : MonoBehaviour
    {
        [SerializeField] GameObject itemPrefab;
        [SerializeField] int numConcurrentItems = 2;
        
        // variables
        protected List <TilePiece> items = new List<TilePiece>();

        // cache
        protected Snake snake;

        protected void Start()
        {
            // cache
            snake = FindObjectOfType<Snake>();

            // registering to events
            GameManager.Instance.OnTick += GameManager_OnTick;
        }

        protected void GameManager_OnTick(object sender, EventArgs e)
        {
            if (items.Count < numConcurrentItems)
            {
                TilePiece newItemTilePiece = SpawnItemAtValidPos();
                
                if (newItemTilePiece != null)
                { 
                    items.Add(newItemTilePiece);
                }
            }
        }

        protected TilePiece SpawnItem(GridPosition gridPosition)
        {
            Vector3 spawnPos = Grid.GridPosToWorldPos(gridPosition);
            GameObject itemGO = Instantiate(itemPrefab,
                                             spawnPos,
                                             Quaternion.identity,
                                             this.transform);

            return new TilePiece(itemGO.transform);
        }

        protected TilePiece SpawnItemAtValidPos()
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
                return SpawnItem(spawnPos);
            }
            else return null;
        }

        public bool IsCollidingWithItem(GridPosition gridPos)
        {
            foreach (TilePiece item in items)
            {
                if (item.IsColliding(gridPos)) return true;
            }

            return false;
        }
        public bool IsCollidingWithItem(TilePiece tile)
        {
            foreach (TilePiece item in items)
            {
                if (item.IsColliding(tile)) return true;
            }

            return false;
        }
        
        protected void DestroyItemAtPos(GridPosition gridPos)
        {
            foreach (TilePiece item in items)
            {
                if (item.IsColliding(gridPos))
                {
                    Destroy(item.GetTransform().gameObject);
                    items.Remove(item);
                    return;
                }
            }
        }

        public abstract void HandleItemCollidingWithSnake (TilePiece tile);
    }
}

