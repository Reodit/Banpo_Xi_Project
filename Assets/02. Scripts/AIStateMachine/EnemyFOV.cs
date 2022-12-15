using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    public float ViewRange = 15f;
    public float ViewAngle = 100f;

    public GameObject bossDragonObj;
    private Boss_Dragon boss_dragon;
    private Transform enemyTr;
    private Transform playerTr;
    private LayerMask playerLayer;
    private LayerMask obstacleLayer;
    private LayerMask layerMask;

    public void InitFOV()
    {
        bossDragonObj = GameObject.FindGameObjectWithTag("Enemy");
        boss_dragon = bossDragonObj.GetComponent<Boss_Dragon>();
        enemyTr = boss_dragon.transform;
        playerTr = boss_dragon.mEnemyAggro.Target.transform;

        playerLayer = LayerMask.GetMask("Player");
        obstacleLayer = LayerMask.GetMask("Obstacle");
        layerMask = playerLayer | obstacleLayer;
    }

    public Vector3 CirclePoint(float angle)
    {
        angle += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad),
            0,
            Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    public bool isTracePlayer()
    {
        bool isTrace = false;

        Collider[] colls = Physics.OverlapSphere(enemyTr.position,
            ViewRange, 1 << playerLayer);

        if (colls.Length == 1)
        {
            Vector3 dir = (playerTr.position - enemyTr.position).normalized;

            if (Vector3.Angle(enemyTr.forward, dir) < ViewAngle * 0.5f)
            {
                isTrace = true;
            }
        }

        return isTrace;
    }

    public bool isViewPlayer()
    {
        bool isView = false;
        RaycastHit hit;

        Vector3 dir = (playerTr.position - enemyTr.position).normalized;

        if (Physics.Raycast(enemyTr.position, dir, out hit, ViewRange, layerMask))
        {
            isView = (hit.collider.CompareTag("Player"));
        }

        return isView;
    }
}
