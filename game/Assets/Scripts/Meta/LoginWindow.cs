using ServerRest.ServerStatusInteractor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Meta
{
    public class LoginWindow : MonoBehaviour
    {
        [SerializeField] private TMP_InputField tmpInputField;
        [SerializeField] private SceneLoaderChannel sceneLoaderChannel;
        [SerializeField] private Image image;


        public void LoginButtonClicked()
        {
            var login = tmpInputField.text;
            if (login.Length <= 0) return;
            ServerInteractor.Instance.Authorize(login, null, () =>
            {
                ServerInteractor.Instance.UploadUserPhoto();
                
                sceneLoaderChannel.LoadMainMenu();
                
            });
        }

        public void LoadImageButtonClicked()
        {
            ServerInteractor.Instance.CallPhotoUploadDialog();
        }
    }
}