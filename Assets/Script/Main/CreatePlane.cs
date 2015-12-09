using GameProtocol;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Main
{
    public class CreatePlane:MonoBehaviour
    {
        [SerializeField] private InputField nameField;
        [SerializeField] private Button btnSubmit;

        public void Open()
        {
            btnSubmit.enabled = true;
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void Click()
        {
            if (nameField.text.Length > 6 || nameField.text == string.Empty)
            {
                WarningManager.errors.Add(new WarningModel( "请输入正确昵称"));
                return;
            }
            btnSubmit.enabled = false;
            this.Wirte(Protocol.TYPE_USER,0,UserProtocol.CREATE_CREQ,nameField.text);
        }
    }
}