using Cinemachine;
using KBCore.Refs;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Plataformer
{
    public class CameraManager : ValidatedMonoBehaviour {


        [Header("References")]
        [SerializeField, Anywhere] CinemachineFreeLook freeLookVCam;
        [SerializeField, Anywhere] InputReader input;

        [Header("Settings")]

        [SerializeField, Range(0.5f, 3f)] float speedMultiplier = 1f;

        bool isRMBPressed;
        bool cameraMovementLock;

        void OnEnable()
        {
            input.Look += OnLook;
            input.EnableMouseControlCamera += OnEnableMouseControlCamera;
            input.DisableMouseControlCamera += OnDisableMouseControlCamera;
        }

        void OnDisable()
        {
            input.Look -= OnLook;
            input.EnableMouseControlCamera -= OnEnableMouseControlCamera;
            input.DisableMouseControlCamera -= OnDisableMouseControlCamera;
        }

        void OnLook(Vector2 cameraMovement, bool isDeviceMouse)
        {
            if (cameraMovementLock) return;

            if (isDeviceMouse && !isRMBPressed) return;

            //If the device is mouse use fixedDeltaTime, otherwise use deltaTime

            float deviceMultiplier = isDeviceMouse ? Time.fixedDeltaTime : Time.deltaTime;

            //Set the camera axis values

            freeLookVCam.m_XAxis.m_InputAxisValue = cameraMovement.x * speedMultiplier * deviceMultiplier;
            freeLookVCam.m_YAxis.m_InputAxisValue = cameraMovement.y * speedMultiplier * deviceMultiplier;


        }

        void OnDisableMouseControlCamera()
        {
            isRMBPressed = false;
            //Unlock the cursor to make it visible
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            //Reset the camera axis to prevent jumping when re-enabling mouse control
            freeLookVCam.m_XAxis.m_InputAxisValue = 0f;
            freeLookVCam.m_YAxis.m_InputAxisValue = 0f;

        }

        void OnEnableMouseControlCamera()
        {
            isRMBPressed = true;
            //Lock the cursor to the center of the screen and hide it
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            StartCoroutine(DisableMouseForFrame());
        }

        IEnumerator DisableMouseForFrame()
        {
            cameraMovementLock = true;
            yield return new WaitForEndOfFrame();
            cameraMovementLock = false;

        }
    }
}
