using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace Invector.vCharacterController
{
    public class vNThirdPersonInput : NetworkBehaviour
    {
        #region Variables       

        [Header("Controller Input")]
        public string horizontalInput = "Horizontal";
        public string verticallInput = "Vertical";
        public KeyCode jumpInput = KeyCode.Space;
        public KeyCode strafeInput = KeyCode.Tab;
        public KeyCode sprintInput = KeyCode.LeftShift;
        public KeyCode dashInput = KeyCode.Mouse1;
        public KeyCode attackInput = KeyCode.Mouse0;
        public KeyCode skillQInput = KeyCode.Q;

        [Header("Camera Input")]
        public string rotateCameraXInput = "Mouse X";
        public string rotateCameraYInput = "Mouse Y";

        [HideInInspector] public vThirdPersonController cc;
        [HideInInspector] public vThirdPersonCamera tpCamera;
        [HideInInspector] public Camera cameraMain;

        #endregion

        protected virtual void Start()
        {
            InitilizeController();
            InitializeTpCamera();
            Cursor.lockState = CursorLockMode.Locked;
        }

        protected virtual void FixedUpdate()
        {
            cc.UpdateMotor();               // updates the ThirdPersonMotor methods
            cc.ControlLocomotionType();     // handle the controller locomotion type and movespeed
            cc.ControlRotationType();       // handle the controller rotation type
        }

        public override void FixedUpdateNetwork()
        {
            //    cc.UpdateMotor();               // updates the ThirdPersonMotor methods
            //    cc.ControlLocomotionType();     // handle the controller locomotion type and movespeed
            //    cc.ControlRotationType();       // handle the controller rotation type

            InputHandle();                  // update the input methods
            cc.UpdateAnimator();            // updates the Animator Parameters
            //cc.CursorManager();
        }


        protected virtual void Update()
        {
            //InputHandle();                  // update the input methods
            cc.UpdateAnimator();            // updates the Animator Parameters
            cc.CursorManager();
        }

        public virtual void OnAnimatorMove()
        {
            cc.ControlAnimatorRootMotion(); // handle root motion animations 
        }

        #region Basic Locomotion Inputs

        protected virtual void InitilizeController()
        {
            Debug.Log("cc get component");
            cc = GetComponent<vThirdPersonController>();

            if (cc != null)
                cc.Init();
        }

        protected virtual void InitializeTpCamera()
        {
            if (tpCamera == null)
            {
                tpCamera = FindObjectOfType<vThirdPersonCamera>();
                if (tpCamera == null)
                    return;
                if (tpCamera)
                {
                    tpCamera.SetMainTarget(this.transform);
                    tpCamera.Init();
                }
            }
        }
        protected virtual void InputHandle()
        {
            if (cc && GetInput(out NetworkInputData data))
            {
                MoveInput(data);
                CameraInput();
                SprintInput(data);
                StrafeInput(data);
                JumpInput();
                DashInput();
                AttackInput();
            }
        }

        public virtual void MoveInput(NetworkInputData data)
        {           
            //cc.input.x = Input.GetAxis(horizontalInput);
            //cc.input.z = Input.GetAxis(verticallInput);

            cc.input.x = data.xAxis;
            cc.input.z = data.zAxis;
        }
        IEnumerator WaitForCamera()
        {
            yield return new WaitUntil(() => Camera.main != null);
            yield return new WaitUntil(() => cc != null);
            cameraMain = Camera.main;
            cc.rotateTarget = cameraMain.transform;
        }
        protected virtual void CameraInput()
        {
            if (!cameraMain)
            {
                StartCoroutine(WaitForCamera());
                //if (!Camera.main) Debug.Log("Missing a Camera with the tag MainCamera, please add one.");
                //else
                //{
                //    cameraMain = Camera.main;
                //    cc.rotateTarget = cameraMain.transform;
                //}
            }

            if (cameraMain)
            {
                cc.UpdateMoveDirection(cameraMain.transform);
            }

            if (tpCamera == null)
                return;

            var Y = Input.GetAxis(rotateCameraYInput);
            var X = Input.GetAxis(rotateCameraXInput);

            tpCamera.RotateCamera(X, Y);
        }

        protected virtual void StrafeInput(NetworkInputData data)
        {
            if (data.strafe)
                cc.Strafe();
        }

        protected virtual void SprintInput(NetworkInputData data)
        {
            //if (Input.GetKeyDown(sprintInput))
            //    cc.Sprint(true);
            //else if (Input.GetKeyUp(sprintInput))
            //    cc.Sprint(false);

            cc.Sprint(data.sprint);
        }

        /// <summary>
        /// Conditions to trigger the Jump animation & behavior
        /// </summary>
        /// <returns></returns>
        protected virtual bool JumpConditions()
        {
            return cc.isGrounded && cc.GroundAngle() < cc.slopeLimit && !cc.isJumping && !cc.stopMove;
        }
        protected virtual bool DashConditions()
        {
            return !cc.isDashing;
        }
        protected virtual bool AttackConditions()
        {
            return !cc.isJumping && !cc.isDashing;
        }
        protected virtual bool SkillQConditions()
        {
            return !cc.isJumping && !cc.isDashing && cc.isGrounded;
        }

        /// <summary>
        /// Input to trigger the Jump 
        /// </summary>
        protected virtual void JumpInput()
        {
            if (Input.GetKeyDown(jumpInput) && JumpConditions())
                cc.Jump();
        }
        protected virtual void DashInput()
        {
            if (Input.GetKeyDown(dashInput) && DashConditions())
                cc.Dash();
        }

        protected virtual void AttackInput()
        {
            if (Input.GetKey(attackInput) && AttackConditions())
            {
                cc.Attack();
            }
        }
        #endregion
    }
}
