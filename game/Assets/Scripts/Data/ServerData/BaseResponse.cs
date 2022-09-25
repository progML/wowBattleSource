using System;

namespace Data.ServerData
{
    [Serializable]
    public class BaseResponse: BasicResponse
    {
        public BaseData data;
    }
}