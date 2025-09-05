using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pursuer : MonoBehaviour
{
    [Header("Referências")]
    public Transform playerBody;
    public AudioSource footstepSource;
    public AudioClip footstepClip;
    public AudioSource whisperSource;
    public AudioClip whisperClip;

    [Header("Movimento")]
    public float detectionRange = 15f;
    public float stopDistance = 1f;

    [Header("Sussurros")]
    public float maxWhisperDistance = 10f;
    public float minWhisperVolume = 0.1f;
    public float maxWhisperVolume = 1f;

    [Header("Perseguição")]
    public float loseSightDistance = 20f;
    public LayerMask obstructionMask;
    public float randomMoveRadius = 5f;
    public float randomMoveInterval = 3f;

    private NavMeshAgent agent;
    private float footstepTimer = 0f;
    public float footstepInterval = 0.5f;

    private bool isChasing = false;
    private bool isSearching = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (whisperSource != null && whisperClip != null)
        {
            whisperSource.clip = whisperClip;
            whisperSource.loop = true;
            whisperSource.volume = 0f;
            whisperSource.Play();
        }

        StartCoroutine(RandomMovementCoroutine());
    }

    void Update()
    {
        if (playerBody == null) return;

        float distanceToBody = Vector3.Distance(transform.position, playerBody.position);
        bool canSeeBody = CanSeePlayerBody();

        if (distanceToBody <= detectionRange && canSeeBody)
        {
            isChasing = true;
            isSearching = false;
        }
        else if (distanceToBody > loseSightDistance || !canSeeBody)
        {
            isChasing = false;
            isSearching = true;
        }

        // Define alvo do NavMeshAgent
        if (isChasing)
        {
            agent.isStopped = false;
            agent.SetDestination(playerBody.position);
        }
        else if (isSearching)
        {
            agent.isStopped = false;
            agent.SetDestination(agent.destination); // continua indo para ponto aleatório
        }
        else
        {
            agent.isStopped = true;
        }

        // passos
        if (!agent.isStopped && agent.velocity.magnitude > 0.1f)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                footstepSource.PlayOneShot(footstepClip);
                footstepTimer = footstepInterval;
            }
        }

        // volume de sussurros
        if (whisperSource != null)
        {
            float t = isChasing ? Mathf.Clamp01(1 - (distanceToBody / maxWhisperDistance)) : 0.1f;
            whisperSource.volume = Mathf.Lerp(minWhisperVolume, maxWhisperVolume, t);
        }
    }

    private bool CanSeePlayerBody()
    {
        Vector3 dir = (playerBody.position - transform.position).normalized;

        if (Physics.Raycast(transform.position, dir, out RaycastHit hit, loseSightDistance, ~obstructionMask))
        {
            if (hit.collider.CompareTag("PlayerBody"))
                return true;
        }

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBody"))
        {
            Debug.Log("Player capturado!");
            // aqui você pode chamar o método de morte ou reset de cena
        }
    }

    private IEnumerator RandomMovementCoroutine()
    {
        while (true)
        {
            if (isSearching)
            {
                Vector2 randomCircle = Random.insideUnitCircle * randomMoveRadius;
                Vector3 randomTarget = new Vector3(transform.position.x + randomCircle.x, transform.position.y, transform.position.z + randomCircle.y);
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomTarget, out hit, randomMoveRadius, NavMesh.AllAreas))
                {
                    agent.SetDestination(hit.position);
                }
            }

            yield return new WaitForSeconds(randomMoveInterval);
        }
    }
}
