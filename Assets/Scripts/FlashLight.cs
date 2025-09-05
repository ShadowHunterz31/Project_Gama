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

    [Header("Áudio da Lanterna")]
    public AudioSource audioSource;
    public AudioClip soundOn;
    public AudioClip soundOff;

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

            // Toca o som de ligar ou desligar
            if (audioSource != null)
            {
                if (flashlightOn && soundOn != null)
                    audioSource.PlayOneShot(soundOn);
                else if (!flashlightOn && soundOff != null)
                    audioSource.PlayOneShot(soundOff);
            }
        }

        // Faz o fade do range da luz fraca
        float targetRange = flashlightOn ? weakLightMinRange : weakLightMaxRange;
        weakLight.range = Mathf.Lerp(weakLight.range, targetRange, Time.deltaTime * fadeSpeed);
    }
}
