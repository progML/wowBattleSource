using System;
using ServerRest;
using ServerRest.ServerStatusInteractor;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Meta
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private SceneLoaderChannel sceneLoaderChannel;
        [SerializeField] private UserProfilePanel userProfilePanel;
        [SerializeField] private DefaultUsersProfileConfig defaultUsersProfile;


        public void PlayButtonClick()
        {
            ServerInteractor.Instance.GetFight(null, () =>
            {
                ServerInteractor.Instance.GetProfile(ServerGameStatus.Instance.enemyData.id, null, () =>
                {
                    ServerInteractor.Instance.GetUserPhoto(ServerGameStatus.Instance.enemyData.id, type =>
                    {
                        sceneLoaderChannel.LoadCoreGameplay();
                    }, () =>
                    {
                        sceneLoaderChannel.LoadCoreGameplay();
                    }, false);
                });
            });
        }

        public void MyBaseButtonClick()
        {
            sceneLoaderChannel.LoadMyBase();
        }

        public void ChangeUserIconClick()
        {
            ServerInteractor.Instance.UploadUserPhotoFull();
        }

        private void Start()
        {
            if (!ServerGameStatus.Instance.playerDataInitialized)
            {
                ServerGameStatus.Instance.playerDataInitialized = true;
                
                ServerGameStatus.Instance.playerData.login = defaultUsersProfile.Name;
                ServerGameStatus.Instance.playerData.icon = defaultUsersProfile.Icon;
            }

            if (ServerGameStatus.Instance.playerData.login == defaultUsersProfile.Name)
            {
                ServerInteractor.Instance.GetMyProfile(null, UpdateUserPanel);
            }
            if (ServerGameStatus.Instance.playerData.icon == defaultUsersProfile.Icon)
            {
                ServerInteractor.Instance.GetMyPhoto(null, UpdateUserPanel, false);
            }

            UpdateUserPanel();
            ServerInteractor.Instance.GetBase(null, UpdateUserPanel, false);
        }

        private void OnEnable()
        {
            ServerInteractor.Instance.ImageUploadedEvent.AddListener(OnImageUploaded);
        }

        private void OnImageUploaded(ImageUploadedArgs arg0)
        {
            UpdateUserPanel();
        }

        private void OnDisable()
        {
            ServerInteractor.Instance.ImageUploadedEvent.AddListener(OnImageUploaded);
        }

        private void UpdateUserPanel()
        {
            userProfilePanel.SetUp(new UserUIData(ServerGameStatus.Instance.playerData.icon, ServerGameStatus.Instance.playerData.login));

        }
    }
}