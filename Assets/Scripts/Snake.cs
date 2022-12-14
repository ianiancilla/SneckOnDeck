using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

using SnakeGame.GameLoop;

using SnakeGame.GridSpace;
using Grid = SnakeGame.GridSpace.Grid;

using SnakeGame.PlayerInput;
using Input = SnakeGame.PlayerInput.Input;

namespace SnakeGame.Actors
{
    public class Snake : MonoBehaviour
    {
        [SerializeField] GameObject snakeTilePrefab;
        [SerializeField] Direction startingDirection = Direction.North;
        [SerializeField] int startingLength = 1;

        // variables
        TilePiece head;

        Direction currentDirection;  //the direction the snake is actually going
        Direction latestDirectionInput;  //mutliple inputs can be recorded between ticks
       
        int currentLength;
        bool grewLengthThisTick = false;

        List<TilePiece> snakeTiles = new List<TilePiece>();

        // cache
        ItemSpawner[] itemSpawners;

        void Awake()
        {
            currentDirection = startingDirection;
            currentLength = startingLength;

            // set head
            GameObject headGO = Instantiate(snakeTilePrefab, Vector3.zero, Quaternion.identity, this.transform);
            head = new TilePiece(headGO.transform);
            snakeTiles.Add(head);
        }

        private void Start()
        {
            // registering to events
            GameManager.Instance.OnTick += GameManager_OnTick;
            GameManager.Instance.OnEat += GameManager_OnEat;

            // cache
            itemSpawners = FindObjectsOfType<ItemSpawner>();
        }

        void Update()
        {
            HandleMovementInput();
        }

        private void HandleMovementInput()
        {
            Vector2 movementInput = Input.Instance.GetMovementDirection();
            switch (movementInput)
            {
                case Vector2 v when v.Equals(Vector2.up) 
                && currentDirection != Direction.South:
                    latestDirectionInput = Direction.North;
                    break;
                case Vector2 v when v.Equals(Vector2.down)
                && currentDirection != Direction.North:
                    latestDirectionInput = Direction.South;
                    break;
                case Vector2 v when v.Equals(Vector2.left)
                && currentDirection != Direction.East:
                    latestDirectionInput = Direction.West;
                    break;
                case Vector2 v when v.Equals(Vector2.right)
                && currentDirection != Direction.West:
                    latestDirectionInput = Direction.East;
                    break;
                default:
                    break;
            }
        }

        private void GameManager_OnTick(object sender, EventArgs e)
        {
            ApplyLatestInput();
            Move();
            ItemCollisionCheck();
        }

        private void ApplyLatestInput()
        {
            currentDirection = latestDirectionInput;
        }

        private void Move()
        {
            // find new head position
            GridPosition newHeadPos = Grid.GetNeighbour(head.GetGridPosition(), currentDirection);

            // if collided with screen border
            if (!Grid.IsValidGridPos(newHeadPos))
            {
                GameManager.Instance.GameOver();
                return;
            }

            // add a tile on top of last one if one should be added
            if (snakeTiles.Count < currentLength)
            {
                Transform newTileTransform = Instantiate(snakeTilePrefab, 
                                                         Vector3.zero, 
                                                         Quaternion.identity,
                                                         this.transform).transform;

                TilePiece newTile = new TilePiece(newTileTransform);
                newTile.MoveTileToGridPos(snakeTiles.Last().GetGridPosition());

                snakeTiles.Add(newTile);
                grewLengthThisTick = true;
            }

            // update tail positions
            int lastTileToUpdate = snakeTiles.Count - 1;
            if (grewLengthThisTick) // the last one does not move if just created
            {
                lastTileToUpdate --;
                grewLengthThisTick = false;
            }

            // from the last one, each follows the position of the one previous, before that is updated
            for (int i = lastTileToUpdate; i > 0; i--)
            {
                snakeTiles[i].MoveTileToGridPos(snakeTiles[i-1].GetGridPosition());
            }

            // check for self-collision
            foreach(TilePiece tile in snakeTiles)
            {
                if (tile.IsColliding(newHeadPos))
                {
                    GameManager.Instance.GameOver();
                }
            }

            // update head position
            head.MoveTileToGridPos(newHeadPos);
        }

        private void ItemCollisionCheck()
        {
            foreach (ItemSpawner spawner in itemSpawners)
            {
                foreach (TilePiece tile in snakeTiles)
                {
                    if (spawner.IsCollidingWithItem(tile))
                    {
                        GameManager.Instance.Eat(tile.GetGridPosition());
                    }
                }
            }
        }

        private void GameManager_OnEat(object sender, GridPosEventArgs args)
        {
            currentLength++;
        }

        private static void Die()
        {
            Debug.Log("DEATH");
        }

        public bool IsGridPosOnSnake(GridPosition gridPos)
        {
            foreach (TilePiece tile in snakeTiles)
            {
                if (tile.IsColliding(gridPos)) return true;
            }

            return false;
        }
    }

}
