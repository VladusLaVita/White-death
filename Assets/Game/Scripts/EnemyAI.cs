using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Настройки дистанции и обзора")]
    public float detectionRadius = 15f;
    public float patrolRadius = 10f;
    public LayerMask obstacleMask; // Слой препятствий (стен)
    public LayerMask playerMask;   // Слой игрока

    [Header("Настройки скорости")]
    public float patrolSpeed = 2f;  // Скорость при патрулировании
    public float chaseSpeed = 5f;   // Скорость при погоне

    [Header("Компоненты")]
    private NavMeshAgent agent;
    public Transform player;

    [Header("Аниматор и анимации")]
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        // Устанавливаем начальную скорость
        agent.speed = patrolSpeed;
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Проверяем: в радиусе ли игрок И нет ли между нами стен
        if (distanceToPlayer <= detectionRadius && CanSeePlayer())
        {
            ChasePlayer();
        }
        else
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                StartPatrol();
            }
        }

        if (agent.speed > 0 && agent.speed < 1.5)
        {
            animator.SetBool("Walk", false);
        }
        else
        {
            animator.SetBool("Walk", true);
        }
    }

    // Проверка прямой видимости через Raycast
    bool CanSeePlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Пускаем луч от врага к игроку
        // Если луч натыкается на что-то на слое obstacleMask раньше, чем дойдет до игрока — значит там стена
        if (Physics.Raycast(transform.position + Vector3.up, directionToPlayer, out RaycastHit hit, distanceToPlayer, obstacleMask))
        {
            return false; // Видим стену
        }

        return true; // Препятствий нет
    }

    void ChasePlayer()
    {
        agent.speed = chaseSpeed; // Увеличиваем скорость
        agent.SetDestination(player.position);
    }

    void StartPatrol()
    {
        agent.speed = patrolSpeed; // Возвращаем обычную скорость

        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, 1))
        {
            agent.SetDestination(hit.position);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);

        // Рисуем луч до игрока в редакторе для тестов
        if (player != null)
        {
            Gizmos.color = CanSeePlayer() ? Color.green : Color.black;
            Gizmos.DrawLine(transform.position + Vector3.up, player.position);
        }
    }
}