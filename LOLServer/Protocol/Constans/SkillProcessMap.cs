#region 描述

// **********************************************************************
// 
// 文件名(File Name)：			SkillProcessMap.cs
// 
// 作者(Author)：				da_fei
// 
// 创建时间(CreateTime):			2016-01-29 17:10:54Z
//
// 描述(Description):			SkillProcessMap 技能存储表				
//
// **********************************************************************

#endregion

using System.Collections.Generic;
using GameProtocol.Constans.Skill;

namespace GameProtocol.Constans
{
    public class SkillProcessMap
    {
        static Dictionary<int ,ISkill>  skills = new Dictionary<int, ISkill>();

        static SkillProcessMap()
        {
            Put(-1, new SkillAttack());
        }

        static void Put(int code, ISkill skill)
        {
            skills.Add(code,skill);
        }

        public static bool Has(int code)
        {
            return skills.ContainsKey(code);
        }

        public static ISkill Get(int code)
        {
            return skills[code];
        }
    }
}