using System;

namespace Data.ServerData
{
    [Serializable]
    public class AuthData: BasicResponse
    {
        public AuthDataContent data;
    }
    [Serializable]
    public class AuthDataContent
    {
        public string token;
        public string user_state;
    }
}