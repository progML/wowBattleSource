using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameplay
{
    public class DefeatGameState : GameState
    {
        public override void StateStart()
        {
            uiWindowsOpener.ShowDefeatPanel(serverGameStateProvider.GetDefender());
            uiWindowsOpener.DefeatPanel.OkButtonClicked.AddListener(OnOkButtonClicked);
        }

        private void OnOkButtonClicked()
        {
            uiWindowsOpener.DefeatPanel.OkButtonClicked.RemoveListener(OnOkButtonClicked);
            sceneLoaderChannel.LoadMainMenu();
        }

        public override void StateFinish()
        {
        }
    }
}