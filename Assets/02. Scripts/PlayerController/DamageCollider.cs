using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [SerializeField]
    float hitBoxDisappearTime;
    [SerializeField]
    GameObject numberHitBox;
    Vector3 target;

    Collider damageCollider;
    public int currentWeaponDamage = 25;

    private void Awake()
    {
        damageCollider = GetComponent<Collider>();
        damageCollider.gameObject.SetActive(true); 
        damageCollider.isTrigger = true;
        damageCollider.enabled = false;
    }
    public void EnableDamageCollider()
    {
        damageCollider.enabled = true;
    }
    public void DisableDamageCollider()
    {
        damageCollider.enabled = false;
    }
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("onTrigger");
        if (collision.CompareTag("Enemy"))
        {
            EnemyStats enemyStats = collision.GetComponent<EnemyStats>();

            if (enemyStats != null)
            {
                enemyStats.TakeDamage(currentWeaponDamage);
                target = this.gameObject.transform.position;
                NumerHitBox();
            }
        }
    }
    private void NumerHitBox()
    {
        var numBox = Instantiate(numberHitBox, target, Quaternion.identity);
        numBox.GetComponent<NumberHitBoxController>().setNumberBox(currentWeaponDamage);
        Destroy(numBox, hitBoxDisappearTime);
    }

}
