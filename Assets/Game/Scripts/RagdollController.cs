using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [Header("Настройки")]
    [Tooltip("Множитель силы удара")]
    [SerializeField] private float forceMultiplier = 5f;

    [Header("Ссылки")]
    [Tooltip("Аниматор персонажа. Если не указан, будет найден автоматически")]
    [SerializeField] private Animator characterAnimator;

    private Rigidbody[] allRigidbodies;
    public bool isRagdollActive = false;

    private void Awake()
    {
        if (characterAnimator == null)
            characterAnimator = GetComponent<Animator>();

        // Собираем все Rigidbody в иерархии
        allRigidbodies = GetComponentsInChildren<Rigidbody>();

        // Изначально отключаем физику, чтобы анимация/управление работали корректно
        SetRagdollPhysics(false);
    }

    /// <summary>
    /// Основной метод для вызова из других скриптов.
    /// Пробуждает физику, отключает анимацию и применяет импульс.
    /// </summary>
    /// <param name="impactPoint">Точка приложения силы (в мировых координатах)</param>
    /// <param name="forceDirection">Направление силы</param>
    /// <param name="forceAmount">Мощность удара</param>
    public void ActivateRagdoll(Vector3 impactPoint, Vector3 forceDirection, float forceAmount)
    {
        if (isRagdollActive) return;

        isRagdollActive = true;

        // Отключаем аниматор, чтобы он не переписывал позы
        if (characterAnimator != null)
            characterAnimator.enabled = false;

        // Включаем физику
        SetRagdollPhysics(true);

        // Применяем силу ко всем частям тела
        foreach (var rb in allRigidbodies)
        {
            if (rb != null)
            {
                rb.AddForceAtPosition(
                    forceDirection.normalized * forceAmount * forceMultiplier,
                    impactPoint,
                    ForceMode.Impulse
                );
            }
        }
    }

    /// <summary>
    /// Упрощённая версия: просто включает рэгдолл без приложения силы.
    /// </summary>
    public void ActivateRagdoll()
    {
        ActivateRagdoll(transform.position, Vector3.up, 0f);
    }

    /// <summary>
    /// Отключает рэгдолл и возвращает объект в анимируемое/управляемое состояние.
    /// </summary>
    public void DeactivateRagdoll()
    {
        if (!isRagdollActive) return;

        isRagdollActive = false;
        SetRagdollPhysics(false);

        if (characterAnimator != null)
            characterAnimator.enabled = true;
    }

    private void SetRagdollPhysics(bool enabled)
    {
        foreach (var rb in allRigidbodies)
        {
            if (rb == null) continue;

            rb.isKinematic = !enabled;
            rb.useGravity = enabled;
            // Для стабильной физики при высоких скоростях лучше использовать Continuous
            rb.collisionDetectionMode = enabled ? CollisionDetectionMode.Continuous : CollisionDetectionMode.Discrete;
        }
    }
}