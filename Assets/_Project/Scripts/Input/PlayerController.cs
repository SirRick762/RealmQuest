using Cinemachine;
using KBCore.Refs;
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Plataformer
{
    public class PlayerController: ValidatedMonoBehaviour 
    {
        [Header("References")]
        //[SerializeField,Self] CharacterController controller;
        [SerializeField,Self] Rigidbody rb;
        [SerializeField, Self] Animator animator;
        [SerializeField, Anywhere] CinemachineFreeLook freeLookVCam;
        [SerializeField, Anywhere] InputReader input;

        [Header("Settings")]
        [SerializeField] float moveSpeed = 6f;
        [SerializeField] float rotationSpeed = 15f;
        [SerializeField] float smoothTime = 0.2f;

        const float ZeroF = 0f;

        Transform mainCam;

        float currentSpeed;
        float velocity;

        Vector3 movement;

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
            
        }

        void Start() => input.EnablePlayerActions();

        void Update()
        {
            movement = new Vector3(input.Direction.x, 0f,input.Direction.y);
            
            UpdateAnimator();
        }
        private void FixedUpdate()
        {
            // HandleJump();
            HandleMovement();
        }

        void UpdateAnimator()
        {
            animator.SetFloat(Speed,currentSpeed);
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
