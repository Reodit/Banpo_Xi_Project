using Unity.VisualScripting;
using UnityEngine;

namespace Invector.vCharacterController
{
    public class vThirdPersonMotor : MonoBehaviour
    {
        #region 인스펙터 변수

        [Header("- Movement")]

        [Tooltip("'in plcae' 애니메이션이 있는 경우 끄고 위의 값을 사용하여 캐릭터를 이동하거나 추가 속도로 루트 모션과 함께 사용하세요.")]
        public bool useRootMotion = false;
        [Tooltip("월드 축을 사용하여 캐릭터를 회전하려면 이것을 사용하고, 카메라 축을 사용하려면 false를 사용하세요. - Isometric 카메라 확인")]
        public bool rotateByWorld = false;
        [Tooltip("스태미나가 끝나거나 움직임이 멈출 때까지 캐릭터가 달리도록 버튼을 누를 때 Sprint를 사용하려면 이것을 선택하세요. 체크를 해제하면 SprintInput을 누르거나 스태미나가 끝날 때까지 캐릭터가 질주합니다.")]
        public bool useContinuousSprint = true;
        [Tooltip("항상 Free Movement 상태에서 Sprint하려면 이 옵션을 선택하세요.")]
        public bool sprintOnlyFree = true;
        public enum LocomotionType
        {
            FreeWithStrafe,
            OnlyStrafe,
            OnlyFree,
        }
        public LocomotionType locomotionType = LocomotionType.FreeWithStrafe;

        public vMovementSpeed freeSpeed, strafeSpeed;

        [Header("- Airborne")]

        [Tooltip("True일 경우 현재 리지드바디 속도를 사용하여 점프 거리에 영향을 미침")]
        public bool jumpWithRigidbodyForce = false;
        [Tooltip("공중에 떠 있는 동안 회전 여부")]
        public bool jumpAndRotate = true;
        [Tooltip("캐릭터가 점프하는 시간")]
        public float jumpTimer = 0.3f;
        [Tooltip("추가 점프 높이를 추가합니다. 루트 모션으로만 점프하려면 값을 0으로 둡니다.")]
        public float jumpHeight = 4f;
        [Tooltip("공중에 떠 있는 동안 캐릭터가 움직이는 속도")]
        public float airSpeed = 5f;
        [Tooltip("공중에 떠 있는 동안 방향의 부드러움")]
        public float airSmooth = 6f;
        [Tooltip("캐릭터가 Grounded 되지 않은 경우 추가 중력 적용")]
        public float extraGravity = -10f;
        [HideInInspector]
        public float limitFallVelocity = -15f;

        [Header("- Ground")]
        [Tooltip("캐릭터가 걸을 수 있는 레이어")]
        public LayerMask groundLayer = 1 << 0;
        [Tooltip("Grounded 되지 않은 거리")]
        public float groundMinDistance = 0.25f;
        public float groundMaxDistance = 0.5f;
        [Tooltip("최대 Walk 각도")]
        [Range(30, 80)] public float slopeLimit = 75f;

        [Header("- Action")]
        public float dashTimer = 1f;
        public float attackTimer = 1f;
        public float skillQTimer = 0.3f;


        #endregion

        #region Components

        internal Animator animator;
        internal Rigidbody _rigidbody;                                                      // access the Rigidbody component
        internal PhysicMaterial frictionPhysics, maxFrictionPhysics, slippyPhysics;         // create PhysicMaterial for the Rigidbody
        internal CapsuleCollider _capsuleCollider;                                          // access CapsuleCollider information

        #endregion

        #region Internal Variables

        // movement bools
        internal bool isJumping;
        internal bool isDashing;
        internal bool isAttacking;
        internal bool isSkillQ;
        internal bool isStrafing
        {
            get
            {
                return _isStrafing;
            }
            set
            {
                _isStrafing = value;
            }
        }
        internal bool isGrounded { get; set; }
        internal bool isSprinting { get; set; }
        public bool stopMove { get; set; }

