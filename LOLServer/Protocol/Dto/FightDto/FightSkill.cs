#region 描述

// **********************************************************************
// 
// 文件名(File Name)：			FightSkill.cs
// 
// 作者(Author)：				da_fei
// 
// 创建时间(CreateTime):			2016-01-26 14:17:11Z
//
// 描述(Description):			FightSkill技能模型				
//
// **********************************************************************

#endregion

using System;

namespace GameProtocol.Dto.FightDto
{
    [Serializable]
    public class FightSkill
    {
        public FightSkill()
        {
        }

        public FightSkill(int code, int skillLevel, int nextLevel, float range, float freshTime, string name, string info, SkillTarget target, SkillType type)
        {
            Code = code;
            SkillLevel = skillLevel;
            NextLevel = nextLevel;
            Range = range;
            FreshTime = freshTime;
            Name = name;
            Info = info;
            Target = target;
            Type = type;
        }

        /// <summary>
        ///     技能ID
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        ///     技能等级
        /// </summary>
        public int SkillLevel { get; set; }

        /// <summary>
        ///     升级所需的英雄等级
        /// </summary>
        public int NextLevel { get; set; }

        /// <summary>
        ///     释放距离
        /// </summary>
        public float Range { get; set; }

        /// <summary>
        ///     技能刷新时间
        /// </summary>
        public float FreshTime { get; set; }

        /// <summary>
        ///     技能名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     技能描述
        /// </summary>
        public string Info { get; set; }

        /// <summary>
        ///     技能伤害目标类型
        /// </summary>
        public SkillTarget Target { get; set; }

        /// <summary>
        ///     技能释放类型
        /// </summary>
        public SkillType Type { get; set; }
    }
}