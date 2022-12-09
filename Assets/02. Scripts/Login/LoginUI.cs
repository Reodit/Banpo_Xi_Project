using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    [SerializeField] Button _loginButton;
    [SerializeField] TMP_InputField _loginInputField;

    PhotonConnector _photonConnector;
    
    
    #region MonoBehaviourCallbacks
    
    void Start()
    {
        _photonConnector = GetComponent<PhotonConnector>();
        _loginInputField.onValueChanged.AddListener(OnInputFieldChanged);
        _loginButton.onClick.AddListener(OnClickLoginButton);
    }

    

    void Update()
    {
        
    }

    #endregion

    
    #region Public Callbacks

    
    #endregion

    #region Private Callbacks

    void OnInputFieldChanged(string nickName)
    {
        _loginInputField.text = nickName;
        PlayerPrefs.SetString("NICKNAME" , nickName);
    }
    
    void OnClickLoginButton()
    {
        // 이 버튼을 누르면 현재 로그인 UI 꺼지고 호스트 클라이언트 버튼 UI 뜨게 만듬
    }

    #endregion

    #region PhotonFusionCallBacks
    
    
    
    #endregion
}
