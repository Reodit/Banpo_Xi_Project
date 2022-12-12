using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using Invector.vCharacterController;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PortalController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    GameObject UI_PopUp;
    [SerializeField]
    TMP_Text dialog;
    [SerializeField]
    string dialogTxt;
    [SerializeField]
    Button enterBtn, cancleBtn;
    [SerializeField]
    TMP_Text enterTxt, cancelTxt;

    [Header("GamePlay")]
    [SerializeField]
    Transform linkPortalTrans;
    [SerializeField]
    CinemachineVirtualCamera vcam1;
    [SerializeField]
    Light pointLight;
    vThirdPersonController playerScript;
    vThirdPersonInput playerInput;

    [SerializeField]
    List<GameObject> playerList = new List<GameObject>();

    IEnumerator coroutine;


    void Start()
    {
        coroutine = LightIntensityUp();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            enterBtn.onClick.AddListener(Teleport);
            cancleBtn.onClick.AddListener(CancleTeleport);
            dialog.text = dialogTxt;
            enterTxt.text = "이동";
            cancelTxt.text = "취소";
            playerList.Add(other.gameObject);
            playerScript = other.gameObject.GetComponent<vThirdPersonController>();
            playerInput = other.gameObject.GetComponent<vThirdPersonInput>();
            UI_PopUp.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            playerScript.stopMove = true;
            playerInput.enabled = false;
            vcam1.enabled = false;
            StartCoroutine(coroutine);
            Debug.Log("Enter");
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerList.Remove(other.gameObject);
            playerScript = other.gameObject.GetComponent<vThirdPersonController>();
            playerInput = other.gameObject.GetComponent<vThirdPersonInput>();
            UI_PopUp.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            playerScript.stopMove = false;
            playerInput.enabled = true;
            vcam1.enabled = true;
            StopCoroutine(coroutine);
            pointLight.intensity = 2f;
            //StartCoroutine(LightIntensityDown());
            Debug.Log("Exit");
        }        
    }

    public void CancleTeleport()
    {
        UI_PopUp.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        playerScript.stopMove = false;
        playerInput.enabled = true;
        vcam1.enabled = true;
        StopCoroutine(coroutine);
        pointLight.intensity = 2f;
        //StartCoroutine(LightIntensityDown());
    }

    public void Teleport()
    {
        StopCoroutine(coroutine);
        pointLight.intensity = 2f;
        //StartCoroutine(LightIntensityDown());
        foreach (var player in playerList)
        {
            player.transform.position = linkPortalTrans.position;
            player.transform.rotation = linkPortalTrans.rotation;
        }
        Debug.Log("Teleport");
    }

    IEnumerator LightIntensityUp()
    {
        while (pointLight.intensity < 50)
        {
            pointLight.intensity += 0.5f;
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator LightIntensityDown()
    {
        while(pointLight.intensity > 2)
        {
            pointLight.intensity -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }

}
