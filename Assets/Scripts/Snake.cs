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
        Transform head;

        Direction currentDirection;  //the direction the snake is actually going
        Direction latestDirectionInput;  //mutliple inputs can be recorded between ticks
       
        int currentLength;
        bool grewLengthThisTick = false;

        List<Transform> snakeTileTransforms = new List<Transform>();

        // cache
        AppleSpawner appleSpawner;

        void Awake()
        {
            currentDirection = startingDirection;
            currentLength = startingLength;

            // set head
            head = Instantiate(snakeTilePrefab, Vector3.zero, Quaternion.identity).transform;
            snakeTileTransforms.Add(head);
        }

        private void Start()
        {
            // registering to events
            GameManager.Instance.OnTick += GameManager_OnTick;
            GameManager.Instance.OnEat += GameManager_OnEat;

            // cache
            appleSpawner = FindObjectOfType<AppleSpawner>();
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
            FeedCheck();
        }

        private void ApplyLatestInput()
        {
            currentDirection = latestDirectionInput;
        }

        private void Move()
        {
            // find new head position
            GridPosition headPos = Grid.WorldPosToGridPos(head.transform.position);
            GridPosition newHeadPos = Grid.GetNeighbour(headPos, currentDirection);

            // if collided with screen border
            if (!Grid.IsValidGridPos(newHeadPos))
            {
                Die();
                return;
            }

            // add a tile on top of last one if one should be added
            if (snakeTileTransforms.Count < currentLength)
            {
                Transform newTile = Instantiate(snakeTilePrefab, 
                                                snakeTileTransforms.Last().position, 
                                                Quaternion.identity).transform;

                snakeTileTransforms.Add(newTile);
                grewLengthThisTick = true;
            }

            // update tail positions
            int lastTileToUpdate = snakeTileTransforms.Count - 1;
            if (grewLengthThisTick) // the last one does not move if just created
            {
                lastTileToUpdate --;
                grewLengthThisTick = false;
            }

            // from the last one, each follows the position of the one previous, before that is updated
            for (int i = lastTileToUpdate; i > 0; i--)
            {
                snakeTileTransforms[i].position = snakeTileTransforms[i-1].position;
            }

            // check for self-collision
            foreach(Transform tile in snakeTileTransforms)
            {
                if (Grid.GridPosToWorldPos(newHeadPos) == tile.position)
                {
                    Die();
                }
            }

            // update head position
            head.position = Grid.GridPosToWorldPos(newHeadPos);
        }

        private void FeedCheck()
        {
            foreach (Transform tile in snakeTileTransforms)
            {
                GridPosition tilePos = Grid.WorldPosToGridPos(tile.position);
                if (appleSpawner.IsApplePos(tilePos))
                {
                    GameManager.Instance.Eat(tilePos);
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
            return Grid.IsPosOnList(gridPos, snakeTileTransforms);
        }
    }

}
