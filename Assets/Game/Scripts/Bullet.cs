using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 100f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Health>(out Health hp))
        {
            hp.GetDamage(damage);
            Debug.Log($"{collision.gameObject.name}");
        }
    }
}
