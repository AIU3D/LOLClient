#region 描述

// **********************************************************************
// 
// 文件名(File Name)：			DamageDTO.cs
// 
// 作者(Author)：				da_fei
// 
// 创建时间(CreateTime):			2016-01-29 16:43:59Z
//
// 描述(Description):			DamageDTO 伤害计算模型				
//
// **********************************************************************

#endregion

using System;

namespace GameProtocol.Dto.FightDto
{
    [Serializable]
    public class DamageDTO
    {
        /// <summary>
        /// 使用者ID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 使用技能
        /// </summary>
        public int Skill { get; set; }

        /// <summary>
        /// 攻击目标
        /// </summary>
        public int[][] Targets { get; set; }
 
    }
}