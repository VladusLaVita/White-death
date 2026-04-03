using UnityEngine;

public class VendigoCatch : MonoBehaviour
{
    private Animator animator;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что вошли в триггер именно игрока
        if (other.CompareTag("Player"))
        {
            animator.SetTrigger("Attack");
            Destroy(other.gameObject);

        }
    }
}
