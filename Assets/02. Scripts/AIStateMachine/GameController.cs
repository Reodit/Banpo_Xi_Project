using System.Collections.Generic;
using UnityEngine;

public enum Phase { Normal = 0, Berserk, Groggy };

public class GameController : MonoBehaviour
{
    [SerializeField]
    private string[] arrayBoss_Dragon; 
    [SerializeField]
    private GameObject Boss_DragonPrefab;  

    // 재생 제어를 위한 모든 에이전트 리스트
    private List<BaseGameEntity> entitys;

    private void Awake()
    {
        entitys = new List<BaseGameEntity>();

        for (int i = 0; i < arrayBoss_Dragon.Length; ++i)
        {
            // 에이전트 생성, 초기화 메소드 호출.
            GameObject clone = Instantiate(Boss_DragonPrefab, this.transform.position, Quaternion.identity);
            Boss_Dragon entity = clone.GetComponent<Boss_Dragon>();
            entity.Setup(arrayBoss_Dragon[i]);
            

            // 에이전트들의 재생 제어를 위해 리스트에 저장
            entitys.Add(entity);
        }
    }

    private void Update()
    {
        // 모든 에이전트의 Updated()를 호출해 에이전트 구동
        for (int i = 0; i < entitys.Count; ++i)
        {
            entitys[i].Updated();
        }
    }
}

