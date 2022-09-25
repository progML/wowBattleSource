using System;

namespace Gameplay
{
    public class CheckServerState : GameState
    {
        public override void StateStart()
        {
            CheckState();
        }

        public override void StateFinish()
        {
        }


        private void CheckState()
        {
            switch (serverGameStateProvider.GetServerGameState())
            {
                case ServeGameState.Playing:
                    gameStateMachine.ChangeState(typeof(SelectingLineState));
                    break;
                case ServeGameState.Victory:
                    gameStateMachine.ChangeState(typeof(VictoryGameState));
                    break;
                case ServeGameState.Defeat:
                    gameStateMachine.ChangeState(typeof(DefeatGameState));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}