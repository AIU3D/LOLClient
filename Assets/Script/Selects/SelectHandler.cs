using GameProtocol;
using GameProtocol.Dto;
using NetFrame.Auto;
using UnityEngine;

public class SelectHandler:MonoBehaviour,IHandler
{
    private SelectRoomDTO room;
    public void MessageReceive(SocketModel model)
    {
        switch (model.Command)
        {
            case SelectProtocol.DESTROY_BRO:
                Application.LoadLevel(1);
                break;
            case SelectProtocol.ENTER_SRES:
                Enter(model.GetMessage<SelectRoomDTO>());
                break;
            case SelectProtocol.ENTER_EXBRO:
                Enter(model.GetMessage<int>());
                break;
            case SelectProtocol.FIGHT_BRO:
                Application.LoadLevel(3);                       
                break;
            case SelectProtocol.READY_BRO:
                Ready(model.GetMessage<SelectModel>());
                break;
            case SelectProtocol.SELECT_BRO:
                Select(model.GetMessage<SelectModel>());
                break;
            case SelectProtocol.SELECT_SRES:
                WarningManager.errors.Add(new WarningModel("角色选择失败"));
                break;
            case SelectProtocol.TALK_BRO:
                Talk(model.GetMessage<string>());
                break;
        }
    }

    private void Ready(SelectModel selectModel)
    {
        if (selectModel.UserID == GameData.user.ID)
        {
            //禁止点击操作
            SelectEventUtil.selected();
        }
        foreach (SelectModel item in room.TeamOne)
        {
            if (item.UserID == selectModel.UserID)
            {
                item.Hero = selectModel.Hero;
                item.IsReady = true;
                //刷新UI界面
                SelectEventUtil.refreshView(room);

                return;
            }
        }
        foreach (SelectModel item in room.TeamTwo)
        {
            if (item.UserID == selectModel.UserID)
            {
                item.Hero = selectModel.Hero;
                item.IsReady = true;
                //刷新UI界面
                SelectEventUtil.refreshView(room);
                return;
            }
        }
    }

    /// <summary>
    /// 选择英雄
    /// </summary>
    /// <param name="selectModel"></param>
    private void Select(SelectModel selectModel)
    {
        foreach (SelectModel item in room.TeamOne)
        {
            if (item.UserID == selectModel.UserID)
            {
                item.Hero = selectModel.Hero;
                //刷新UI界面
                SelectEventUtil.refreshView(room);

                return;
            }
        }
        foreach (SelectModel item in room.TeamTwo)
        {
            if (item.UserID == selectModel.UserID)
            {
                item.Hero = selectModel.Hero;
                //刷新UI界面
                SelectEventUtil.refreshView(room);

                return;
            }
        }
    }


    private void Talk(string value)
    {
        //向聊天面板添加信息
    }
    /// <summary>
    /// 自身的进入
    /// </summary>
    /// <param name="roomDTO"></param>
    private void Enter(SelectRoomDTO roomDTO)
    {
        room = roomDTO;
        //刷新界面UI
        SelectEventUtil.refreshView(room);

    }

    /// <summary>
    /// 其他人的进入
    /// </summary>
    /// <param name="id"></param>
    private void Enter(int id)
    {
        if (room == null) return;
        foreach (SelectModel item in room.TeamOne)
        {
            if (item.UserID == id)
            {
                item.IsEnter = true;
                //刷新UI界面
                SelectEventUtil.refreshView(room);

                return;
            }
        }
        foreach (SelectModel item in room.TeamTwo)
        {
            if (item.UserID == id)
            {
                item.IsEnter = true;
                //刷新UI界面
                SelectEventUtil.refreshView(room);

                return;
            }
        }
    }
}