using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Invector.vCharacterController;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class AltarController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    GameObject UI_PopUp;
    [SerializeField]
    TMP_Text dialog;
    [SerializeField]
    string dialogTxt;
    [SerializeField]
    Button enterBtn, cancelBtn;
    [SerializeField]
    TMP_Text enterTxt, cancelTxt;

    [Header("GamePlay")]
    [SerializeField]
    List<GameObject> interactionColliders = new List<GameObject>();
    [SerializeField]
    List<Light> pointLights = new List<Light>();
    [SerializeField]
    CinemachineVirtualCamera vcam1;
    [SerializeField]
    GameObject GC;
    [SerializeField]
    Light pointLight;
    vThirdPersonController playerScript;
    vThirdPersonInput playerInput;
    IEnumerator coroutine;

    private void Start()
    {
        coroutine = LightIntensityUp();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            enterBtn.onClick.AddListener(BossRaid);
            cancelBtn.onClick.AddListener(CancleBossRaid);
            dialog.text = dialogTxt;
            enterTxt.text = "확인";
            cancelTxt.text = "취소";
            UI_PopUp.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            playerScript = other.gameObject.GetComponent<vThirdPersonController>();
            playerInput = other.gameObject.GetComponent<vThirdPersonInput>();
            playerScript.stopMove = true;
            playerInput.enabled = false;
            vcam1.enabled = false;
            StartCoroutine(coroutine);
        }
    }

    
    private void BossRaid()
    {
        GC.SetActive(true);
        this.transform.position -= new Vector3(0, -20f, 0);
        foreach (var collider in interactionColliders)
        {
            collider.SetActive(false);
        }

        foreach (var light in pointLights)
        {
            light.color = Color.red;
        }
        Cursor.lockState = CursorLockMode.Locked;
        playerScript.stopMove = false;
        playerInput.enabled = true;
        vcam1.enabled = true;
        UI_PopUp.SetActive(false);
        StopCoroutine(coroutine);
        pointLight.intensity = 10f;
    }

    public void CancleBossRaid()
    {
        UI_PopUp.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        playerScript.stopMove = false;
        playerInput.enabled = true;
        vcam1.enabled = true;
        StopCoroutine(coroutine);
        pointLight.intensity = 10f;
    }

    IEnumerator LightIntensityUp()
    {
        while (pointLight.intensity < 100)
        {
            pointLight.intensity += 0.5f;
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator LightIntensityDown()
    {
        while (pointLight.intensity > 2)
        {
            pointLight.intensity -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
