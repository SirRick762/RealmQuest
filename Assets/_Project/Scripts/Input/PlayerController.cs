using System.Collections.Generic;
using Cinemachine;
using KBCore.Refs;
using Platformer;
using UnityEngine;
using Utilities;

namespace Plataformer
{
    public class PlayerController: ValidatedMonoBehaviour 
    {
        [Header("References")]
        [SerializeField, Self] Rigidbody rb;
        [SerializeField, Self] GroundChecker groundChecker;
        [SerializeField, Self] Animator animator;
        [SerializeField, Anywhere] CinemachineFreeLook freeLookVCam;
        [SerializeField, Anywhere] InputReader input;

        [Header("Movement Settings")]
        [SerializeField] float moveSpeed = 6f;
        [SerializeField] float rotationSpeed = 15f;
        [SerializeField] float smoothTime = 0.2f;

        [Header("Jump Settings")]
        [SerializeField] float jumpForce = 10f;
        [SerializeField] float jumpDuration = 0.5f;
        [SerializeField] float jumpCooldown = 0f;
        [SerializeField] float jumpMaxHeight = 2f;
        [SerializeField] float gravityMultiplier = 3f;

        const float ZeroF = 0f;

        Transform mainCam;

        float currentSpeed;
        float velocity;
        float jumpVelocity;

        Vector3 movement;

        List<Timer> timers;
        CountdownTimer jumpTimer;
        CountdownTimer jumpCooldownTimer;

        //Animator parameters
        static readonly int Speed = Animator.StringToHash("Speed");



        void Awake()
        {
            mainCam = Camera.main.transform;
            freeLookVCam.Follow = transform;
            freeLookVCam.LookAt = transform;
            //Invoke event when observed transform os teleported, adjusting freeLookVCam's position accordingly
            freeLookVCam.OnTargetObjectWarped(
                transform,
                transform.position - freeLookVCam.transform.position - Vector3.forward
                );

            rb.freezeRotation = true;

            jumpTimer = new CountdownTimer(jumpDuration);
            jumpCooldownTimer = new CountdownTimer(jumpCooldown);
            timers = new List<Timer>(capacity: 2) { jumpTimer, jumpCooldownTimer };

            jumpTimer.OnTimerStop += () => jumpCooldownTimer.Start();
        }

        void Start() => input.EnablePlayerActions();

        void OnEnable()
        {
            input.Jump += OnJump;  
        }

        void OnDisable()
        {
            input.Jump -= OnJump;
        }

        void OnJump(bool performed)
        {
            if(performed && !jumpTimer.IsRunning && !jumpCooldownTimer.IsRunning && groundChecker.IsGrounded) 
            {
            
                jumpTimer.Start();

            }
            else if(!performed && jumpTimer.IsRunning) {
            
            jumpTimer.Stop();
            
            }

        }

        void Update()
        {
            movement = new Vector3(input.Direction.x, 0f,input.Direction.y);

            HandleTimers();
            UpdateAnimator();
        }
        private void FixedUpdate()
        {
            HandleJump();
            HandleMovement();
        }

        void UpdateAnimator()
        {
            animator.SetFloat(Speed,currentSpeed);
        }

        void HandleTimers()
        {
            foreach(var timer in timers)
            {
                timer.Tick(Time.deltaTime);
            }
        }

        void HandleJump()
        {
            if(!jumpTimer.IsRunning && groundChecker.IsGrounded) {
                jumpVelocity = ZeroF;
                jumpTimer.Stop();
                return;
            }

            if (jumpTimer.IsRunning)
            {
                float launchPoint = 0.9f;
                if(jumpTimer.Progress > launchPoint)
                {
                    jumpVelocity = Mathf.Sqrt(2 * jumpMaxHeight * Mathf.Abs(Physics.gravity.y));
                }
                else {

                    jumpVelocity += (1 - jumpTimer.Progress) * jumpForce * Time.fixedDeltaTime;
                }
            }

            else
            {
               jumpVelocity += Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime;
            }

            rb.velocity = new Vector3(rb.velocity.x, jumpVelocity, rb.velocity.z);
        }

        void HandleMovement()
        {

            
            //Rotate movement direction to match camera rotation
            var adjustedDirection = Quaternion.AngleAxis(mainCam.eulerAngles.y, Vector3.up) * movement;
            if(adjustedDirection.magnitude > ZeroF)
            {
                HandleRotation(adjustedDirection);
                HandleHorizontalController(adjustedDirection);
                SmoothSpeed(adjustedDirection.magnitude);

            }
            else {

                SmoothSpeed(ZeroF);

                //Reset horizontal velocity for a snappy stop
                rb.velocity = new Vector3(ZeroF,rb.velocity.y,ZeroF);

            }

        }

         void HandleHorizontalController(Vector3 adjustedDirection)
        {
            //Mexer o player
            Vector3 velocity = adjustedDirection * moveSpeed *Time.fixedDeltaTime;
            rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
            
        }

        void HandleRotation(Vector3 adjustedDirection)
        {
            //Adjust rotation to match camera direction
            var targetRotation = Quaternion.LookRotation(adjustedDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.LookAt(transform.position + adjustedDirection);
        }

        void SmoothSpeed(float value)
        {
            currentSpeed = Mathf.SmoothDamp(currentSpeed, value, ref velocity, smoothTime);
        }
    }
}
