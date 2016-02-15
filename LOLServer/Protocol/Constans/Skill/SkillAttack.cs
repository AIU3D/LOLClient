#region 描述

// **********************************************************************
// 
// 文件名(File Name)：			SkillAttack.cs
// 
// 作者(Author)：				da_fei
// 
// 创建时间(CreateTime):			2016-01-29 17:14:20Z
//
// 描述(Description):			SkillAttack 普通攻击处理				
//
// **********************************************************************

#endregion

using System.Collections.Generic;
using GameProtocol.Dto.FightDto;

namespace GameProtocol.Constans.Skill
{
    public class SkillAttack: ISkill
    {
        public void Damage(int level, ref AbsFightModel atk, ref AbsFightModel target, ref List<int[]> damages)
        {
            int value = atk.Atk - target.Def;
            value = value > 0 ? value : 1;
            target.Hp = target.Hp - value <= 0 ? 0 : target.Hp - value;
            damages.Add(new int[]{target.Id,value,target.Hp == 0?0:1});
        }
    }
}