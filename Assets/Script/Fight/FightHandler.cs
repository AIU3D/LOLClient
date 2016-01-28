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
        private Dictionary<int, GameObject> models = new Dictionary<int, GameObject>();
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
            }
        }

        private void Move(MoveDTO value)
        {
            Vector3 target = new Vector3( value.X,value.Y,value.Z);
            models[value.UserID].SendMessage("Move",target);
        }

        private void StartFight(FightRoomModel model)
        {
            room = model;
            string path = null;
            foreach (AbsFightModel item in model.teamOne)
            {
                GameObject go = null;
                if (item.Type == ModelType.HUMAN)
                {
                    path = "Player/" + item.Code;
                    go = Load(path, start1);
                    PlayerCtr ctrl = go.GetComponent<PlayerCtr>();
                    ctrl.Init((FightPlayerModel)item);
                    this.models.Add(item.Id,go );
                }
                else
                {
                    path = "Build/1_" + item.Code;
                    this.models.Add(item.Id, Load(path, position1[item.Code - 1]));
                }
                if(item.Id == GameData.user.ID)
                {
                    FightScene.Instance.InitView((FightPlayerModel)item,go);
                    FightScene.Instance.LookAt();
                }
            }

            foreach (AbsFightModel item in model.teamTwo)
            {
                GameObject go = null;
                if (item.Type == ModelType.HUMAN)
                {
                    path = "Player/" + item.Code;
                    go = Load(path, start2);
                    PlayerCtr ctrl = go.GetComponent<PlayerCtr>();
                    ctrl.Init((FightPlayerModel)item);
                    this.models.Add(item.Id, go);
                }
                else
                {
                    path = "Build/2_" + item.Code;
                    this.models.Add(item.Id, Load(path, position2[item.Code - 1]));
                }
                if (item.Id == GameData.user.ID)
                {
                    FightScene.Instance.InitView((FightPlayerModel)item,go);
                    FightScene.Instance.LookAt();
                }
            }
        }

        private GameObject Load(string path, Transform trans)
        {
            return (GameObject) Instantiate(Resources.Load<GameObject>(path), trans.position, trans.rotation);
        }
    }
}