        internal float inputMagnitude;                      // 애니메이터 컨트롤러에서 애니메이션을 업데이트하도록 inputMagnitude를 설정합니다.
        internal float verticalSpeed;                       // verticalInput을 기반으로 verticalSpeed를 설정합니다.
        internal float horizontalSpeed;                     // horizontalInput을 기반으로 horizontalSpeed를 설정합니다.       
        internal float moveSpeed;                           // MoveCharacter 메서드에 대한 현재 moveSpeed 설정
        internal float verticalVelocity;                    // Rigidbody 수직 속도 설정
        internal float colliderRadius, colliderHeight;      // capsule collider 추가 정보 저장        
        internal float heightReached;                       // 캐릭터가 공중에서 도달한 최대 높이
        internal float jumpCounter;                         // 점프를 재설정하는 루틴을 계산하는 데 사용
        internal float dashCounter;                         // 대시를 재설정하는 루틴을 계산하는 데 사용
        internal float attackCounter;                       // 공격을 재설정하는 루틴을 계산하는 데 사용
        internal float skillQCounter;
        internal float groundDistance;                      // 지면과의 거리를 알기 위해 사용
        internal RaycastHit groundHit;                      // 땅에 닿는 레이캐스트
        internal bool lockMovement = false;                 // 컨트롤러의 움직임을 잠급니다(애니메이션 아님).
        internal bool lockRotation = false;                 // 컨트롤러의 회전을 잠급니다(애니메이션 아님).  
        internal bool _isStrafing;                          // strafe movement를 설정하는 데 내부적으로 사용됨              
        internal Transform rotateTarget;                    // camera.transform에 대한 일반 참조로 사용됩니다.
        internal Vector3 input;                             // 컨트롤러에 대한 입력 생성
        internal Vector3 colliderCenter;                    // Capsule Collider 정보의 중심을 저장합니다.              
        internal Vector3 inputSmooth;                       // inputSmooth 값을 기반으로 부드러운 입력 생성     
        internal Vector3 moveDirection;                     // 플레이어의 움직이는 방향을 아는 데 사용

        #endregion

        public void Init()
        {
            animator = GetComponent<Animator>();
            animator.updateMode = AnimatorUpdateMode.AnimatePhysics;

            // 벽과 가장자리를 통해 캐릭터를 슬라이드
            frictionPhysics = new PhysicMaterial();
            frictionPhysics.name = "frictionPhysics";
            frictionPhysics.staticFriction = .25f;
            frictionPhysics.dynamicFriction = .25f;
            frictionPhysics.frictionCombine = PhysicMaterialCombine.Multiply;

            // Collider가 경사로에서 미끄러지는 것을 방지합니다.
            maxFrictionPhysics = new PhysicMaterial();
            maxFrictionPhysics.name = "maxFrictionPhysics";
            maxFrictionPhysics.staticFriction = 1f;
            maxFrictionPhysics.dynamicFriction = 1f;
            maxFrictionPhysics.frictionCombine = PhysicMaterialCombine.Maximum;

            // 공기 물리학
            slippyPhysics = new PhysicMaterial();
            slippyPhysics.name = "slippyPhysics";
            slippyPhysics.staticFriction = 0f;
            slippyPhysics.dynamicFriction = 0f;
            slippyPhysics.frictionCombine = PhysicMaterialCombine.Minimum;

            // Rigidbody 정보 
            _rigidbody = GetComponent<Rigidbody>();

            // capsule collider 정보
            _capsuleCollider = GetComponent<CapsuleCollider>();

            // collider preferences 저장
            colliderCenter = GetComponent<CapsuleCollider>().center;
            colliderRadius = GetComponent<CapsuleCollider>().radius;
            colliderHeight = GetComponent<CapsuleCollider>().height;

            isGrounded = true;
        }

        public virtual void UpdateMotor()
        {
            CheckGround();
            CheckSlopeLimit();
            ControlAttackBehaviour();
            ControlJumpBehaviour();
            ControlDashBehaviour();
            AirControl();
        }

        #region Locomotion

        public virtual void SetControllerMoveSpeed(vMovementSpeed speed)
        {
            if (speed.walkByDefault)
                moveSpeed = Mathf.Lerp(moveSpeed, isSprinting ? speed.runningSpeed : speed.walkSpeed, speed.movementSmooth * Time.deltaTime);
            else
                moveSpeed = Mathf.Lerp(moveSpeed, isSprinting ? speed.sprintSpeed : speed.runningSpeed, speed.movementSmooth * Time.deltaTime);
        }

        public virtual void MoveCharacter(Vector3 _direction)
        {
            // calculate input smooth
            inputSmooth = Vector3.Lerp(inputSmooth, input, (isStrafing ? strafeSpeed.movementSmooth : freeSpeed.movementSmooth) * Time.deltaTime);

            if (!isGrounded || isJumping) return;

            _direction.y = 0;
            _direction.x = Mathf.Clamp(_direction.x, -1f, 1f);
            _direction.z = Mathf.Clamp(_direction.z, -1f, 1f);
            // limit the input
            if (_direction.magnitude > 1f)
                _direction.Normalize();

            Vector3 targetPosition = (useRootMotion ? animator.rootPosition : _rigidbody.position) + _direction * (stopMove ? 0 : moveSpeed) * Time.deltaTime;
            Vector3 targetVelocity = (targetPosition - transform.position) / Time.deltaTime;

            bool useVerticalVelocity = true;
            if (useVerticalVelocity) targetVelocity.y = _rigidbody.velocity.y;
            _rigidbody.velocity = targetVelocity;
        }


