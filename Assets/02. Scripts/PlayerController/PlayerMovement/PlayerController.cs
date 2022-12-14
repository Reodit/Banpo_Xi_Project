using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent((typeof(CharacterController)))]
public class PlayerController : MonoBehaviour
{
    // 변수
    [SerializeField] 
    private float speed = 3.0f;
    [SerializeField]
    private float rotateSpeed = 3.0f;

    private Vector3 moveDirection;
    // 컴포넌트
    CharacterController cc;
    
    
    
    
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }
    void Update()
    {
        
    }

    void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        moveDirection = new Vector3(horizontal, 0, vertical);
        moveDirection *= speed;

        cc.SimpleMove(moveDirection);
    }
}
