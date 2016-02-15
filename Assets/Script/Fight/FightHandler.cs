#region 描述

// **********************************************************************
// 
// 文件名(File Name)：			FightHandler.cs
// 
// 作者(Author)：				da_fei
// 
// 创建时间(CreateTime):			2016-01-27 11:28:20Z
//
// 描述(Description):			FightHandler 战斗逻辑处理				
//
// **********************************************************************

#endregion

#region

using System.Collections.Generic;
using GameProtocol;
using GameProtocol.Dto.FightDto;
using NetFrame.Auto;
using UnityEngine;

#endregion

namespace Assets.Script.Fight
{
    public class FightHandler : MonoBehaviour, IHandler
    {
        [SerializeField] private Transform[] position1;
        [SerializeField] private Transform[] position2;
        private FightRoomModel room;
        [SerializeField] private Transform start1;
        [SerializeField] private Transform start2;
        private Dictionary<int, PlayerCtr> models = new Dictionary<int, PlayerCtr>();
//        private Dictionary<int, GameObject> teamTwo = new Dictionary<int, GameObject>();

        public void MessageReceive(SocketModel model)
        {
            switch (model.Command)
            {
                case FightProtocol.START_BRO:
                    StartFight(model.GetMessage<FightRoomModel>());
                    break;
                case FightProtocol.MOVE_BRO:
                    Move(model.GetMessage<MoveDTO>());
                    break;
                case FightProtocol.ATTACK_BRO:
                    Move(model.GetMessage<MoveDTO>());
                    break;
                case FightProtocol.DAMAGE_BRO:
                    Damage(model.GetMessage<DamageDTO>());
                    break;
            }
        }

        private void Damage(DamageDTO value)
        {
            foreach (int[] target in value.Targets)
            {
                PlayerCtr pc = models[target[0]];
                //实例化掉血数字
                FightScene.Instance.NumUp(pc.transform, target[1].ToString());
                pc.HpChange();
                if (pc.data.Id == GameData.user.ID)
                {
                    FightScene.Instance.RefreshView(pc.data);
                }
                if (target[2] == 0)
                {
                    if (target[0] >= 0)
                    {
                        pc.gameObject.SetActive(false);
                        if (pc.data.Id == GameData.user.ID)
                        {
                            FightScene.Instance.Dead = true;
                        }
                    }
                    else
                    {
                        Destroy(pc.gameObject);
                    }
                }
            }
        }

        private void Attack(AttackDTO atk)
        {
            PlayerCtr obj = models[atk.UserID];
            PlayerCtr target = models[atk.TargetID];
            obj.Attack(new Transform[] { target.transform });
        }

        private void Move(MoveDTO value)
        {
            Vector3 target = new Vector3( value.X,value.Y,value.Z);
            models[value.UserID].SendMessage("Move",target);
        }

        private void StartFight(FightRoomModel model)
        {
            room = model;

            //判断队伍
            int myTeam = -1;
            foreach (AbsFightModel item in model.teamOne)
            {
                if(item.Id == GameData.user.ID)
                {
                    myTeam = item.Team;
                }
            }
            if(myTeam == -1)
            {
                foreach (AbsFightModel item in model.teamTwo)
                {
                    if (item.Id == GameData.user.ID)
                    {
                        myTeam = item.Team;
                    }
                }
            }


            string path = null;
            foreach (AbsFightModel item in model.teamOne)
            {
                PlayerCtr ctrl = null;
                if (item.Type == ModelType.HUMAN)
                {
                    path = "Player/" + item.Code;
                    ctrl = Load(path, start1);
                    ctrl.Init((FightPlayerModel)item,myTeam);
                    this.models.Add(item.Id, ctrl);
                }
                else
                {
                    path = "Build/1_" + item.Code;
                    this.models.Add(item.Id, Load(path, position1[item.Code - 1]));
                }
                if(item.Id == GameData.user.ID)
                {
                    FightScene.Instance.InitView((FightPlayerModel)item, ctrl.gameObject);
                    FightScene.Instance.LookAt();
                }
            }

            foreach (AbsFightModel item in model.teamTwo)
            {
                PlayerCtr ctrl = null;
                if (item.Type == ModelType.HUMAN)
                {
                    path = "Player/" + item.Code;
                    ctrl = Load(path, start2);
                    ctrl.Init((FightPlayerModel)item,myTeam);
                    this.models.Add(item.Id, ctrl);
                }
                else
                {
                    path = "Build/2_" + item.Code;
                    this.models.Add(item.Id, Load(path, position2[item.Code - 1]));
                }
                if (item.Id == GameData.user.ID)
                {
                    FightScene.Instance.InitView((FightPlayerModel)item, ctrl.gameObject);
                    FightScene.Instance.LookAt();
                }
            }
        }

        private PlayerCtr Load(string path, Transform trans)
        {
            return ((GameObject) Instantiate(Resources.Load<GameObject>(path), trans.position, trans.rotation)).GetComponent<PlayerCtr>();
        }
    }
}

