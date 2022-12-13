using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class NumberHitBoxController : MonoBehaviour
{
    [Header("GamePlay")]
    [SerializeField]
    float minTextSize = 0.5f;
    [SerializeField]
    float textDownSizingTerm = 0.1f;
    string damageText = "9999";

    CinemachineVirtualCamera vcam1;
    [SerializeField]
    TMP_Text numberBoxTxt;

    private void Start()
    {
        vcam1 = FindObjectOfType<CinemachineVirtualCamera>();
        StartCoroutine(textSizeCoroutine());
    }

    void setNumberBox(int damage)
    {
        damageText = damage.ToString();
        numberBoxTxt.text = damageText;
    }

    private void Update()
    {
        gameObject.transform.LookAt(vcam1.transform.position);
    }

    IEnumerator textSizeCoroutine()
    {
        while (numberBoxTxt.fontSize >= minTextSize)
        {
            numberBoxTxt.fontSize -= 0.1f;
            yield return new WaitForSeconds(textDownSizingTerm);
        }
    }
}
