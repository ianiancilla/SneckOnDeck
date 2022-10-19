using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using SnakeGame.GameLoop;
using SnakeGame.GridSpace;
using Grid = SnakeGame.GridSpace.Grid;

namespace SnakeGame.Snake
{
    public class Snake : MonoBehaviour
    {
        [Header("Snake")]
        [SerializeField] GameObject snakeTilePrefab;

        [Space]
        [Header("Gameplay")]
        [SerializeField] Direction startingDirection = Direction.North;

        // variables
        Direction currentDirection;
        GameObject head;
        float tickTimer;

        void Awake()
        {
            currentDirection = startingDirection;

            head = Instantiate(snakeTilePrefab, Vector3.zero, Quaternion.identity);
        }

        private void Start()
        {
            // registering to events
            GameManager.Instance.OnTick += GameManager_OnTick;
        }

        private void GameManager_OnTick(object sender, EventArgs e)
        {
            Move();
        }

        private void Move()
        {
            GridPosition headPos = Grid.WorldPosToGridPos(head.transform.position);
            GridPosition newHeadPos = Grid.GetNeighbour(headPos, currentDirection);

            if (Grid.IsValidGridPos(newHeadPos))
            {
                head.transform.position = Grid.GridPosToWorldPos(newHeadPos);
            }

            else
            {
                Debug.Log("SBAM");
            }
        }
    }

}
