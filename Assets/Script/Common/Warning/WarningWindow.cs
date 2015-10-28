using UnityEngine;
using UnityEngine.UI;

public class WarningWindow : MonoBehaviour
{
    [SerializeField] private Text text;

    private WarningResult result;
    public void Active(WarningModel value)
    {
        text.text = value.value;
        this.result = value.result;
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
        if (result != null) result();
    }
    

}