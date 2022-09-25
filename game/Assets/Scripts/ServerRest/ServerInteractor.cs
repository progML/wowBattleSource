using System;
using System.Security.Cryptography;
using Data.ServerData;
using Misc;
using UnityEngine;

namespace ServerRest.ServerStatusInteractor
{
    [RequireComponent(typeof(RequestCore))]
    public class ServerInteractor : MonoBehaviour
    {
        [SerializeField] private NetworkConfiguration networkConfiguration;
        public NetworkConfiguration NetworkConfiguration => networkConfiguration;
        public readonly ImageUploadedEvent ImageUploadedEvent = new ImageUploadedEvent();
        private RequestCore requestCore;

        public static ServerInteractor Instance
        {
            get
            {
                if (_instance == null) _instance = (ServerInteractor) FindObjectOfType(typeof(ServerInteractor));
                if (_instance == null)
                {
                    var obj = new GameObject("ServerInteractor");
                    obj.AddComponent<DontDestroyOnLoad>();
                    _instance = new GameObject("ServerInteractor").AddComponent<ServerInteractor>();
                }

                return _instance;
            }
        }

        private static ServerInteractor _instance;

        private void Awake()
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }

            requestCore = GetComponent<RequestCore>();
        }

        public void OnImageUploadedJS()
        {
            Debug.Log("are you sure?");
            ImageUploadedEvent.Invoke(new ImageUploadedArgs());
        }
        public void Authorize(string login, Action<ErrorType> onError = null, Action onLoad = null)
        {
            requestCore.PostRequest<AuthData>(NetworkConfiguration.AuthRoute,
                JsonUtility.ToJson(new AuthPostData(login)), onError, data =>
                {
                    requestCore.SetUserKey(data.data.token);
                    ServerGameStatus.Instance.playerData.login = login;
                    onLoad?.Invoke();
                });
        }

        public void GetBase(Action<ErrorType> onError = null, Action onLoad = null, bool postError = true)
        {
            requestCore.GetRequest<BaseResponse>(NetworkConfiguration.BaseRoute, onError,
                data =>
                {
                    ServerGameStatus.Instance.playerData.baseMask = data.data.shields_bit_mask;
                    onLoad?.Invoke();
                }, postError);
        }

        public void PutBase(int baseShield, Action<ErrorType> onError = null, Action onLoad = null)
        {
            
            requestCore.PutRequest<BaseResponse>(NetworkConfiguration.BaseRoute, JsonUtility.ToJson(new  BasePostData(baseShield)), onError,
                data =>
                {
                    ServerGameStatus.Instance.playerData.baseMask = data.data.shields_bit_mask;
                    onLoad?.Invoke();
                });
        }

        public void GetFight(Action<ErrorType> onError = null, Action onLoad = null)
        {
            requestCore.GetRequest<FightResponse>(NetworkConfiguration.FightRoute, onError,
                response =>
                {
                    ServerGameStatus.Instance.enemyData.baseMask = response.data.base_data.shields_bit_mask;
                    ServerGameStatus.Instance.enemyData.id = response.data.user_id;
                    ServerGameStatus.Instance.serveGameState = response.data.NewState;
                    ServerGameStatus.Instance.playerData.attackCount = response.data.rest_hits;
                    ServerGameStatus.Instance.enemyData.currentHealth = response.data.rest_hp;
                    onLoad?.Invoke();
                });
        }

        public void PostFight(int hitPosition, Action<ErrorType> onError = null, Action<bool> onLoad = null)
        {
            requestCore.PostRequest<FightResponse>(NetworkConfiguration.FightRoute,
                JsonUtility.ToJson(new FightPostData(hitPosition)),
                onError, response =>
                {
                    ServerGameStatus.Instance.playerData.attackCount--;
                    ServerGameStatus.Instance.enemyData.baseMask = response.data.base_data.shields_bit_mask;
                    ServerGameStatus.Instance.serveGameState = response.data.NewState;
                    var lastHit = response.data.hit_data.Length-1;
                    if (response.data.hit_data[lastHit].hit_success) ServerGameStatus.Instance.enemyData.currentHealth--;
                    onLoad?.Invoke(response.data.hit_data[lastHit].hit_success);
                });
        }

        public void GetMyPhoto(Action<ErrorType> onError = null, Action onLoad = null, bool postError = true)
        {
            requestCore.TextureRequest(NetworkConfiguration.MyPhotoRoute, onError, texture =>
            {
                var sprite = Sprite.Create((Texture2D) texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                ServerGameStatus.Instance.playerData.icon = sprite;
                onLoad?.Invoke();
            }, postError);
        }

        public void CallPhotoUploadDialog()
        {
            requestCore.RequestUserImage();
        }

        public void UploadUserPhoto()
        {
            requestCore.UploadImage(NetworkConfiguration.MyPhotoRoute);
        }

        public void UploadUserPhotoFull()
        {
            requestCore.UploadImageFull();
        }
        public void GetUserPhoto(int userid, Action<ErrorType> onError = null, Action onLoad = null, bool postError = true)
        {
            var route = NetworkConfiguration.PhotoRoute + $"/{userid}";
            requestCore.TextureRequest(route, onError, texture =>
            {
                var sprite = Sprite.Create((Texture2D) texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                ServerGameStatus.Instance.enemyData.icon = sprite;
                onLoad?.Invoke();
            }, postError);
        }
        public void GetMyProfile(Action<ErrorType> onError = null, Action onLoad = null)
        {
            requestCore.GetRequest<ProfileResponse>(NetworkConfiguration.MyProfileRoute, onError,
                response =>
                {
                    ServerGameStatus.Instance.playerData.login = response.data.login;
                    onLoad?.Invoke();
                });
        }

        public void GetProfile(int userid, Action<ErrorType> onError = null, Action onLoad = null)
        {
            
            var route = NetworkConfiguration.ProfileRoute + $"/{userid}";
            requestCore.GetRequest<ProfileResponse>(route, onError,
                response =>
                {
                    ServerGameStatus.Instance.enemyData.login = response.data.login;
                    onLoad?.Invoke();
                });
        }

        public void GetGameData(Action<ErrorType> onError = null, Action onLoad = null)
        {
//            GameConfigResponse
            requestCore.GetRequest<GameConfigResponse>(networkConfiguration.GameDataRoute, onError,
                response =>
                {
                    ServerGameStatus.Instance.staticGameParams.Set(response.data);
                    onLoad?.Invoke();
                });
        }
    }
}