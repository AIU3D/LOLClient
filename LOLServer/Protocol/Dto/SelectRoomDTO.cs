using System;

namespace GameProtocol.Dto
{
    /// <summary>
    /// 房间选择信息模型
    /// </summary>
    [Serializable]
    public class SelectRoomDTO
    {
        public SelectModel[] TeamOne { get; set; }
        public SelectModel[] TeamTwo { get; set; }

        public int GetTeam(int uid)
        {
            foreach (SelectModel item in TeamOne)
            {
                if (item.UserID == uid) return 1;
            }
            foreach (SelectModel item in TeamTwo)
            {
                if (item.UserID == uid) return 2;
            }
            return -1;
        }
    }
}