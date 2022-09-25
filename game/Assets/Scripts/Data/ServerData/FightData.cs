using System;
using Gameplay;
using UnityEngine;

namespace Data.ServerData
{
    [Serializable]
    public class FightData
    {
        public string fight_state;
        public int user_id;
        public HitData[] hit_data;
        public BaseData base_data;
        public int rest_hits;
        public int rest_hp;
        public ServeGameState NewState
        {
            get
            {
                switch (fight_state)
                {
                    case "FightState.NEW":
                    case "NEW":
                    case "IN_PROGRESS":
                    case "FightState.IN_PROGRESS":
                        return ServeGameState.Playing;
                    case "FightState.WIN":
                    case "WIN":
                        return ServeGameState.Victory;
                    case "FightState.LOSE":
                    case "LOSE":
                        return ServeGameState.Defeat;
                    default:
                        return ServeGameState.Playing;
                }
            }
        }
        
        
    }
}