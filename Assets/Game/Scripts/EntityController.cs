using UnityEngine;
using UnityEngine.AI;

public class EntityController : MonoBehaviour
{
    public bool deadFlag = false;
    public RagdollController ragdollController;


    public void Die(Vector3 forceDir, Vector3 impactPoint)
    {
        Debug.Log("ActivateRagdoll вызван");

        // Останавливаем NavMeshAgent
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        // Останавливаем EnemyAI
        EnemyAI ai = GetComponent<EnemyAI>();
        if (ai != null) ai.enabled = false;

        ragdollController.ActivateRagdoll(impactPoint, forceDir, 10f);
    }
}
