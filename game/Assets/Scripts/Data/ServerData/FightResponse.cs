using System;

namespace Data.ServerData
{
    [Serializable]
    public class FightResponse: BasicResponse
    {
        public FightData data;
    }
}