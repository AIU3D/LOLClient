#region 描述

// **********************************************************************
// 
// 文件名(File Name)：			AnimState.cs
// 
// 作者(Author)：				da_fei
// 
// 创建时间(CreateTime):			2016-01-27 17:03:48Z
//
// 描述(Description):			AnimState 动画状态表				
//
// **********************************************************************

#endregion

namespace Assets.Script.Fight
{
    /// <summary>
    /// 动画状态表
    /// 0待机 1跑 2攻击 3技能 4死亡
    /// </summary>
    public class AnimState
    {
        public const int IDLE = 0;
        public const int RUN = 1;
        public const int ATTACK = 2;
        public const int SKILL = 3;
        public const int DEAD = 4;
    }
}