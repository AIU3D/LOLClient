#region

using GameProtocol.Dto;
using UnityEngine;
using UnityEngine.UI;

#endregion

public class SelectGrid
{
    [SerializeField]
    private Image head; //头像
    [SerializeField]
    private Image img;
    [SerializeField]
    private Text name; //名称

    public void Refresh(SelectModel model)
    {
        name.text = model.Name;
        if (model.IsEnter)
        {
            if (model.Hero == -1)
            {
                head.sprite = Resources.Load<Sprite>("HeroIcon/nul");
            }
            else
            {
                head.sprite = Resources.Load<Sprite>("HeadIcon/" + model.Hero);
            }
        }
        else
        {
            head.sprite = Resources.Load<Sprite>("HeadIcon/NoEnter");
        }

        if (model.IsReady)
        {
        }
        else
        {
            img.color = Color.white;
        }

    }

    private void Selected()
    {
        img.color = Color.red;
    }
}