        public virtual void CheckSlopeLimit()
        {
            if (input.sqrMagnitude < 0.1) return;

            RaycastHit hitinfo;
            var hitAngle = 0f;

            if (Physics.Linecast(transform.position + Vector3.up * (_capsuleCollider.height * 0.5f), transform.position + moveDirection.normalized * (_capsuleCollider.radius + 0.2f), out hitinfo, groundLayer))
            {
                hitAngle = Vector3.Angle(Vector3.up, hitinfo.normal);

                var targetPoint = hitinfo.point + moveDirection.normalized * _capsuleCollider.radius;
                if ((hitAngle > slopeLimit) && Physics.Linecast(transform.position + Vector3.up * (_capsuleCollider.height * 0.5f), targetPoint, out hitinfo, groundLayer))
                {
                    hitAngle = Vector3.Angle(Vector3.up, hitinfo.normal);

                    if (hitAngle > slopeLimit && hitAngle < 85f)
                    {
                        stopMove = true;
                        return;
                    }
                }
            }
            stopMove = false;
        }

        public virtual void RotateToPosition(Vector3 position)
        {
            Vector3 desiredDirection = position - transform.position;
            RotateToDirection(desiredDirection.normalized);
        }

        public virtual void RotateToDirection(Vector3 direction)
        {
            RotateToDirection(direction, isStrafing ? strafeSpeed.rotationSpeed : freeSpeed.rotationSpeed);
        }

        public virtual void RotateToDirection(Vector3 direction, float rotationSpeed)
        {
            if (!jumpAndRotate && !isGrounded) return;
            direction.y = 0f;
            Vector3 desiredForward = Vector3.RotateTowards(transform.forward, direction.normalized, rotationSpeed * Time.deltaTime, .1f);
            Quaternion _newRotation = Quaternion.LookRotation(desiredForward);
            transform.rotation = _newRotation;
        }

        #endregion

        #region Attack Methods
        protected virtual void ControlAttackBehaviour()
        {
            if (!isAttacking) return;
            
            attackCounter -= Time.deltaTime;
            stopMove = true;
            if (attackCounter <= 0)
            {
                attackCounter = 0;
                isAttacking = false;
                stopMove = false;
                animator.ResetTrigger("Attack");
            }
        }
        #endregion


        #region Dash Methods
        protected virtual void ControlDashBehaviour()
        {
            if (!isDashing) return;

            dashCounter -= Time.deltaTime;
            if(dashCounter <= 0)
            {
                dashCounter = 0;
                isDashing = false;
            }
        }
        #endregion
        #region Jump Methods

        protected virtual void ControlJumpBehaviour()
        {
            if (!isJumping) return;

            jumpCounter -= Time.deltaTime;
            if (jumpCounter <= 0)
            {
                jumpCounter = 0;
                isJumping = false;
            }
            // apply extra force to the jump height   
            var vel = _rigidbody.velocity;
            vel.y = jumpHeight;
            _rigidbody.velocity = vel;
        }

        public virtual void AirControl()
        {
            if ((isGrounded && !isJumping)) return;
            if (transform.position.y > heightReached) heightReached = transform.position.y;
            inputSmooth = Vector3.Lerp(inputSmooth, input, airSmooth * Time.deltaTime);

            if (jumpWithRigidbodyForce && !isGrounded)
            {
                _rigidbody.AddForce(moveDirection * airSpeed * Time.deltaTime, ForceMode.VelocityChange);
                return;
            }

            moveDirection.y = 0;
            moveDirection.x = Mathf.Clamp(moveDirection.x, -1f, 1f);
            moveDirection.z = Mathf.Clamp(moveDirection.z, -1f, 1f);

            Vector3 targetPosition = _rigidbody.position + (moveDirection * airSpeed) * Time.deltaTime;
            Vector3 targetVelocity = (targetPosition - transform.position) / Time.deltaTime;

            targetVelocity.y = _rigidbody.velocity.y;
            _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, targetVelocity, airSmooth * Time.deltaTime);
        }

        protected virtual bool jumpFwdCondition
        {
            get
            {
                Vector3 p1 = transform.position + _capsuleCollider.center + Vector3.up * -_capsuleCollider.height * 0.5F;
                Vector3 p2 = p1 + Vector3.up * _capsuleCollider.height;
                return Physics.CapsuleCastAll(p1, p2, _capsuleCollider.radius * 0.5f, transform.forward, 0.6f, groundLayer).Length == 0;
            }
        }

        #endregion

