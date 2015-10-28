using System;
using GameProtocol;
using GameProtocol.Dto;
using UnityEngine;
using UnityEngine.UI;

public class LoginScreen : MonoBehaviour
{
    /// <summary>
    /// 序列化可以在外部拖拽控件，但是不让外部类调用
    /// </summary>

    #region 登录面板部分
    [SerializeField] private InputField accountInput;

    [SerializeField] private InputField passwordInput;

    [SerializeField] private Button loginBtn;

    #endregion

    [SerializeField] private GameObject regPanel;

    [SerializeField] private InputField regAccountInput;

    [SerializeField] private InputField regPWInput;

    [SerializeField] private InputField regRepeatPW;

    private void Start()
    {
        NetIO io = NetIO.Instance;
    }

    /// <summary>
    /// 登录按钮
    /// </summary>
    public void LoginOnClick()
    {
        if (accountInput.text.Length == 0 || accountInput.text.Length > 6)
        {
            Debug.Log("账号错误");
            WarningManager.errors.Add(new WarningModel("账号不合法"));
            return;
        }

        if (passwordInput.text.Length == 0 || passwordInput.text.Length > 6)
        {
            return;
        }

        //验证通过，申请登录
        AccountInfoDTO dto = new AccountInfoDTO();
        dto.account = accountInput.text.Trim();
        dto.password = passwordInput.text.Trim();
        NetIO.Instance.Write(GameProtocol.Protocol.TYPE_LOGIN, 0, LoginProtocol.LOGIN_CREQ, dto);
        loginBtn.interactable = false; //按钮是否可选
    }

    public void OpenLoginBtn()
    {
        passwordInput.text = string.Empty;
        loginBtn.interactable = true;
    }

    /// <summary>
    /// 注册按钮事件
    /// </summary>
    public void RegClick()
    {
        regPanel.SetActive(true);
    }

    public void RegCloseClick()
    {
        regAccountInput.text = string.Empty;
        regPWInput.text = string.Empty;
        regRepeatPW.text = string.Empty;
        regPanel.SetActive(false);
    }

    public void RegAccount()
    {
        if (regAccountInput.text.Length == 0 || regAccountInput.text.Length > 6)
        {
            Debug.Log("账号错误");
            return;
        }

        if (regPWInput.text.Length == 0 || regPWInput.text.Length > 6)
        {
            Debug.Log("密码错误");
            return;
        }

        if (!regPWInput.text.Equals(regRepeatPW.text))
        {
            Debug.Log("输入不一致");
            return;
        }

        //验证通过，申请注册，并关闭注册面板
        AccountInfoDTO dto = new AccountInfoDTO();
        dto.account = regAccountInput.text.Trim();
        dto.password = regPWInput.text.Trim();
        NetIO.Instance.Write(Protocol.TYPE_LOGIN, 0, LoginProtocol.REG_CREQ, dto);
        RegCloseClick();
    }
}