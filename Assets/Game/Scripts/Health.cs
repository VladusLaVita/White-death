using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health preferences")]
    public float maxHealth = 100f;

    public float currentHealth;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private EntityController entityController;


    void Start()
    {
        currentHealth = maxHealth; // ← должно быть так
    }

    public void GetDamage(float damage, Vector3 forceDir, Vector3 impactPoint)
    {
        currentHealth -= damage;
        Debug.Log($"Здоровья осталось: {currentHealth}");

        if (currentHealth <= 0)
        {
            Debug.Log("Вызываю Die()");
            if (gameObject.CompareTag("Player"))
                playerController.Die();
            else if (gameObject.CompareTag("Entity"))
                entityController.Die(forceDir, impactPoint);
        }
    }
}