        #region Ground Check                

        protected virtual void CheckGround()
        {
            CheckGroundDistance();
            ControlMaterialPhysics();

            if (groundDistance <= groundMinDistance)
            {
                isGrounded = true;
                if (!isJumping && groundDistance > 0.05f)
                    _rigidbody.AddForce(transform.up * (extraGravity * 2 * Time.deltaTime), ForceMode.VelocityChange);

                heightReached = transform.position.y;
            }
            else
            {
                if (groundDistance >= groundMaxDistance)
                {
                    // set IsGrounded to false 
                    isGrounded = false;
                    // check vertical velocity
                    verticalVelocity = _rigidbody.velocity.y;
                    // apply extra gravity when falling
                    if (!isJumping)
                    {
                        _rigidbody.AddForce(transform.up * extraGravity * Time.deltaTime, ForceMode.VelocityChange);
                    }
                }
                else if (!isJumping)
                {
                    _rigidbody.AddForce(transform.up * (extraGravity * 2 * Time.deltaTime), ForceMode.VelocityChange);
                }
            }
        }

        protected virtual void ControlMaterialPhysics()
        {
            // change the physics material to very slip when not grounded
            _capsuleCollider.material = (isGrounded && GroundAngle() <= slopeLimit + 1) ? frictionPhysics : slippyPhysics;

            if (isGrounded && input == Vector3.zero)
                _capsuleCollider.material = maxFrictionPhysics;
            else if (isGrounded && isAttacking)
                _capsuleCollider.material = maxFrictionPhysics;
            else if (isGrounded && input != Vector3.zero)
                _capsuleCollider.material = frictionPhysics;
            else
                _capsuleCollider.material = slippyPhysics;
        }

        protected virtual void CheckGroundDistance()
        {
            if (_capsuleCollider != null)
            {
                // radius of the SphereCast
                float radius = _capsuleCollider.radius * 0.9f;
                var dist = 10f;
                // ray for RayCast
                Ray ray2 = new Ray(transform.position + new Vector3(0, colliderHeight / 2, 0), Vector3.down);
                // raycast for check the ground distance
                if (Physics.Raycast(ray2, out groundHit, (colliderHeight / 2) + dist, groundLayer) && !groundHit.collider.isTrigger)
                    dist = transform.position.y - groundHit.point.y;
                // sphere cast around the base of the capsule to check the ground distance
                if (dist >= groundMinDistance)
                {
                    Vector3 pos = transform.position + Vector3.up * (_capsuleCollider.radius);
                    Ray ray = new Ray(pos, -Vector3.up);
                    if (Physics.SphereCast(ray, radius, out groundHit, _capsuleCollider.radius + groundMaxDistance, groundLayer) && !groundHit.collider.isTrigger)
                    {
                        Physics.Linecast(groundHit.point + (Vector3.up * 0.1f), groundHit.point + Vector3.down * 0.15f, out groundHit, groundLayer);
                        float newDist = transform.position.y - groundHit.point.y;
                        if (dist > newDist) dist = newDist;
                    }
                }
                groundDistance = (float)System.Math.Round(dist, 2);
            }
        }

        public virtual float GroundAngle()
        {
            var groundAngle = Vector3.Angle(groundHit.normal, Vector3.up);
            return groundAngle;
        }

        public virtual float GroundAngleFromDirection()
        {
            var dir = isStrafing && input.magnitude > 0 ? (transform.right * input.x + transform.forward * input.z).normalized : transform.forward;
            var movementAngle = Vector3.Angle(dir, groundHit.normal) - 90;
            return movementAngle;
        }

        #endregion

        [System.Serializable]
        public class vMovementSpeed
        {
            [Range(1f, 20f)]
            public float movementSmooth = 6f;
            [Range(0f, 1f)]
            public float animationSmooth = 0.2f;
            [Tooltip("캐릭터의 회전 속도")]
            public float rotationSpeed = 16f;
            [Tooltip("캐릭터는 Run 대신 걷기로 움직임을 제한")]
            public bool walkByDefault = false;
            [Tooltip("Idle 상태일 때 카메라를 앞으로 회전")]
            public bool rotateWithCamera = false;
            [Tooltip("RootMotion을 사용하는 경우 Rigidbody 또는 extra speed를 사용하여 Walk 속도")]
            public float walkSpeed = 2f;
            [Tooltip("RootMotion을 사용하는 경우 Rigidbody 또는 extra speed를 사용하여 Run 속도")]
            public float runningSpeed = 4f;
            [Tooltip("RootMotion을 사용하는 경우 Rigidbody 또는 extra speed를 사용하여 Sprint하는 속도")]
            public float sprintSpeed = 6f;
        }
    }
}