using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int duration = 3;
    private Vector3 direction;
    public void Move()
    {
        if (duration > 0)
        {
            transform.localPosition += direction.normalized;
            duration--;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(Vector3 direction)
    {
        this.direction = direction;
    }
}
