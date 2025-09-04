using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    public Light flashlight;      // A lanterna (Spot Light)
    public Light weakLight;       // A luz fraca do player
    public float fadeSpeed = 4f;  // Velocidade do fade
    private float weakLightMinRange = 4f;  // Range mínimo da luz fraca
    private float weakLightMaxRange = 6f;  // Range máximo da luz fraca

    private bool flashlightOn = false;

    void Start()
    {
        flashlight.enabled = false;
        weakLight.enabled = true;
        weakLight.range = weakLightMaxRange; // range inicial da luz fraca
    }

    void Update()
    {
        // Alterna a lanterna ao apertar F
        if (Input.GetKeyDown(KeyCode.F))
        {
            flashlightOn = !flashlightOn;
            flashlight.enabled = flashlightOn;
        }

        // Faz o fade do range da luz fraca
        float targetRange = flashlightOn ? weakLightMinRange : weakLightMaxRange;
        weakLight.range = Mathf.Lerp(weakLight.range, targetRange, Time.deltaTime * fadeSpeed);
    }

}
