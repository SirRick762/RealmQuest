﻿using Cinemachine;
using KBCore.Refs;
using System;
using UnityEngine;

namespace Plataformer
{
    public class PlayerController: ValidatedMonoBehaviour 
    {
        [Header("References")]
        [SerializeField,Self] CharacterController controller;
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
            
        }

        void Start() => input.EnablePlayerActions();

        void Update()
        {
            HandleMovement();
            UpdateAnimator();
        }

        void UpdateAnimator()
        {
            animator.SetFloat(Speed,currentSpeed);
        }

        void HandleMovement()
        {

            var movementDirection = new Vector3(input.Direction.x, 0f, input.Direction.y).normalized;
            //Rotate movement direction to match camera rotation
            var adjustedDirection = Quaternion.AngleAxis(mainCam.eulerAngles.y, Vector3.up) * movementDirection;
            if(adjustedDirection.magnitude > ZeroF)
            {
                HandleRotation(adjustedDirection);
                HandleCharacterController(adjustedDirection);
                SmoothSpeed(adjustedDirection.magnitude);

            }
            else {

                SmoothSpeed(ZeroF);

            }

        }

         void HandleCharacterController(Vector3 adjustedDirection)
        {
            //Mexer o player
            var adjustedMovement = adjustedDirection * (moveSpeed * Time.deltaTime);
            controller.Move(adjustedMovement);
            
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
