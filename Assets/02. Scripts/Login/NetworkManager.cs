using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class NetworkManager : NetworkBehaviour
{
    public static Action<NetworkManager> OnlobbyChanged;
    public static NetworkManager Instance { get; private set; }
    
    [Networked(OnChanged = nameof(OnChangedLobbyInfo))] public int PlayerCount { get; set; }
    
    
    #region MonoBehaviour Callbacks

    void Awake()
    {
        if (Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    #endregion

    #region pulbic Methods

    #endregion

    #region Private Callbacks

    static void OnChangedLobbyInfo(Changed<NetworkManager> changed)
    {
        OnlobbyChanged?.Invoke(Instance);
    }

    #endregion
    
    
}
