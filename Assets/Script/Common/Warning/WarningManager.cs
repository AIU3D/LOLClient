using System.Collections.Generic;
using UnityEngine;

public class WarningManager:MonoBehaviour
{
    public static List<WarningModel> errors = new List<WarningModel>();

    [SerializeField] private WarningWindow window;

    void Update()
    {
        if (errors.Count > 0)
        {
            WarningModel model = errors[0];
            window.Active(model);
            errors.RemoveAt(0);
        }
    }
}