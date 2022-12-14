using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using SnakeGame.GridSpace;
using Grid = SnakeGame.GridSpace.Grid;

namespace SnakeGame.GameLoop
{
    public class GridPosEventArgs : EventArgs
    {
        public GridPosition Data { get; set; }
        public GridPosEventArgs(GridPosition data)
        {
            Data = data;
        }
    }

    public class GameManager : MonoBehaviour
    {
        [SerializeField] float tickStartingDuration = 1f;
        [SerializeField] float tickDecreasePercentageOnEat = 20f;
        [SerializeField] float tickMinDuration = 0.1f;



        public static GameManager Instance { get; private set; }

        // events
        public event EventHandler OnTick;
        public event EventHandler<GridPosEventArgs> OnEat;

        // variables
        public float tickDuration;
        float tickTimer;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("There's more than one GameManager!");
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        void Start()
        {
            tickDuration = tickStartingDuration;
            tickTimer = tickDuration;
        }

        void Update()
        {
            tickTimer -= Time.deltaTime;
            if (tickTimer <= 0)
            {
                OnTick? .Invoke(this, EventArgs.Empty);
                tickTimer = tickDuration;
            }
        }

        public void Eat(GridPosition gridPos)
        {
            if (tickDecreasePercentageOnEat != 0)
            {
                float tentativeTick = tickDuration - (tickDuration * (tickDecreasePercentageOnEat / 100));
                tickDuration = Mathf.Max(tentativeTick, tickMinDuration);
            }
            OnEat ? .Invoke(this, new GridPosEventArgs(gridPos));
        }
    }
}
