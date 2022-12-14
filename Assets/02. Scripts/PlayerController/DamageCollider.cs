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

    GameObject TargetMonster;
    Boss_Dragon boss_Dragon;

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
        //Debug.Log("onTrigger");
        if (collision.CompareTag("Enemy"))
        {
            TargetMonster = GameObject.FindGameObjectWithTag("Enemy");
            boss_Dragon = TargetMonster.GetComponent<Boss_Dragon>();

            target = this.gameObject.transform.position;
            currentWeaponDamage = Random.Range(90, 110);
            NumerHitBox();
            TakeDamage();
        }
    }
    private void NumerHitBox()
    {
        var numBox = Instantiate(numberHitBox, target, Quaternion.identity);
        numBox.GetComponent<NumberHitBoxController>().setNumberBox(currentWeaponDamage);
        Destroy(numBox, hitBoxDisappearTime);
    }

    void TakeDamage()
    {
        boss_Dragon.HP -= currentWeaponDamage;
    }
}
