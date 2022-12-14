using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string moveHorizontalAxisName = "Horizontal";
    public string moveVerticalAxisName = "Vertical";

    public string attackButtonName = "Fire1";
    public string dashButtonName = "Dash";
    public string jumpButtonName = "Jump";
    
    public Vector2 moveInput { get; private set; }
    public bool attack { get; private set; }
    public bool jump { get; private set; }
    
    public bool dash { get; private set; }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = new Vector2(Input.GetAxis(moveHorizontalAxisName), Input.GetAxis(moveVerticalAxisName));
        if (moveInput.sqrMagnitude > 1)
            moveInput = moveInput.normalized;
        
        
        jump = Input.GetButtonDown((jumpButtonName));
        attack = Input.GetButton((attackButtonName));
        dash = Input.GetButtonDown((dashButtonName));
    }

}
