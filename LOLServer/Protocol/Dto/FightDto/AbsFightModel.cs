#region 描述

// **********************************************************************
// 
// 文件名(File Name)：			AbsFightModel.cs
// 
// 作者(Author)：				da_fei
// 
// 创建时间(CreateTime):			2016-01-26 14:05:42Z
//
// 描述(Description):			AbsFightModel 战斗角色公有属性				
//
// **********************************************************************

#endregion

using System;

namespace GameProtocol.Dto.FightDto
{
    [Serializable]
    public class AbsFightModel
    {
        /// <summary>
        ///     攻击力
        /// </summary>
        public int Atk { get; set; }

        /// <summary>
        ///     防御
        /// </summary>
        public int Def { get; set; }

        /// <summary>
        ///     血量
        /// </summary>
        public int Hp { get; set; }

        /// <summary>
        ///     战斗区域中的唯一识别码ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     最大血量
        /// </summary>
        public int HpMax { get; set; }

        /// <summary>
        ///     名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     攻击速度
        /// </summary>
        public float AtkSpeed { get; set; }

        /// <summary>
        ///     移动速度
        /// </summary>
        public float MoveSpeed { get; set; }

        /// <summary>
        ///     攻击范围
        /// </summary>
        public float AtkRange { get; set; }

        /// <summary>
        ///     视野范围
        /// </summary>
        public float EyeRange { get; set; }

        /// <summary>
        ///     模型唯一识别码，但是战斗中会有多个相同兵种出现
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        ///     标识当前生命体的类别
        /// </summary>
        public ModelType Type { get; set; }

        /// <summary>
        ///     单位所在的队伍
        /// </summary>
        public int Team { get; set; }
    }
}