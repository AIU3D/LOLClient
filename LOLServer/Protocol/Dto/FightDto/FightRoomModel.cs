#region 描述

// **********************************************************************
// 
// 文件名(File Name)：			FightRoomModel.cs
// 
// 作者(Author)：				da_fei
// 
// 创建时间(CreateTime):			2016-01-27 11:22:07Z
//
// 描述(Description):			FightRoomModel战斗房间的传输模型				
//
// **********************************************************************

#endregion

using System;

namespace GameProtocol.Dto.FightDto
{
    [Serializable]
    public class FightRoomModel
    {
        public AbsFightModel[] teamOne;
        public AbsFightModel[] teamTwo;
    }
}