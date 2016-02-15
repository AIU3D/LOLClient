#region 描述

// **********************************************************************
// 
// 文件名(File Name)：			FightRoom.cs
// 
// 作者(Author)：				da_fei
// 
// 创建时间(CreateTime):			2016-01-26 13:49:12Z
//
// 描述(Description):			FightRoom  战斗房间				
//
// **********************************************************************

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GameProtocol;
using GameProtocol.Constans;
using GameProtocol.Dto;
using GameProtocol.Dto.FightDto;
using NetFrame;
using NetFrame.Auto;
using Tools;

namespace LOLServer.Logic.Fight
{
    public class FightRoom : AbsMutilHandler, IHandlerInterface
    {
        public Dictionary<int, AbsFightModel> teamOnes = new Dictionary<int, AbsFightModel>();
        public Dictionary<int, AbsFightModel> teamTwos = new Dictionary<int, AbsFightModel>();

        public List<int> offLine = new List<int>(); 
         //房间ID自增器 需要线程安全
        private int enterCount = 0;
        
        #region 
        public int AddIndex
        {
            get { return Interlocked.Increment(ref enterCount); }
        }

        public int ReduIndex
        {
            get { return Interlocked.Decrement(ref enterCount); }
        }

        public int RestIndex
        {
            get { return Interlocked.Exchange(ref enterCount, 0); }
        }

        #endregion

        public void ClientClose(UserToken token, string error)
        {
            Leave(token);
            int userID = GetUserId(token);
            if(teamOnes.ContainsKey(userID) || teamTwos.ContainsKey(userID))
            {
                if(!offLine.Contains(userID))
                {
                    offLine.Add(userID);
                }
            }

            if(offLine.Count == enterCount)
            {
                EventUtil.destroyFight(GetArea());
            }
        }

        public void MessageReceive(UserToken token, SocketModel message)
        {
            switch (message.Command)
            {
                case FightProtocol.ENTER_CREQ:
                    Enter(token);
                    break;
                case FightProtocol.MOVE_CREQ:
                    Move(token,message.GetMessage<MoveDTO>());
                    break;
                case FightProtocol.ATTACK_CREQ:
                    Attack(token,message.GetMessage<int>());
                    break;
                case FightProtocol.DAMAGE_CREQ:
                    Damage(token, message.GetMessage<DamageDTO>());
                    break;
            }
        }

        

        #region 消息的处理

        private new void Enter(UserToken token)
        {
            if (IsEntered(token)) return;
            base.Enter(token);
            //所有人准备了 发送房间信息
            if(ReduIndex==0)
            {
                FightRoomModel room = new FightRoomModel();
                room.teamOne = teamOnes.Values.ToArray();
                room.teamTwo = teamTwos.Values.ToArray();
                Brocast(FightProtocol.START_BRO,room);
            }
        }
       
        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="token"></param>
        /// <param name="value"></param>
        private void Move(UserToken token,MoveDTO value)
        {
            int userID = GetUserId(token);
            value.UserID = userID;
            Brocast(FightProtocol.MOVE_BRO,value);
        }
        
        private void Attack(UserToken token,int value)
        {
            AttackDTO atk = new AttackDTO();
            atk.UserID = GetUserId(token);
            atk.TargetID = value;
            Brocast(FightProtocol.ATTACK_BRO,atk);
        }

        private void Damage(UserToken token, DamageDTO value)
        {
            int userId = GetUserId(token);
            AbsFightModel atkModel;
            int skillLevel = 0;
            //判断攻击者是玩家英雄 还是小兵
            if (value.UserID >= 0)
            {
                if (value.UserID != userId) return;
                atkModel = GetPlayer(userId);
                if (value.Skill > 0)
                {
                    skillLevel = (atkModel as FightPlayerModel).SkillLevel(value.Skill);
                    if (skillLevel == -1)
                    {
                        return;
                    }
                }
            }
            else
            {
                //TODO:
                atkModel = GetPlayer(userId);
            }
            //获取技能算法
            //循环获取目标数据 和攻击者数据 进行伤害计算 得出伤害数值
            if (!SkillProcessMap.Has(value.Skill)) return;
            ISkill skill = SkillProcessMap.Get(value.Skill);
            List<int[]> damages = new List<int[]>();
            foreach (int[] item in value.Targets)
            {
                AbsFightModel target = GetPlayer(item[0]);
                skill.Damage(skillLevel, ref atkModel, ref target, ref damages);
                if (target.Hp == 0)
                {
                    switch (target.Type)
                    {
                        case ModelType.HUMAN:
                            if (target.Id > 0)
                            {
                                //击杀英雄
                                //启动定时任务 指定时间之后发送英雄复活信息 并且将英雄数据设置为满状态
                            }
                            else
                            {
                                //击杀小兵
                                //移除小兵数据
                            }
                            break;
                        case ModelType.BUILD:
                            //打破了建筑 给钱

                            break;
                    }
                }
            }
            value.Targets = damages.ToArray();
            Brocast(FightProtocol.DAMAGE_BRO, value);
        }

