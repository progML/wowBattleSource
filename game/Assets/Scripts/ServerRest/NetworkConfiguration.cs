using UnityEngine;

namespace ServerRest
{
    [CreateAssetMenu(fileName = "networkConfiguration", menuName = "Configuration/Network Configuration", order = 0)]
    public class NetworkConfiguration : ScriptableObject
    {
        [Header("Server")]
        [SerializeField] private string serverHost;
        [SerializeField] private bool secureConnection;
        
        [Header("Routes")]
        
        [SerializeField] private string authRoute;
        [SerializeField] private string baseRoute;
        [SerializeField] private string fightRoute;
        [SerializeField] private string photoRoute;
        [SerializeField] private string profileRoute;
        [SerializeField] private string gameDataRoute;
        
        public string ConnectionPrefix => "http"+(secureConnection?"s":"")+"://" + serverHost;
        
        //Routes
        
        public string AuthRoute => ConnectionPrefix + authRoute;
        public string BaseRoute => ConnectionPrefix + baseRoute;
        public string FightRoute => ConnectionPrefix + fightRoute;
        public string PhotoRoute => ConnectionPrefix + photoRoute;
        public string ProfileRoute => ConnectionPrefix + profileRoute;
        public string GameDataRoute => ConnectionPrefix + gameDataRoute;
        public string MyProfileRoute => ProfileRoute + "/my";
        public string MyPhotoRoute => PhotoRoute + "/my";
    }
}