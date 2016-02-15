#region 描述

// **********************************************************************
// 
// 文件名(File Name)：			Enum1.cs
// 
// 作者(Author)：				da_fei
// 
// 创建时间(CreateTime):			2016-01-26 14:29:14Z
//
// 描述(Description):
//
// **********************************************************************

#endregion

namespace GameProtocol.Dto.FightDto
{
    /// <summary>
    ///     能够造成效果的单位类型
    /// </summary>
    public enum SkillTarget
    {
        /// <summary>
        ///     自身释放
        /// </summary>
        SELF,

        /// <summary>
        ///     友方英雄
        /// </summary>
        F_H,

        /// <summary>
        ///     友方非建筑单位
        /// </summary>
        F_N_B,

        /// <summary>
        ///     友方全体
        /// </summary>
        F_ALL,

        /// <summary>
        ///     敌方英雄
        /// </summary>
        E_H,

        /// <summary>
        ///     敌方非建筑单位
        /// </summary>
        E_N_B,

        /// <summary>
        ///     敌方和中立单位
        /// </summary>
        E_S_N,

        /// <summary>
        ///     非友方单位
        /// </summary>
        N_F_ALL
    }

    /// <summary>
    ///     技能类型
    /// </summary>
    public enum SkillType
    {
        /// <summary>
        ///     以自身为中心释放
        /// </summary>
        SELF,

        /// <summary>
        ///     以目标为中心释放
        /// </summary>
        TARGET,

        /// <summary>
        ///     以鼠标位置为目标释放
        /// </summary>
        POSITION,

        /// <summary>
        ///     被动技能
        /// </summary>
        PASSIVE
    }

    /// <summary>
    ///     战斗模型类型
    /// </summary>
    public enum ModelType
    {
        /// <summary>
        ///     建筑类型
        /// </summary>
        BUILD,

        /// <summary>
        ///     英雄类型
        /// </summary>
        HUMAN
    }
}