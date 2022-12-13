using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAggro
{
    public GameObject Target;
    public float TargetDistance;
    public GameObject[] CurrentPlayers;
    public int CurrentPlayersCount;
    
    private GameObject[] _initPlayers;

    public EnemyAggro(GameObject target, GameObject[] players)
    {
        Target = target;
        _initPlayers = players;
        CurrentPlayersCount = _initPlayers.Length;
    }

    public void InitCurrentPlayers()
    {
        int index = 0;
        CurrentPlayers = new GameObject[CurrentPlayersCount];
        
        foreach (var e in _initPlayers)
        {
            if (e)
            {
                CurrentPlayers[index++] = e;
            }
        }
        
        //TODO
        CurrentPlayersCount = CurrentPlayers.Length;
        if (CurrentPlayers.Length == 0)
        {
            Debug.LogError("할당된 플레이어가 없습니다. 모든 플레이어가 Destroy되었나요?");
            return;
        }
    }

    private void TargetInit()
    {
        if (Target)
        {
            Debug.Log("이전 타겟 :  " + Target.transform.name);
            Target = null;
        }

        else
        {
            Debug.Log("이전 Target이 Null 상태였습니다.");
        }
    }
    private void SetAggroByDistance(Vector3 enemyPosition)
    {
        Dictionary<GameObject, float> playerDistanceInfo = new Dictionary<GameObject, float>();
        
        for (int i = 0; i < CurrentPlayersCount; ++i)
        {
            playerDistanceInfo.Add(
                CurrentPlayers[i],
                Vector3.Distance(enemyPosition, CurrentPlayers[i].transform.position));
        }

        Target = playerDistanceInfo.FirstOrDefault(x => x.Value == playerDistanceInfo.Values.Min()).Key;
        TargetDistance = playerDistanceInfo.FirstOrDefault(x => x.Value == playerDistanceInfo.Values.Min()).Value;
        Debug.Log(" 타겟 :  " + Target);
        Debug.Log(" 적 좌표 : " + enemyPosition + "   " + " 플레이어와의 거리 : " + Target.transform.position);
    }
    private void SetAggroBySpecificPlayer(int SpecificPlayerNum)
    {
        if (CurrentPlayersCount > SpecificPlayerNum)
        {
            Target = CurrentPlayers[SpecificPlayerNum];
        }

        else
        {
            Debug.LogError("입력된 Player가 존재하지 않거나 죽어서 해당 Player로 설정할 수 없습니다. 입력한 PlayerNumber : " 
                           + SpecificPlayerNum);
        }
    }
    
    public void SetTarget(EnemyAggroformat aggroformat, Vector3 enemyPosition)
    {
        if (CurrentPlayers.Length == 0)
        {
            Debug.LogError("할당된 플레이어가 없습니다. 모든 플레이어가 Destroy되었나요?");
            return;
        }

        TargetInit();
        
        switch (aggroformat)
        {
            case EnemyAggroformat.Distance:
                SetAggroByDistance(enemyPosition);
                break;
            case EnemyAggroformat.LowCurrentHP:
                Debug.LogError("아직 미구현");
                break;
            case EnemyAggroformat.HighCurrentHP:
                Debug.LogError("아직 미구현");
                break;
        }

        if (!Target)
        {
            Debug.LogError("타겟이 null 입니다.");
        }
    }
    
    public void SetTarget(EnemyAggroformat aggroformat, int SpecificNum)
    {
        if (CurrentPlayers.Length == 0)
        {
            Debug.LogError("할당된 플레이어가 없습니다. 모든 플레이어가 Destroy되었나요?");
            return;
        }
        
        TargetInit();

        switch (aggroformat)
        {
            case EnemyAggroformat.SpecificJob:
                Debug.LogError("아직 미구현");
                break;
            case EnemyAggroformat.SpecificPlayer:
                SetAggroBySpecificPlayer(SpecificNum);
                break;
        }
        
        if (!Target)
        {
            Debug.LogError("타겟이 null 입니다.");
        }
    }
}
