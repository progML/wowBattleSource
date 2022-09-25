using System;
using UnityEngine;

namespace Data.ServerData
{
    [Serializable]
    public class FightPostData
    {
        public int position;

        public FightPostData(int position)
        {
            this.position = position;
        }
    }

    [Serializable]
    public class AuthPostData
    {
        public string login;

        public AuthPostData(string login)
        {
            this.login = login;
        }
    }

    [Serializable]
    public class BasePostData
    {
        public int shields_bit_mask;
        public BasePostData(int shields_bit_mask)
        {
            this.shields_bit_mask = shields_bit_mask;
        }
    }
}