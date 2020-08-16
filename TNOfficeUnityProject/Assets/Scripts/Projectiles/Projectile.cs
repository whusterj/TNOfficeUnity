using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float elapsed = 0f;
    [SerializeField] private float lifetime = 10f;

    void Update()
    {
        // Expire projectiles at the end of lifetime
        elapsed += Time.deltaTime;
        if (elapsed > lifetime)
        {
            Destroy(gameObject);
        }
    }
}
