#region

using System;
using System.Collections.Concurrent;
using System.Threading;
using GameProtocol;
using LOLServer.DAO.Model;
using NetFrame;
using NetFrame.Auto;
using Tools;

#endregion

namespace LOLServer.Logic.Match
{
    /// <summary>
    /// 匹配处理类
    /// </summary>
    public class MatchHandler :AbsHandlerBase, IHandlerInterface
    {
        /// <summary>
        /// 线程安全字典,防止数据竞争导致的脏数据，玩家所在匹配房间映射
        /// </summary>
        private ConcurrentDictionary<int, int> userRoom = new ConcurrentDictionary<int, int>();  
        /// <summary>
        /// 房间id与模型的映射
        /// </summary>
        private ConcurrentDictionary<int, MatchRoom> roomMap = new ConcurrentDictionary<int, MatchRoom>();

        /// <summary>
        /// 回收利用过的房间对象再次利用，减少gc性能开销
        /// </summary>
        private ConcurrentStack<MatchRoom> cacheRooms = new ConcurrentStack<MatchRoom>();

        //房间ID自增器 需要线程安全
        private int index = 0;
        public int AddIndex
        {
            get { return Interlocked.Increment(ref index); }
        }

        public int ReduIndex
        {
            get { return Interlocked.Decrement(ref index); }
        }

        public int RestIndex
        {
            get { return Interlocked.Exchange(ref index,0); }
        }

        public void ClientClose(UserToken token, string error)
        {
            Leave(token);
        }

        public void MessageReceive(UserToken token, SocketModel message)
        {
            switch (message.Command)
            {
                case MatchProtocol.ENTER_CREQ:
                    Enter(token);
                    break;
                case MatchProtocol.LEAVE_CREQ:
                    Leave(token);
                    break;
            }
        }

        void Leave(UserToken token)
        {
            //取出用户ID
            int userID = GetUserId(token);
            //判断是否有房间映射关系
            if (!userRoom.ContainsKey(userID)) return;
            int roomID = userRoom[userID];
            //判断是否拥有此房间
            if (roomMap.ContainsKey(roomID))
            {
                MatchRoom room = roomMap[roomID];
                //根据用户所在的队伍进行移除
                if (room.TeamOne.Contains(userID))
                {
                    room.TeamOne.Remove(userID);
                }
                else if (room.TeamTwo.Contains(userID))
                {
                    room.TeamTwo.Remove(userID);
                }
                //移除用户和房间的关系
                userRoom.TryRemove(userID, out roomID);
                //删除房间，并将放入缓存中
                if (room.TeamOne.Count + room.TeamTwo.Count == 0)
                {
                    roomMap.TryRemove(roomID, out room);
                    cacheRooms.Push(room);
                }
            }
            
        }

        private void Enter(UserToken token)
        {
            int userId = GetUserId(token);
            //判断是否正在匹配的房间中
            if (!userRoom.ContainsKey(userId))
            {
                MatchRoom room = null;
                bool isEnter = false;
                //当前是否有在等待中的房间
                if (roomMap.Count > 0)
                {
                    //遍历所有等待中的房间
                    foreach (MatchRoom item in roomMap.Values)
                    {
                        //如果没满员
                        if (item.TeamMax*2>item.TeamOne.Count + item.TeamTwo.Count)
                        {
                            room = item;
                            //如果队伍一没满员则进入队伍1
                            if (room.TeamOne.Count < room.TeamMax)
                            {
                                room.TeamOne.Add(userId);
                            }
                            else
                            {
                                room.TeamTwo.Add(userId);
                            }
                            //添加玩家与房间的映射关系
                            isEnter = true;
                            userRoom.TryAdd(userId, room.Id);
                            break;
                        }
                    }
                    //当所有房间全部满员 判断缓存中是否有房间
                    if (!isEnter)
                    {
                        CreateRoomFromCache(userId, out room);                        
                    }
                }
                else
                {
                    //没有等待中的房间
                    CreateRoomFromCache(userId, out room);
                }

                //不管什么方式进入房间，判断房间是否满员，满了就开始选人，将房间丢进缓存队列
                if (room.TeamOne.Count == room.TeamTwo.Count&& room.TeamOne.Count == room.TeamMax)
                {
                    //通知选人模块开始选人
                    EventUtil.createSelect(room.TeamOne, room.TeamTwo);

                    WriteToUsers(room.TeamOne.ToArray(),GetType(),0,MatchProtocol.ENTER_DELECT_BRO,null);
                    WriteToUsers(room.TeamTwo.ToArray(),GetType(),0,MatchProtocol.ENTER_DELECT_BRO,null);

                    //移除玩家映射
                    foreach (int item in room.TeamOne)
                    {
                        int i;
                        userRoom.TryRemove(item, out i);
                    }
                    foreach (int item in room.TeamTwo)
                    {
                        int i;
                        userRoom.TryRemove(item, out i);
                    }
                    //重置房间数据
                    room.TeamOne.Clear();
                    room.TeamTwo.Clear();
                    //将房间从等待房间中移除
                    roomMap.TryRemove(room.Id, out room);
                    //加入缓存
                    cacheRooms.Push(room);
                }
            }
           
        }

        /// <summary>
        /// 创建房间从缓存数据中
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="room"></param>
        private void CreateRoomFromCache(int userId, out MatchRoom room)
        {
            if (cacheRooms.Count > 0)
            {
                cacheRooms.TryPop(out room);
                AddUserToRoom(userId, room);
            }
            else
            {
                room = new MatchRoom();
                room.Id = AddIndex;
                AddUserToRoom(userId, room);                
            }
        }

        /// <summary>
        /// 将用户添加到创建好的房间内
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="room"></param>
        private void AddUserToRoom(int userId, MatchRoom room)
        {
            room.TeamOne.Add(userId);
            roomMap.TryAdd(room.Id, room);
            userRoom.TryAdd(userId, room.Id);
        }

        public override byte GetType()
        {
            return Protocol.TYPE_MATCH;
        }
    }
}