using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.InsightsModels;
using UnityEngine;

public class PlayfabConnector : MonoBehaviour
{

    string _userName;
    const string TITLE_ID = "D2DC5";
    
    #region MonoBehaviour Callbacks

    void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
            PlayFabSettings.TitleId = TITLE_ID;
        }
    }

    #endregion

    #region Public Methods

    public string Login()
    {
        _userName = PlayerPrefs.GetString("NICKNAME");
        bool enableLogin = IsValidUserName();
        if (enableLogin)
        {
            LoginUsingCustomId();
            return _userName;
        }
        return null;
    }
    
    #endregion

    #region Private Methods

    bool IsValidUserName()
    {
        bool isValid = _userName.Length is >= 3 and <= 24;
        return isValid;
    }
    
    void LoginUsingCustomId()
    {
        Debug.Log("LoginUsingCustomId");
        var request = new LoginWithCustomIDRequest { CustomId = _userName, CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginCustomIdSucceed, OnPlayfabFailed);
    }

    void UpdateDisplayName()
    {
        Debug.Log("UpdateDisplayName");
        var request = new UpdateUserTitleDisplayNameRequest { DisplayName = _userName };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnUpdateUserTitleNameSucceed, OnPlayfabFailed);
    }

    #endregion

    #region PlayFab Callbacks

    void OnLoginCustomIdSucceed(LoginResult obj)
    {
        Debug.Log($"OnLoginCustomIdSucceed : {obj}");
        UpdateDisplayName();
    }
    
    void OnUpdateUserTitleNameSucceed(UpdateUserTitleDisplayNameResult obj)
    {
        Debug.Log($"OnUpdateUserTitleNameSucceed, {obj}");    
    }

    void OnPlayfabFailed(PlayFabError obj)
    {
        Debug.LogError($"OnPlayfabFailed : {obj}");
    }

    #endregion
}
