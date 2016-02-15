#region 描述

// **********************************************************************
// 
// 文件名(File Name)：			FightBuildModel.cs
// 
// 作者(Author)：				da_fei
// 
// 创建时间(CreateTime):			2016-01-26 14:34:57Z
//
// 描述(Description):			FightBuildModel建筑模型				
//
// **********************************************************************

#endregion

using System;

namespace GameProtocol.Dto.FightDto
{
    [Serializable]
    public class FightBuildModel:AbsFightModel
    {
        public FightBuildModel()
        {
        }

        public FightBuildModel(int id,int code,int hp,int hpMax,int atk,int def,bool isBorn, int bornTime, bool initiative, bool infrared)
        {
            Id = id;
            Code = code;
            Hp = hp;
            HpMax = hpMax;
            Atk = atk;
            Def = def;
            IsBorn = isBorn;
            BornTime = bornTime;
            Initiative = initiative;
            Infrared = infrared;
        }

        /// <summary>
        /// 是否重生
        /// </summary>
        public bool IsBorn { get; set; }

        /// <summary>
        /// 重生时间
        /// </summary>
        public int BornTime { get; set; }

        /// <summary>
        /// 是否攻击
        /// </summary>
        public bool Initiative { get; set; }

        /// <summary>
        /// 是否反隐
        /// </summary>
        public bool Infrared { get; set; }
    }
}