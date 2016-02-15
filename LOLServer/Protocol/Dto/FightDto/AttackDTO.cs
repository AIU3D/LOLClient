#region 描述

// **********************************************************************
// 
// 文件名(File Name)：			AttackDTO.cs
// 
// 作者(Author)：				da_fei
// 
// 创建时间(CreateTime):			2016-01-29 15:34:45Z
//
// 描述(Description):			AttackDTO攻击传输模型				
//
// **********************************************************************

#endregion

using System;

namespace GameProtocol.Dto.FightDto
{
    [Serializable]
    public class AttackDTO
    {
        /// <summary>
        /// 攻击者ID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 被攻击者ID
        /// </summary>
        public int TargetID { get; set; }
 
    }
}