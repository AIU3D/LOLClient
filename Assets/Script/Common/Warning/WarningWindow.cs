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
        if (value.delay >0)
        {
            Invoke("Close",value.delay);
        }
        gameObject.SetActive(true);
    }

    public void Close()
    {
        if (IsInvoking("Close"))
        {
            CancelInvoke("Close");
        }
        gameObject.SetActive(false);
        if (result != null) result();
    }
    

}