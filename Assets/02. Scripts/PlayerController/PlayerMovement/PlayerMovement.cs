using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerMovement : MonoBehaviour
{
    // 변수
    public float speed = 5f; // 이동 속도
    public float jumpVelocity = 20f; // 점프할 때 순간적 속도
    [Range(0.01f, 1f)] public float airControlPercent; // 공중에 있는동안 속도 

    // Damping 값
    public float speedSmoothTime = 0.1f;  
    public float turnSmoothTime = 0.1f;    

    private float speedSmoothVelocity; 
    private float turnSmoothVelocity;
    private float currentVelocity_Y;
    
    // 컴포넌트
    private CharacterController cc;
    private PlayerInput playerInput; 
    private Animator anim;
    private Camera followCam;
    public float currentSpeed => new Vector2(cc.velocity.x, cc.velocity.z).magnitude;
    
    // 애니메이터 변수
    private int HashVerticalMove = Animator.StringToHash("Vertical Move");
    private int HashHorizontalMove = Animator.StringToHash("Horizontal Move");

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        anim = GetComponent<Animator>();
        followCam = Camera.main;
    }

    private void Update()
    {
        UpdateAnimation(playerInput.moveInput);    
    }

    private void FixedUpdate()
    {
        if (playerInput.jump)
        {
            Jump();
        }
        if (currentSpeed > 0.2f || playerInput.attack)
        {
            Rotate();
        }

        Move(playerInput.moveInput);
    }

    public void Move(Vector2 moveInput)
    {
        var targetSpeed = speed * moveInput.magnitude;
        var moveDirection = transform.forward * moveInput.y + transform.right * moveInput.x;

        var smoothTime = cc.isGrounded ? speedSmoothTime : speedSmoothTime / airControlPercent;

        targetSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, smoothTime);
        currentVelocity_Y += Time.deltaTime * Physics.gravity.y;

        var velocity = moveDirection * targetSpeed + Vector3.up * currentVelocity_Y;

        cc.Move(velocity * Time.deltaTime);

        if (cc.isGrounded)
        {
            currentVelocity_Y = 0f;
        }
    }
    public void Rotate()
    {
        var targetRotation = followCam.transform.eulerAngles.y;
        targetRotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        transform.eulerAngles = Vector3.up * targetRotation;
    }
    public void Jump()
    {
        if (!cc.isGrounded) return;

        currentVelocity_Y = jumpVelocity;
        anim.CrossFadeInFixedTime("Jump", 0.1f);
    }

    public void Attack()
    {
        
    }

    private void UpdateAnimation(Vector2 moveInput)
    {
        var animationSpeedPercent = currentSpeed / speed;
        anim.SetFloat(HashVerticalMove, moveInput.y * animationSpeedPercent, 0.05f, Time.deltaTime);
        anim.SetFloat(HashHorizontalMove, moveInput.x * animationSpeedPercent, 0.05f, Time.deltaTime);
    }
}
