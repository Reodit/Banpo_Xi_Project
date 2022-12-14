using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraonAttackEffect : MonoBehaviour
{
    [SerializeField] GameObject BreathEffect;
    [SerializeField] GameObject ClawAttackEffect;
    [SerializeField] GameObject BiteAttactEffect;


    void PlayBreathEffect()
    {
        if (BreathEffect)
        {
            GameObject InitParentObj = GameObject.FindGameObjectWithTag("ParticleParts");
            Instantiate(BreathEffect, InitParentObj.transform);
        }
    }

    void PlayClawAttackEffect()
    {
        if (ClawAttackEffect)
        {
            BreathEffect.SetActive(true);
        }
    }

    void PlayBiteAttactEffect()
    {
        if (BiteAttactEffect)
        {
            BreathEffect.SetActive(true);
        }
    }
}
