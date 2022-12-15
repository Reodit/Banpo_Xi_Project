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
    CinemachineFreeLook vcam1;
    [SerializeField]
    Light pointLight;
    vThirdPersonController playerScript;
    vThirdPersonInput playerInput;
    IEnumerator coroutine;

    [SerializeField]
    private string[] arrayBoss_Dragon;
    [SerializeField]
    private GameObject Boss_DragonPrefab;
    private List<BaseGameEntity> entitys;

    private void Update()
    {
        // 모든 에이전트의 Updated()를 호출해 에이전트 구동
        for (int i = 0; i < entitys.Count; ++i)
        {
            if (entitys[i])
            {
                entitys[i].Updated();
            }

            else
            {
                //Debug.Log("적이 할당되지 않거나 모두 Destroy 되었습니다.");
                return;
            }
        }
    }

    private void Start()
    {
        entitys = new List<BaseGameEntity>();
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
            UIManager.Instance.isUIOpen = true;
            playerInput.enabled = false;
            vcam1.enabled = false;
            StartCoroutine(coroutine);
        }
    }

    
    private void BossRaid()
    {
        for (int i = 0; i < arrayBoss_Dragon.Length; ++i)
        {
            // 에이전트 생성, 초기화 메소드 호출.
            GameObject clone = Instantiate(Boss_DragonPrefab, this.transform.position, Quaternion.identity);
            Boss_Dragon entity = clone.GetComponent<Boss_Dragon>();
            entity.Setup(arrayBoss_Dragon[i]);


            // 에이전트들의 재생 제어를 위해 리스트에 저장
            entitys.Add(entity);
        }

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
        UIManager.Instance.isUIOpen = false;
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
        UIManager.Instance.isUIOpen = false;
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
