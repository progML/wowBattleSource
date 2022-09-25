using System;
using UnityEngine;

namespace Data.ServerData
{
    [Serializable]
    public abstract class BasicResponse
    {
        public bool success;
        public string message;
    }
}