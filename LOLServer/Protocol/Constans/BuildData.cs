#region 描述

// **********************************************************************
// 
// 文件名(File Name)：			BuildData.cs
// 
// 作者(Author)：				da_fei
// 
// 创建时间(CreateTime):			2016-01-26 16:08:17Z
//
// 描述(Description):			BuildData	箭塔数据			
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
    public class BuildData
    {
        public static readonly Dictionary<int, BuildDataModel> buildMap = new Dictionary<int, BuildDataModel>();

        /// <summary>
        ///     第一次访问类时创建
        /// </summary>
        static BuildData()
        {
//            Create(1, 5000, 0, 50, false, 0, "主基地", false, true);
            Create(1, 3000, 200, 50, false, 30, "高级箭塔", true, true);
            Create(2, 2000, 150, 30, true, 0, "中级箭塔", false, true);
            Create(3, 1000, 100, 20, true, 0, "初级箭塔", false, true);
        }

        private static void Create(int code, int hp, int atk, int def, bool initiative, int bornTime, string name,
            bool isBorn, bool infrared)
        {
            BuildDataModel model = new BuildDataModel(code, hp, atk, def, initiative, bornTime, name, isBorn, infrared);
            buildMap.Add(code, model);
        }
    }

    [Serializable]
    public partial class BuildDataModel
    {
        public BuildDataModel()
        {
        }

        public BuildDataModel(int code, int hp, int atk, int def, bool initiative, int bornTime, string name,
            bool isBorn, bool infrared)
        {
            Code = code;
            Hp = hp;
            Atk = atk;
            Def = def;
            Initiative = initiative;
            BornTime = bornTime;
            Name = name;
            IsBorn = isBorn;
            Infrared = infrared;
        }

        /// <summary>
        ///     编码ID
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        ///     血量
        /// </summary>
        public int Hp { get; set; }

        /// <summary>
        ///     攻击力
        /// </summary>
        public int Atk { get; set; }

        /// <summary>
        ///     防御
        /// </summary>
        public int Def { get; set; }

        /// <summary>
        ///     是否攻击
        /// </summary>
        public bool Initiative { get; set; }

        /// <summary>
        ///     重生时间
        /// </summary>
        public int BornTime { get; set; }

        /// <summary>
        ///     名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     是否重生
        /// </summary>
        public bool IsBorn { get; set; }

        /// <summary>
        ///     是否反隐
        /// </summary>
        public bool Infrared { get; set; }
    }
}