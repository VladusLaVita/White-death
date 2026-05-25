using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health preferences")]
    public float maxHealth = 100f;

    [SerializeField]
    private float currentHealth;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private EntityController entityController;


    void Start()
    {
        maxHealth = currentHealth;
    }

    public void GetDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            if (gameObject.CompareTag("Player")) playerController.deadFlag = true;
            else if (gameObject.CompareTag("Entity")) entityController.deadFlag = true;
            else Destroy(gameObject);
        }

    }

    public void GetHealth(float health)
    {
        GetDamage(-health);
    }
    public void Die()
    {
        if (playerController != null) playerController.Die();
        else entityController.Die();
    }
}
