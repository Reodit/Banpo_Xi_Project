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
    [SerializeField]
    float textDownSize = 0.02f;
    string damageText = "9999";
    byte alpha = 255;
    byte disapearValue = 5;

    CinemachineVirtualCamera vcam1;
    [SerializeField]
    TMP_Text numberBoxTxt;

    private void Start()
    {
        vcam1 = FindObjectOfType<CinemachineVirtualCamera>();
        StartCoroutine(textSizeCoroutine());
    }

    public void setNumberBox(int damage)
    {
        damageText = damage.ToString();
        numberBoxTxt.text = damageText;
    }

    private void Update()
    {
        gameObject.transform.LookAt(vcam1.transform.position);
        gameObject.transform.Rotate(0, 180, 0);
    }

    IEnumerator textSizeCoroutine()
    {
        while (numberBoxTxt.fontSize >= minTextSize)
        {
            alpha--;
            alpha--;
            alpha--;
            alpha--;
            numberBoxTxt.color =
                new Color32(16,214,224, alpha);
            numberBoxTxt.fontSize -= textDownSize;
            yield return new WaitForSeconds(textDownSizingTerm);
        }
    }
}
