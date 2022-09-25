using Meta.BaseRedactor;
using ServerRest;
using ServerRest.ServerStatusInteractor;
using TMPro;
using UnityEngine;

namespace Meta
{
    public class PlayerBaseMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI shieldsCounter;
        [SerializeField] private SceneLoaderChannel sceneLoaderChannel;

        public readonly RespawnBaseFromStatusRequestEvent RespawnBaseFromStatusRequestEvent =
            new RespawnBaseFromStatusRequestEvent();

        public void UpdateShieldsRemaining(int settedCount)
        {
            shieldsCounter.text = (ServerGameStatus.Instance.staticGameParams.maxShields - settedCount).ToString();
        }

        public void ReturnToMainMenuClick()
        {
            sceneLoaderChannel.LoadMainMenu();
        }

        public void SendNewBaseClick()
        {
            ServerInteractor.Instance.PutBase(ServerGameStatus.Instance.modifiedPlayerBase, null, ()=>
            {
                RespawnBaseFromStatusRequestEvent.Invoke(new RespawnBaseFromStatusRequestArgs());
            });
        }
    }
}