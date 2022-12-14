using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPBarController : MonoBehaviour
{
    [SerializeField]
    Boss_Dragon boss;
    [SerializeField]
    Slider hpBar;
    [SerializeField]
    Image fillColor;
    [SerializeField]
    TMP_Text barCounterTxt;

    [SerializeField]
    Color32[] hpBarColors = new Color32[3];

    private void Update()
    {
        UpdateHPBar();
    }

    void UpdateHPBar()
    {
        if (boss.HP > boss.page2HP) fillColor.color = hpBarColors[0];
        if (boss.HP <= boss.page2HP && boss.HP > boss.page3HP) fillColor.color = hpBarColors[1];
        if (boss.HP <= boss.page3HP) fillColor.color = hpBarColors[2];

        barCounterTxt.text = "x" + ((boss.HP + 999)/ 1000).ToString();
        hpBar.value = (float) (boss.HP % 1001) / 1000;
        Debug.Log(hpBar.value);
    }
}
