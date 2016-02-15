using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using GameProtocol;
using GameProtocol.Dto;
using LOLServer.DAO.Model;
using NetFrame;
using NetFrame.Auto;
using Tools;

namespace LOLServer.Logic.Selection
{
    public class SelectRoom:AbsMutilHandler,IHandlerInterface
    {
        public ConcurrentDictionary<int,SelectModel> teamOnes = new ConcurrentDictionary<int, SelectModel>();
        public ConcurrentDictionary<int,SelectModel> teamTwos = new ConcurrentDictionary<int, SelectModel>();

        public List<int> readyList = new List<int>(); 
        //当前已经进入房间的人数
        private int enterCount = 0;
        //当前任务的ID
        private int missionID = -1;
        /// <summary>
        /// 房间初始化
        /// </summary>
        /// <param name="teamOne"></param>
        /// <param name="teamTwo"></param>
        public void Init(List<int> teamOne, List<int> teamTwo)
        {
            this.teamOnes.Clear();
            this.teamTwos.Clear();
            foreach (int item in teamOne)
            {
                SelectModel select = new SelectModel();
                select.UserID = item;
                select.Name = GetUser(item).Name;
                select.Hero = -1;
                select.IsEnter = false;
                select.IsReady = false;
                this.teamOnes.TryAdd(item,select);
            }
            foreach (int item in teamTwo)
            {
                SelectModel select = new SelectModel();
                select.UserID = item;
                select.Name = GetUser(item).Name;
                select.Hero = -1;
                select.IsEnter = false;
                select.IsReady = false;
                this.teamTwos.TryAdd(item,select);
            }

            //初始化完毕，开始定时任务，设定30秒后没有进入选择界面的时候，直接解散此次匹配,使用Timer会新开一个线程影响性能
            missionID = ScheduleUtil.Instance.Schedule(delegate
            {
                //30秒后判断进入情况 如果不是全员进入则解散成员
                if (enterCount<teamOne.Count+teamTwo.Count)
                {
                    DestroyRoom();
                }
                else
                {
                    //选择英雄的时间 再次启动定时任务
                    missionID = ScheduleUtil.Instance.Schedule(delegate
                    {
                        bool selectAll = true;
                        foreach (SelectModel item in this.teamOnes.Values)
                        {
                            if (item.Hero == -1)
                            {
                                selectAll = false;
                                break;
                            }
                        }
                        if (selectAll)
                        {
                            foreach (SelectModel item in this.teamTwos.Values)
                            {
                                if (item.Hero == -1)
                                {
                                    selectAll = false;
                                    break;
                                }
                            }
                        }

                        if (selectAll)
                        {
                            //全部选了，只是没有人点准备按钮，开始战斗
                            StartFight();
                        }
                        else
                        {
                            DestroyRoom();
                        }
                    }, 30*1000);
                }
            }, 30*1000);
        }

        public void ClientClose(UserToken token, string error)
        {
            Leave(token); //先调用离开然后消除房间
            Brocast(SelectProtocol.DESTROY_BRO, null); //通知客户端所有人 房间解散 回主界面
            EventUtil.destroySelect(GetArea()); //调用销毁房间
        }

        private void DestroyRoom()
        {
            Brocast(SelectProtocol.DESTROY_BRO, null); //通知客户端所有人 房间解散 回主界面
            EventUtil.destroySelect(GetArea()); //调用销毁房间
            if (missionID == -1)
            {
                ScheduleUtil.Instance.RemoveMission(missionID);
            }
            enterCount = 0;
        }

        public void MessageReceive(UserToken token, SocketModel message)
        {
            switch (message.Command)
            {
                case SelectProtocol.ENTER_CREQ:
                    EnterRoom(token);
                    break;
                case SelectProtocol.SELECT_CREQ:
                    SelectHero(token, message.GetMessage<int>());
                    break;
                case SelectProtocol.TALK_CREQ:
                    Talk(token,message.GetMessage<string>());
                    break;
                case SelectProtocol.READY_CREQ:
                    Ready(token);
                    break;
            }
        }

