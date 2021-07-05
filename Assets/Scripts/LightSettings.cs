using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSettings : MonoBehaviour
{

    Light light_;
    Settings settings_;
    Settings.shadowquality ActualResolution, SettingsResolution;


    // Start is called before the first frame update
    void Start()
    {
        settings_ = Settings.Instance;
        light_ = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        SettingsResolution = settings_.currentShadowQuality;
        Debug.Log("hi");
        light_.shadowResolution = UnityEngine.Rendering.LightShadowResolution.FromQualitySettings;

        if (ActualResolution != SettingsResolution)
        {
            Debug.Log("Activate");
            switch (settings_.currentShadowQuality)
            {
                case Settings.shadowquality.low:
                    light_.shadowResolution = UnityEngine.Rendering.LightShadowResolution.Low;
                    ActualResolution = Settings.shadowquality.low;
                    Debug.Log("SwitchLow");

                    break;
                case Settings.shadowquality.med:
                    light_.shadowResolution = UnityEngine.Rendering.LightShadowResolution.Medium;
                    ActualResolution = Settings.shadowquality.med;
                    Debug.Log("SwitchMed");

                    break;
                case Settings.shadowquality.hig:
                    light_.shadowResolution = UnityEngine.Rendering.LightShadowResolution.High;
                    ActualResolution = Settings.shadowquality.hig;
                    Debug.Log("SwitchHig");

                    break;
                case Settings.shadowquality.ult:
                    light_.shadowResolution = UnityEngine.Rendering.LightShadowResolution.VeryHigh;
                    ActualResolution = Settings.shadowquality.ult;
                    Debug.Log("SwitchUlt");

                    break;
            }
        }

    }
}