        AbsFightModel GetPlayer(int userId)
        {
            if (teamOnes.ContainsKey(userId))
            {
                return teamOnes[userId];
            }
            return teamTwos[userId];
        }
        #endregion

        public override byte GetType()
        {
            return Protocol.TYPE_FIGHT;
        }

        #region 数据初始化

        internal void Init(SelectModel[] teamOne, SelectModel[] teamTwo)
        {
            offLine.Clear();
            this.enterCount = teamOne.Length + teamTwo.Length;
            //初始化英雄数据
            for (int i = 0; i < teamOne.Length; i++)
            {
                this.teamOnes.Add(teamOne[i].UserID, Create(teamOne[i],1));
            }

            for (int i = 0; i < teamTwo.Length; i++)
            {
                this.teamTwos.Add(teamTwo[i].UserID, Create(teamTwo[i],2));
            }

            //添加建筑
            // 预留ID段 -1 到- 10 为队伍一的建筑，-11 到- 20 为队伍2 建筑
            for (int i = -1; i >= -3; i--)
            {
                this.teamOnes.Add(i, CreateBuild(i, Math.Abs(i),1));
            }

            for (int i = -11; i >= -13; i--)
            {
                this.teamTwos.Add(i, CreateBuild(i, Math.Abs(i) - 10,2));
            }

        }

        private FightBuildModel CreateBuild(int id, int code,int team)
        {
            BuildDataModel data = BuildData.buildMap[code];
            FightBuildModel model = new FightBuildModel(id, code, data.Hp, data.Hp, data.Atk, data.Def, data.IsBorn,
                data.BornTime, data.Initiative, data.Infrared);
            model.Type = ModelType.BUILD;
            model.Team = team;
            return model;
        }

        private FightPlayerModel Create(SelectModel model,int team)
        {
            FightPlayerModel player = new FightPlayerModel();
            player.Id = model.UserID;
            player.Code = model.Hero;
            player.Name = GetUser(model.UserID).Name;
            player.Exp = 0;
            player.Level = 1;
            player.FreePoint = 1;
            player.Mongy = 0;
            player.Type = ModelType.HUMAN; 
            //从配置表里取出对应的英雄数据，然后计算
            HeroDataModel data = HeroData.heroMap[model.Hero];
            player.Hp = data.HpBase;
            player.HpMax = data.HpBase;
            player.Atk = data.AtkBase;
            player.Def = data.DefBase;
            player.MoveSpeed = data.MoveSpeed;
            player.AtkSpeed = data.AtkSpeed;
            player.AtkRange = data.AtkRange;
            player.EyeRange = data.EyeRange;
            player.Skills = InitSkill(data.Skills);
            player.Team = team;

            player.Equs = new int[3];


            return player;
        }

        private FightSkill[] InitSkill(int[] value)
        {
            FightSkill[] skills = new FightSkill[value.Length];

            for (int i = 0; i < value.Length; i++)
            {
                int skillCode = value[i];
                SkillDataModel data = SkillData.skillMap[skillCode];
                SkillLevelData skillLevel = data.levels[0];
                FightSkill skill = new FightSkill(skillCode, 0, skillLevel.level, skillLevel.range, skillLevel.time,
                    data.name, data.info, data.target, data.type);
                skills[i] = skill;
            }

            return skills;

            #endregion

        }
    }
}