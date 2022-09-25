using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameplay
{
    public class VictoryGameState : GameState
    {
        
        public override void StateStart()
        {
            uiWindowsOpener.ShowVictoryPanel(serverGameStateProvider.GetAttacker());
            uiWindowsOpener.VictoryPanel.OkButtonClicked.AddListener(OnOkButtonClicked);
        }

        public override void StateFinish()
        {
        }

        private void OnOkButtonClicked()
        {
            uiWindowsOpener.VictoryPanel.OkButtonClicked.RemoveListener(OnOkButtonClicked);
            sceneLoaderChannel.LoadMainMenu();
        }
    }
}