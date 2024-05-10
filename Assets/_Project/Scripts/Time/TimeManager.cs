using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Sirenix.OdinInspector;

namespace Plataformer
{
    public class TimeManager : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI timeText;

        [SerializeField] Light sun;
        [SerializeField] Light moon;
        [SerializeField] AnimationCurve lightIntesityCurve;
        [SerializeField] float maxSunIntensity = 1;
        [SerializeField] float maxMoonIntensity = 0.5f;
        [SerializeField] Color dayAmbientLight;
        [SerializeField] Color nightAmbientLight;
        [SerializeField] Volume volume;
        [SerializeField] Material skyboxMaterial;

        [SerializeField] RectTransform dial;
        float initialDialRotation;


        ColorAdjustments colorAdjustments;


        [InlineEditor ,SerializeField] TimeSettings timeSettings;

        TimeService service;

        void Start ()
        {
            service = new TimeService(timeSettings);
            volume.profile.TryGet(out colorAdjustments);

            initialDialRotation = dial.rotation.eulerAngles.z;

        }

        void Update ()
        {
            UpdateTimeofDay();
            RotateSun();
            UpdateLightSettings();
            UpdateSkyBlend();

            //if(Input.GetKeyDown(KeyCode.Space)) {
            //    timeSettings.timeMultiplier *= 2;
            //}

            //if(Input.GetKeyDown(KeyCode.LeftShift)) { 
            //    timeSettings.timeMultiplier /= 2;
            //}
        }

        void UpdateSkyBlend()
        {
            float dotProduct = Vector3.Dot(sun.transform.forward, Vector3.up);
            float blend = Mathf.Lerp(0,1,lightIntesityCurve.Evaluate(dotProduct));
            skyboxMaterial.SetFloat("_Blend", blend);

        }
        void UpdateLightSettings()
        {
            float dotProduct = Vector3.Dot(sun.transform.forward, Vector3.down);
            sun.intensity = Mathf.Lerp(0,maxSunIntensity,lightIntesityCurve.Evaluate(dotProduct));
            moon.intensity = Mathf.Lerp(0,maxMoonIntensity, lightIntesityCurve.Evaluate(dotProduct));
            if (colorAdjustments == null) return;
            colorAdjustments.colorFilter.value = Color.Lerp(nightAmbientLight, dayAmbientLight, lightIntesityCurve.Evaluate(dotProduct));
        }
        void RotateSun()
        {
            float rotation = service.CalculateSunAngle();
            sun.transform.rotation = Quaternion.AngleAxis(rotation, Vector3.right);
            dial.rotation = Quaternion.Euler(0,0, rotation+ initialDialRotation);

        }

        void UpdateTimeofDay()
        {
            service.UpdateTime(Time.deltaTime); 
            if(timeText != null )
            {
                timeText.text = service.CurrentTime.ToString("hh:mm");
            }
        }

    }
}
