using Presenter;
using UnityEngine;

namespace Gameplay
{
    public class SelectingLineState : GameState
    {
        [SerializeField] private LineSelector lineSelector;
        [SerializeField] private AttackViewFinishedChannel attackViewFinishedChannel;

        public override void StateStart()
        {
            lineSelector.lineSelected.AddListener(OnLineSelected);
            lineSelector.ShowSelectArrow = true;
        }

        public override void StateFinish()
        {
            lineSelector.ShowSelectArrow = false;
        }

        private void OnLineSelected(LineSelectedEventArgs arg0)
        {
            lineSelector.lineSelected.RemoveListener(OnLineSelected);
            var attackSuccess = serverGameStateProvider.TryAttackLine(arg0.LineNum,
                type => { lineSelector.lineSelected.AddListener(OnLineSelected); }, (bool attackSuccess) =>
                {
                    gameStateMachine.ChangeState(typeof(WatchAttack));
                    gameStatePresenter.AttackLine(arg0.AttackLine, attackSuccess);
                    uiWindowsOpener.FightUi.AttackCountReduce();


                    attackViewFinishedChannel.attackViewFinished.AddListener(ReduceHealth);

                    void ReduceHealth(AttackViewFinishedEventArgs args)
                    {
                        attackViewFinishedChannel.attackViewFinished.RemoveListener(ReduceHealth);
                        if (attackSuccess) uiWindowsOpener.FightUi.DefenderHealthReduce();
                    }
                });
        }
    }
}