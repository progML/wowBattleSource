using ServerRest;
using UnityEngine;

namespace Gameplay
{
    public class FrontStateTester: MonoBehaviour
    {

        public void SetWinState()
        {
            ServerGameStatus.Instance.serveGameState = ServeGameState.Victory;
        }
        
        public void SetLoseState()
        {
            ServerGameStatus.Instance.serveGameState = ServeGameState.Defeat;
        }
        
    }
}