#region 描述

// **********************************************************************
// 
// 文件名(File Name)：			ISkill.cs
// 
// 作者(Author)：				da_fei
// 
// 创建时间(CreateTime):			2016-01-29 17:11:56Z
//
// 描述(Description):			ISkill 技能接口，实现伤害计算				
//
// **********************************************************************

#endregion

using System.Collections.Generic;
using GameProtocol.Dto.FightDto;

namespace GameProtocol.Constans
{
    public interface ISkill
    {
        void Damage(int level, ref AbsFightModel atk, ref AbsFightModel target, ref List<int[]> damages);
    }
}