using System;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
    public class StartGameClip : GameState
    {
        public override void StateStart()
        {
            uiWindowsOpener.StartGamePanel.StartButtonClicked.AddListener(OnStartButtonClicked);
            uiWindowsOpener.ShowStartPanel(
                serverGameStateProvider.GetAttacker(),
                serverGameStateProvider.GetDefender());
        }

        public override void StateFinish()
        {
            var lineCount = serverGameStateProvider.GetCurrentGameLineCount();
            gameStatePresenter.SpawnField(lineCount);
            uiWindowsOpener.ShowFightUI(
                serverGameStateProvider.GetAttacker(),
                serverGameStateProvider.GetDefender(),
                serverGameStateProvider.GetMaxAttackCount(),
                serverGameStateProvider.GetRemainingAttackCount(),
                serverGameStateProvider.GetCurrentHealth(),
                serverGameStateProvider.GetMaxHealth()
            );
        }

        private void OnStartButtonClicked()
        {
            uiWindowsOpener.StartGamePanel.StartButtonClicked.RemoveListener(OnStartButtonClicked);
            gameStateMachine.ChangeState(typeof(CheckServerState));
        }

        private IEnumerator DelayedAction(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action.Invoke();
        }
    }
}