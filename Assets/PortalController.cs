using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using Invector.vCharacterController;
using UnityEngine.UI;

public class PortalController : MonoBehaviour
{
    [SerializeField]
    GameObject UI_PopUp;
    [SerializeField]
    TMP_Text dialog;
    [SerializeField]
    string dialogTxt;
    [SerializeField]
    Transform linkPortalTrans;
    [SerializeField]
    CinemachineVirtualCamera vcam1;
    [SerializeField]
    Button enterBtn, cancleBtn;
    [SerializeField]
    Light pointLight;
    vThirdPersonInput playerScript;

    [SerializeField]
    List<GameObject> playerList = new List<GameObject>();


    void Start()
    {
        enterBtn.onClick.AddListener(Teleport);
        cancleBtn.onClick.AddListener(CancleTeleport);
    }

    private void OnCollisionEnter(Collision collision)
    {
        dialog.text = dialogTxt;
        if(collision.gameObject.tag == "Player")
        {
            playerList.Add(collision.gameObject);
            playerScript = collision.gameObject.GetComponent<vThirdPersonInput>();
            UI_PopUp.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            playerScript.enabled = false;
            vcam1.enabled = false;
            StartCoroutine(LightIntensityUp());
            Debug.Log("Enter");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerList.Remove(collision.gameObject);
            playerScript = collision.gameObject.GetComponent<vThirdPersonInput>();
            UI_PopUp.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            playerScript.enabled = true;
            vcam1.enabled = true;
            StopCoroutine(LightIntensityUp());
            pointLight.intensity = 2;
            Debug.Log("Exit");
        }
    }

    public void CancleTeleport()
    {
        UI_PopUp.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        playerScript.enabled = true;
        vcam1.enabled = true;
        StopCoroutine(LightIntensityUp());
        pointLight.intensity = 2;
    }

    public void Teleport()
    {
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
            pointLight.intensity += 1;
            yield return new WaitForSeconds(1f);
        }
    }

}
