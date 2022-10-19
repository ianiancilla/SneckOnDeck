using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SnakeGame.PlayerInput
{
    public class Input : MonoBehaviour
    {

        public static Input Instance { get; private set; }

        // variables
        float tickTimer;

        // cache
        private PlayerInputActions inputActions;


        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("There's more than one Input component!");
                Destroy(gameObject);
                return;
            }
            Instance = this;


            inputActions = new PlayerInputActions();
            inputActions.Game.Enable();
        }

        public Vector2 GetMovementDirection()
        {
            return inputActions.Game.Movement.ReadValue<Vector2>();
        }
    }

}

