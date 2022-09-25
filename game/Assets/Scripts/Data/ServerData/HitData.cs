using System;

namespace Data.ServerData
{
    [Serializable]
    public class HitData
    {
        public int hit_position;
        public string hit_result;
        public bool hit_success => hit_result == "HitState.BREAK" ||hit_result == "BREAK";
    }
}