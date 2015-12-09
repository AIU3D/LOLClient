#region

using System.Collections.Generic;
using GameProtocol;
using GameProtocol.Dto;
using UnityEngine;
using UnityEngine.UI;

#endregion

/// <summary>
///     选择人物界面
/// </summary>
public class SelectScene : MonoBehaviour
{
    [SerializeField]
    private GameObject heroBtn;
    [SerializeField]
    private Transform listParent;
    [SerializeField]
    private GameObject initMask;
    [SerializeField]
    private SelectGrid[] left;
    [SerializeField]
    private SelectGrid[] right;
    [SerializeField]
    private Button ready;
    private Dictionary<int, HeroGrid> gridMap = new Dictionary<int, HeroGrid>(); 
    private void Start()
    {
        SelectEventUtil.selected = Selected;
        SelectEventUtil.refreshView = RefreshView;
        //显示遮罩防治误操作
        initMask.SetActive(true);
        //初始化英雄列表
        InitHeroList();
        //通知进入场景并加载完成
        this.Wirte(Protocol.TYPE_SELECT, 0, SelectProtocol.ENTER_CREQ, null);
    }

    public void CloseMask()
    {
        initMask.SetActive(false);
    }
    /// <summary>
    ///     初始化英雄列表
    /// </summary>
    private void InitHeroList()
    {
        if (GameData.user == null)
        {
            return;
        }
        int index = 0;
        //通过计算加载英雄列表
        foreach (int item   in GameData.user.HeroList)
        {
            GameObject btn = Instantiate<GameObject>(heroBtn);
            HeroGrid grid = btn.GetComponent<HeroGrid>();
            grid.Init(item);
            btn.transform.parent = listParent;
            btn.transform.localScale = Vector3.one;
            btn.transform.localPosition = new Vector3((48 + index*72*(index%7)), (-42 + index/7*-72), 0);
            gridMap.Add(item,grid);
            index++;
        }
    }

    void RefreshView(SelectRoomDTO room)
    {
        int team = room.GetTeam(GameData.user.ID);
        
        if (team == 1)
        {
            for (int i = 0; i < room.TeamOne.Length; i++)
            {
                left[i].Refresh(room.TeamOne[i]);
            }
            for (int i = 0; i < room.TeamTwo.Length; i++)
            {
                right[i].Refresh(room.TeamTwo[i]);
            }
        }
        else
        {
            for (int i = 0; i < room.TeamOne.Length; i++)
            {
                right[i].Refresh(room.TeamOne[i]);
            }
            for (int i = 0; i < room.TeamTwo.Length; i++)
            {
                left[i].Refresh(room.TeamTwo[i]);
            }
        }
        RefreshHeroList(room);
    }

    private void RefreshHeroList(SelectRoomDTO room)
    {
        int team = room.GetTeam(GameData.user.ID);
        List<int> selected = new List<int>();
        if (team == 1)
        {
            foreach (SelectModel item in room.TeamOne)
            {
                if (item.Hero != -1)
                {
                   selected.Add(item.Hero); 
                }
            }
        }
        else
        {
            foreach (SelectModel item in room.TeamTwo)
            {
                if (item.Hero != -1)
                {
                    selected.Add(item.Hero);
                }
            }
        }

        //将已选的英雄从选择菜单设置状态改为不可选状态
        foreach (int item in gridMap.Keys)
        {
            if (selected.Contains(item)||!ready.interactable)
            {
                gridMap[item].Deactive();
            }
            else
            {
                gridMap[item].Active();
            }
        }
    }

    public void Selected()
    {
        ready.interactable = false;
    }
}