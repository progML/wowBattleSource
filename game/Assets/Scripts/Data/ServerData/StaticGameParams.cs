using System;
using UnityEngine;

namespace Data.ServerData
{
  
    [Serializable]
    public class StaticGameParams
    {
        public void Set(GameData data)
        {
            maxHp = data.FIGHT_DEFAULT_HP;
            baseLen = data.BASE_SLOTS;
            maxShields = data.BASE_SHIELDS;
            maxTurns = data.FIGHT_MAX_HITS;
        }
        public int maxHp;
        public int baseLen;
        public int maxShields;
        public int maxTurns;
    }
}