        private void Ready(UserToken token)
        {
            //判断玩家是否在房间里
            if (!base.IsEntered(token)) return;
            int userID = GetUserId(token);
            if (readyList.Contains(userID))
            {
                return;
            }
            SelectModel selectModel = null;
            if (teamOnes.ContainsKey(userID))
            {
                selectModel = teamOnes[userID];
            }
            else
            {
                selectModel = teamTwos[userID];
            }
            if (selectModel.Hero == -1)
            {
                //没有选择的话 随机选择一个
            }
            else
            {
                selectModel.IsReady = true;
                Brocast(SelectProtocol.SELECT_BRO,selectModel);
                readyList.Add(userID);
                if (readyList.Count >= teamOnes.Count + teamTwos.Count)
                {
                    //开始战斗
                    StartFight();
                }
            }
        }

        private void StartFight()
        {
            if (missionID != -1)
            {
                ScheduleUtil.Instance.RemoveMission(missionID);
                missionID = -1;
            }
            //通知战斗模块  创建战斗房间
            EventUtil.createFight(teamOnes.Values.ToArray(), teamTwos.Values.ToArray());
            Brocast(SelectProtocol.FIGHT_BRO,null);
            //通知选择房间管理器，销毁当前房间
            EventUtil.destroySelect(GetArea());
        }

        private void Talk(UserToken token, string info)
        {
            //判断玩家是否在房间里
            if (!base.IsEntered(token)) return;
            UserModel user = GetUser(token);
            //群体聊天模式
//            Brocast(SelectProtocol.TALK_BRO,user.Name+" :"+info);

            //队伍聊天模式
            if(teamOnes.ContainsKey(user.ID))
            {
                WriteToUsers(teamOnes.Keys.ToArray(), GetType(), GetArea(), SelectProtocol.TALK_BRO, user.Name + " :" + info);
            }
            else
            {
                WriteToUsers(teamTwos.Keys.ToArray(), GetType(), GetArea(), SelectProtocol.TALK_BRO, user.Name + " :" + info);
            }

        }

        private void SelectHero(UserToken token, int heroID)
        {
            //判断玩家是否在房间里
            if(!base.IsEntered(token)) return;
            UserModel user = GetUser(token);
            //判断玩家是否拥有此英雄
            if (!user.HeroList.Contains(heroID))
            {
                //选择失败通知本人，否则群发消息
                Write(token,SelectProtocol.SELECT_SRES,null);
                return;
            }
            //判断英雄队友是否已选
            SelectModel selectModel = null;
            if (teamOnes.ContainsKey(user.ID))
            {
                foreach (SelectModel model in teamOnes.Values)
                {
                    if (model.Hero == heroID)
                    {
                        return;
                    }
                }
                selectModel = teamOnes[user.ID];
            }
            else
            {
                foreach (SelectModel model in teamTwos.Values)
                {
                    if (model.Hero == heroID)
                    {
                        return;
                    }
                }
                selectModel = teamTwos[user.ID];
            }
            //选择成功，通知房间所有人变更数据
            selectModel.Hero = heroID;
            Brocast(SelectProtocol.SELECT_BRO,selectModel);
        }

        private void EnterRoom(UserToken token)
        {
            //判断用户所在的房间，并对进入状态就行修改
            int userID = GetUserId(token);
            if (teamOnes.ContainsKey(userID))
            {
                teamOnes[userID].IsEnter = true;
            }
            else if (teamTwos.ContainsKey(userID))
            {
                teamTwos[userID].IsEnter = true;
            }
            else
            {
                return;
            }

            //判断用户是否已经在房间，不在则计算累加，否则无视
            if (base.Enter(token))
            {
                enterCount++;
            }
            //进入成功，发送房间信息给其他玩家，并通知其他玩家有人进入
            SelectRoomDTO dto = new SelectRoomDTO();
            dto.TeamOne = teamOnes.Values.ToArray();
            dto.TeamTwo = teamTwos.Values.ToArray();
            Write(token,SelectProtocol.ENTER_SRES,dto);
            Brocast(SelectProtocol.ENTER_EXBRO,userID,dto);
        }

        public override byte GetType()
        {
            return Protocol.TYPE_SELECT;
        }


    }
}