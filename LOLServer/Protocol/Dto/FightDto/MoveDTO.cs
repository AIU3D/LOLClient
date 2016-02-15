#region 描述

// **********************************************************************
// 
// 文件名(File Name)：			MoveDTO.cs
// 
// 作者(Author)：				da_fei
// 
// 创建时间(CreateTime):			2016-01-28 16:17:35Z
//
// 描述(Description):			MoveDTO移动数据				
//
// **********************************************************************

#endregion

using System;

namespace GameProtocol.Dto.FightDto
{
    [Serializable]
    public class MoveDTO
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }

      
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }
}