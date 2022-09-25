using System;
using UnityEngine;

namespace Gameplay
{
    public class GameStateMachine : MonoBehaviour
    {
        [Header("Links")]
        [SerializeField] private GameState[] states;
        [SerializeField] private GameState beginState;
        [Header("Monitoring")]
        [SerializeField] private GameState currentState;


        public void ChangeState(Type newGameStateType)
        {
            if (currentState) currentState.StateFinish();
            var found = false;
            foreach (var gameState in states)
            {
                if (gameState.GetType() == newGameStateType)
                {
                    currentState = gameState;
                    found = true;
                    break;
                }
            }

            if (!found) throw new Exception("No such state");

            currentState.StateStart();
        }

        private void Start()
        {
            ChangeState(beginState.GetType());
        }
    }
}