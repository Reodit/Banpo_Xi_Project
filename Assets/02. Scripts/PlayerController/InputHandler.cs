using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace PC
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        //public float mouseX;
        //public float mouseY;
        public bool b_Input;
        public bool dashFlag;

        PlayerControls inputActions;
        //CameraHandler cameraHandler;

        Vector2 movementInput;
        Vector2 cameraInput;

        private void Awake()
        {
            //cameraHandler = CameraHandler.singleton;
        }
        //private void FixedUpdate()
        //{
        //    float delta = Time.fixedDeltaTime;

        //    if(cameraHandler != null ) 
        //    {
        //        cameraHandler.FollowTarget(delta);
        //        cameraHandler.HandleCameraRotation(delta, mouseX, mouseY);

        //    }
        //}

        public void OnEnable()
        {
            if(inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed +=
                    inputActions => movementInput = inputActions.ReadValue<Vector2>();

                //inputActions.PlayerMovement.Camera.performed +=
                //    i => cameraInput = i.ReadValue<Vector2>();
            }
            inputActions.Enable();
        }
        private void OnDisable()
        {
            inputActions.Disable();
        }
        public void TickInput(float delta)
        {
            MoveInput(delta);
            HandleDashInput(delta);
        }
        private void MoveInput(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            // Mathf.Clamp01 : 0과 1사이의 값을 고정하고 값을 반환
            // Mathf.Abs : 절대값 반환
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

            //mouseX = cameraInput.x;
            //mouseY = cameraInput.y;\       
        }
        private void HandleDashInput(float delta)
        {
            b_Input = inputActions.PlayerActions.Dash.phase == UnityEngine.InputSystem.InputActionPhase.Started;
            if (b_Input)
            {
                dashFlag = true;
            }
        }
    }
}