using UnityEngine;

namespace Invector.vCharacterController
{
    public class vThirdPersonInput : MonoBehaviour
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
        [HideInInspector] public Camera cameraMain;

        #endregion

        protected virtual void Start()
        {
            InitilizeController();
            Cursor.lockState = CursorLockMode.Locked;

        }

        protected virtual void FixedUpdate()
        {
            cc.UpdateMotor();               // updates the ThirdPersonMotor methods
            cc.ControlLocomotionType();     // handle the controller locomotion type and movespeed
            cc.ControlRotationType();       // handle the controller rotation type
        }

        protected virtual void Update()
        {
            InputHandle();                  // update the input methods
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
            cc = GetComponent<vThirdPersonController>();

            if (cc != null)
                cc.Init();
        }


        protected virtual void InputHandle()
        {
            if (UIManager.Instance.isUIOpen || cc.isDie) return;

            MoveInput();
            CameraInput();
            SprintInput();
            StrafeInput();
            JumpInput();
            DashInput();
            AttackInput();
        }

        public virtual void MoveInput()
        {
            cc.input.x = Input.GetAxis(horizontalInput);
            cc.input.z = Input.GetAxis(verticallInput);
        }

        protected virtual void CameraInput()
        {
            if (!cameraMain)
            {
                if (!Camera.main) Debug.Log("Missing a Camera with the tag MainCamera, please add one.");
                else
                {
                    cameraMain = Camera.main;
                    cc.rotateTarget = cameraMain.transform;
                }
            }

            if (cameraMain)
            {
                cc.UpdateMoveDirection(cameraMain.transform);
            }


            var Y = Input.GetAxis(rotateCameraYInput);
            var X = Input.GetAxis(rotateCameraXInput);

        }

        protected virtual void StrafeInput()
        {
            if (Input.GetKeyDown(strafeInput))
                cc.Strafe();
        }

        protected virtual void SprintInput()
        {
            if (Input.GetKeyDown(sprintInput))
                cc.Sprint(true);
            else if (Input.GetKeyUp(sprintInput))
                cc.Sprint(false);
        }

        /// <summary>
        /// Conditions to trigger the Jump animation & behavior
        /// </summary>
        /// <returns></returns>
        protected virtual bool JumpConditions()
        {
            return cc.isGrounded && cc.GroundAngle() < cc.slopeLimit && !cc.isJumping  && !cc.isDie;
        }
        protected virtual bool DashConditions()
        {
            return !cc.isDashing && !cc.isDie;
        }
        protected virtual bool AttackConditions()
        {
            return cc.isGrounded && !cc.isJumping && !cc.isDashing && !cc.isDie && !cc.isAttacking;
        }
        protected virtual bool SkillQConditions()
        {
            return !cc.isJumping && !cc.isDashing && cc.isGrounded && !cc.isDie;
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