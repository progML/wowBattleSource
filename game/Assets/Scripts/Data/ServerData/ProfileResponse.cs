using System;

namespace Data.ServerData
{
    [Serializable]
    public class ProfileResponse: BasicResponse
    {
        public ProfileData data;
    }
}