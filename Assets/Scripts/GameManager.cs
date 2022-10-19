using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SnakeGame.GameLoop
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] float tickDuration = 1f;

        public static GameManager Instance { get; private set; }

        // events
        public event EventHandler OnTick;

        // variables
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
    }
}
