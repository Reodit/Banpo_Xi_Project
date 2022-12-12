using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyAggroformat
{
    Distance,
    LowCurrentHP,
    HighCurrentHP,
    SpecificJob,
    SpecificPlayer
}

public class EnemyAggro
{
    public GameObject Target;
    private GameObject[] players;
    
    private EnemyAggro() { }

    public void SetTarget(float EnemyDistance, EnemyAggroformat aggroformat)
    {
        if (players.Length == 0)
        {
            Debug.LogError("Players are all null!");
            return;
        }
        
        for (int i = 0; i < players.Length; ++i)
        {
            if (players[i])
            {
                // switch (aggroformat)
                // {
                //     case EnemyAggroformat.Distance:
                //         
                //
                // }
            }

            else
            {
                
            }

            if (!Target)
            {
                Debug.LogError("Player is not exist!");
            }
        }
    }
    
}
