using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public PlayerRef Player { get; private set; }

    [SerializeField] TMP_Text _nickNameText;
    
    string _nickName;
    
    
    
    public void SetPlayer(PlayerRef player, string nickName)
    {
        Player = player;
        _nickName = nickName;
        _nickNameText.text = nickName;
    }
    
    
    
}
