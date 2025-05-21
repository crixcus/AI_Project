using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIScript : MonoBehaviour
{
    [Header("AI Settings")]
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float maxHP = 10f;
    [SerializeField] private float stoppingDistance = 0f;
    [SerializeField] private float rotateSpeed = 5f;

    private float currentHP;
    private NavMeshAgent agent;
    private Transform player;

    private bool playerInRange = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.stoppingDistance = stoppingDistance;

        agent.updateRotation = false;
        agent.angularSpeed = 0;

        currentHP = maxHP;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("No object tagged 'Player' found.");
        }

    }

    void Update()
    {
        if (player != null && agent.enabled)
        {
            if (!playerInRange)
            {
                // Move toward the player if not in attack range
                agent.SetDestination(player.position);
            }
            else
            {
                // Stop moving when in attack range
                agent.SetDestination(transform.position);
            }

            Vector3 direction = agent.steeringTarget - transform.position;
            direction.y = 0;

            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
            }

            Debug.DrawLine(transform.position, agent.steeringTarget, Color.green);
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
