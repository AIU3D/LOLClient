#region

using UnityEngine;
using UnityEngine.UI;

#endregion

public class HeroGrid : MonoBehaviour
{
    [SerializeField] private Button btn;

    private int id = -1;

    [SerializeField] private Image img;

    public void Init(int id)
    {
        this.id = id;
        img.sprite = Resources.Load<Sprite>("");
        img.sprite = Resources.Load<Sprite>("HeroIcon/" + id);
    }

    public void Active()
    {
        btn.interactable = true;
    }

    public void Deactive()
    {
        btn.interactable = false;
    }

    public void Click()
    {
        if (id != -1)
        {
        }
    }
}