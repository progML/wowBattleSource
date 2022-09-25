using System;
using UnityEngine;

namespace Data.ServerData
{
    [Serializable]
    public class GameData
    {
        public int BASE_SLOTS;
        public int BASE_SHIELDS;
        public int FIGHT_DEFAULT_HP;
        public int FIGHT_MAX_HITS;
    }
    [Serializable]
    public class GameConfigResponse : BasicResponse
    {
        public GameData data;
    }
}