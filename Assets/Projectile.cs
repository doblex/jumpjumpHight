using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float projectileLife;
    [SerializeField] float projectileSpeed;
    [SerializeField] int projectileDamage;
    [SerializeField] int maxBounces;

    int currentBounces;

    Rigidbody2D rb;
    Vector2 lastVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.AddForce(transform.right * projectileSpeed, ForceMode2D.Impulse);
        Destroy(gameObject, projectileLife);
    }

    private void Update()
    {
        lastVelocity = rb.linearVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            player.DoDamage(projectileDamage);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("wall") || collision.gameObject.CompareTag("platform"))
        {
            if (currentBounces >= maxBounces)
            {
                Destroy(gameObject);
                return;
            }

            Vector2 inNormal = collision.contacts[0].normal;
            Vector2 reflectDir = Vector2.Reflect(lastVelocity.normalized, inNormal);

            rb.linearVelocity = Vector2.zero;

            if(currentBounces > 0)
                rb.AddForce(reflectDir * lastVelocity.magnitude / currentBounces, ForceMode2D.Impulse);
            else
                rb.AddForce(reflectDir * lastVelocity.magnitude, ForceMode2D.Impulse);

            currentBounces++;
        }
    }

}
