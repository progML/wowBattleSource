using Meta;
using UI;
using UnityEngine;

namespace Gameplay
{
    public abstract class GameState : MonoBehaviour
    {
        [SerializeField] protected ServerGameStateProvider serverGameStateProvider;
        [SerializeField] protected GameStateMachine gameStateMachine;
        [SerializeField] protected GameStatePresenter gameStatePresenter;
        [SerializeField] protected UiWindowsOpener uiWindowsOpener;
        [SerializeField] protected SceneLoaderChannel sceneLoaderChannel;

        public abstract void StateStart();
        public abstract void StateFinish();
    }
}