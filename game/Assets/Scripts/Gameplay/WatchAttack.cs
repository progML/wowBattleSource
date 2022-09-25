using Presenter;
using UnityEngine;

namespace Gameplay
{
    public class WatchAttack : GameState
    {
        [SerializeField] private AttackViewFinishedChannel attackViewFinishedChannel;

        public override void StateStart()
        {
            attackViewFinishedChannel.attackViewFinished.AddListener(OnAttackFinished);
        }

        public override void StateFinish()
        {
            attackViewFinishedChannel.attackViewFinished.RemoveListener(OnAttackFinished);
        }

        private void OnAttackFinished(AttackViewFinishedEventArgs arg0)
        {
            gameStateMachine.ChangeState(typeof(CheckServerState));
        }
    }
}