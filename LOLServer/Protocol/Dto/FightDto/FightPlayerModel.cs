#region 描述

// **********************************************************************
// 
// 文件名(File Name)：			FightPlayerModel.cs
// 
// 作者(Author)：				da_fei
// 
// 创建时间(CreateTime):			2016-01-26 14:06:54Z
//
// 描述(Description):			FightPlayerModel 玩家模型				
//
// **********************************************************************

#endregion

using System;

namespace GameProtocol.Dto.FightDto
{
    [Serializable]
    public class FightPlayerModel : AbsFightModel
    {
        /// <summary>
        ///     等级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        ///     经验
        /// </summary>
        public int Exp { get; set; }

        /// <summary>
        ///     当前能量
        /// </summary>
        public int MP { get; set; }

        /// <summary>
        ///     最大能量
        /// </summary>
        public int MaxMP { get; set; }

        /// <summary>
        ///     剩余技能点数
        /// </summary>
        public int FreePoint { get; set; }

        /// <summary>
        ///     拥有钱数
        /// </summary>
        public int Mongy { get; set; }

        /// <summary>
        ///     装备表
        /// </summary>
        public int[] Equs { get; set; }

        /// <summary>
        ///     技能表
        /// </summary>
        public FightSkill[] Skills { get; set; }

        public int SkillLevel(int code)
        {
            foreach (FightSkill skill in Skills)
            {
                if(skill.Code == code)
                {
                    return skill.SkillLevel;
                }
            }
            return -1;
        }
    }
}