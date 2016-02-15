#region

using System.Collections.Generic;
using GameProtocol.Dto;

#endregion

namespace Tools
{
    /// <summary>
    ///     事件工具类
    /// </summary>
    public class EventUtil
    {
        public static CreateSelect createSelect;
        public static DestroySelect destroySelect;
        public static CreateFight createFight;
        public static DestroyFight destroyFight;
    }
}

/// <summary>
///     创建选人模块事件
/// </summary>
/// <param name="teamOne"></param>
/// <param name="teamTwo"></param>
public delegate void CreateSelect(List<int> teamOne, List<int> teamTwo);

/// <summary>
///     移除选人模块事件
/// </summary>
/// <param name="roomID"></param>
public delegate void DestroySelect(int roomID);

/// <summary>
///     创建战斗模块事件
/// </summary>
/// <param name="teamOne"></param>
/// <param name="teamTwo"></param>
public delegate void CreateFight(SelectModel[] teamOne, SelectModel[] teamTwo);

/// <summary>
///     移出战斗结束事件
/// </summary>
/// <param name="roomTD"></param>
public delegate void DestroyFight(int roomTD);