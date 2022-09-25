using System;
using Data.ServerData;
using Gameplay;
using Misc;
using ServerRest.ServerStatusInteractor;
using UnityEngine;

namespace ServerRest
{
    [Serializable]
    public class PlayerData
    {
        public int id;
        public string login;
        public int baseMask;
        public Sprite icon;
        public int currentHealth;
        public int attackCount;
    }

    [RequireComponent(typeof(DontDestroyOnLoad))]
    public class ServerGameStatus : MonoBehaviour
    {
        public static ServerGameStatus Instance
        {
            get
            {
                if (_instance == null) _instance = (ServerGameStatus) FindObjectOfType(typeof(ServerGameStatus));
                if (_instance == null)
                {
                    var obj = new GameObject("ServerGameStatus");
                    obj.AddComponent<DontDestroyOnLoad>();
                    _instance = new GameObject("ServerGameStatus").AddComponent<ServerGameStatus>();
                }
                return _instance;
            }
        }


        private static ServerGameStatus _instance;
        
        private void Awake()
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void Start()
        {
            ServerInteractor.Instance.GetGameData();
        }

        public bool playerDataInitialized = false;
        public int modifiedPlayerBase;
        public StaticGameParams staticGameParams;
        public PlayerData playerData;
        public PlayerData enemyData;
        public ServeGameState serveGameState = ServeGameState.Playing;
    }
}