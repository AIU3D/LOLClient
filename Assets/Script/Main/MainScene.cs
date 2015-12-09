#region

using Assets.Script.Main;
using GameProtocol;
using UnityEngine;
using UnityEngine.UI;

#endregion

public class MainScene : MonoBehaviour
{
    [SerializeField] private GameObject mask;
    [SerializeField] private CreatePlane createPlane;
    [SerializeField] private Text nameText;
    [SerializeField] private Slider expBar;
    [SerializeField] private Text matchText; // 匹配按钮文本对象
    private void Start()
    {
        if (GameData.user == null)
        {
            mask.SetActive(true);
            //向服务器要数据
            this.Wirte(Protocol.TYPE_USER,0,UserProtocol.INFO_CREQ,null);
        }
    }

    public void RefreshView()
    {
        nameText.text = GameData.user.Name + "  等级" + GameData.user.Level;
        expBar.value = GameData.user.Exp/100f;
    }

    void OpenCreatePlane()
    {
        createPlane.Open();
    }

    void CloseCreatePlane()
    {
        createPlane.Close();
    }

    void CloseMask()
    {
        mask.SetActive(false);
    }

    /// <summary>
    /// 匹配
    /// </summary>
    public void MatchGroup()
    {
        if (matchText.text.Equals("开始排队"))
        {
            matchText.text = "取消排队";
            this.Wirte(Protocol.TYPE_MATCH,0,MatchProtocol.ENTER_CREQ,null);
        }
        else
        {
            matchText.text = "开始排队";
            this.Wirte(Protocol.TYPE_MATCH, 0, MatchProtocol.LEAVE_CREQ, null);

        }
    }
}