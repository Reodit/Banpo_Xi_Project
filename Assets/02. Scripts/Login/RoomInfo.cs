using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class RoomInfo : MonoBehaviour
{
    public PlayerRef Player { get; private set; }
    
    public void SetPlayer(PlayerRef player)
    {
        Player = player;
    }
}
