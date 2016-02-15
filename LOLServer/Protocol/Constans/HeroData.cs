#region 描述

// **********************************************************************
// 
// 文件名(File Name)：			HeroData.cs
// 
// 作者(Author)：				da_fei
// 
// 创建时间(CreateTime):			2016-01-26 15:22:43Z
//
// 描述(Description):			HeroData英雄的初始数据				
//
// **********************************************************************

#endregion

#region

using System;
using System.Collections.Generic;

#endregion

namespace GameProtocol.Constans
{
    [Serializable]
    public class HeroData
    {
        public static readonly Dictionary<int, HeroDataModel> heroMap = new Dictionary<int, HeroDataModel>();

        /// <summary>
        ///     第一次访问类时创建
        /// </summary>
        static HeroData()
        {
            Create(1,"阿狸",100,20,500,300,5,2,30,10,1,0.5f,6,1,2,3,4);
            Create(2,"啊木木",100,20,500,300,5,2,30,10,1,0.5f,3,1,2,3,4);
            Create(3,"艾希",100,20,500,300,5,2,30,10,1,0.5f,1,6,2,3,4);
            Create(4,"盲僧",100,20,500,300,5,2,30,10,1,0.5f,1,3,2,3,4);
        }

        private static void Create(int code, string name, int atkBase, int defBase, int hpBase, int mpBase, int atkArr,
            int defArr, int hpArr, int mpArr, float moveSpeed, float atkSpeed, int range, int eyeRange, params int[] skills)
        {
            HeroDataModel model = new HeroDataModel();
            model.Code = code;
            model.Name = name;
            model.AtkBase = atkBase;
            model.DefBase = defBase;
            model.HpBase = hpBase;
            model.MpBase = mpBase;
            model.AtkArr = atkArr;
            model.DefArr = defArr;
            model.HpArr = hpArr;
            model.MpArr = mpArr;
            model.MoveSpeed = moveSpeed;
            model.AtkSpeed = atkSpeed;
            model.AtkRange = range;
            model.EyeRange = eyeRange;
            model.Skills = skills;
            heroMap.Add(code, model);
        }
    }
    [Serializable]
        public partial class HeroDataModel
        {
            /// <summary>
            ///     唯一编号
            /// </summary>
            public int Code { get; set; }

            /// <summary>
            ///     英雄名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            ///     基础攻击力
            /// </summary>
            public int AtkBase { get; set; }

            /// <summary>
            ///     基础防御力
            /// </summary>
            public int DefBase { get; set; }

            /// <summary>
            ///     基础血量
            /// </summary>
            public int HpBase { get; set; }

            /// <summary>
            ///     基础蓝量
            /// </summary>
            public int MpBase { get; set; }

            /// <summary>
            ///     攻击成长
            /// </summary>
            public int AtkArr { get; set; }

            /// <summary>
            ///     防御成长
            /// </summary>
            public int DefArr { get; set; }

            /// <summary>
            ///     血量成长
            /// </summary>
            public int HpArr { get; set; }

            /// <summary>
            ///     蓝量成长
            /// </summary>
            public int MpArr { get; set; }

            /// <summary>
            ///     移动速度
            /// </summary>
            public float MoveSpeed { get; set; }

            /// <summary>
            ///     攻击速度
            /// </summary>
            public float AtkSpeed { get; set; }

            /// <summary>
            ///     攻击距离
            /// </summary>
            public int AtkRange { get; set; }

            /// <summary>
            ///     视野距离
            /// </summary>
            public int EyeRange { get; set; }

            /// <summary>
            ///     技能
            /// </summary>
            public int[] Skills { get; set; }
        }
    
}