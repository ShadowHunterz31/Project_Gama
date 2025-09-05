using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSound : MonoBehaviour
{
    [Header("Loop de Ambiente")]
    public AudioSource ambientLoop; // som contínuo (vento, ruído)

    [Header("Sons Aleatórios")]
    public AudioSource randomSource; 
    public AudioClip[] randomClips; // sons aleatórios (rangidos, portas, etc.)
    public float minDelay = 5f;     // tempo mínimo entre sons
    public float maxDelay = 15f;    // tempo máximo entre sons
    private float timer;

    void Start()
    {
        if (ambientLoop != null)
            ambientLoop.Play();

        ResetTimer();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f && randomClips.Length > 0)
        {
            // Escolhe um som aleatório
            AudioClip clip = randomClips[Random.Range(0, randomClips.Length)];
            randomSource.PlayOneShot(clip);

            ResetTimer();
        }
    }

    void ResetTimer()
    {
        timer = Random.Range(minDelay, maxDelay);
    }
}
