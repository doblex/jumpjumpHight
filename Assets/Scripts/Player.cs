using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Player : Character
{
    public delegate void OnPlayerAction(GameObject player);
    public event OnPlayerAction onPlayerAction;

    [SerializeField] LayerMask platformLayer;

    [SerializeField] KeyCode attack = KeyCode.E;
    [SerializeField] KeyCode jump = KeyCode.Space;
    [SerializeField] KeyCode torch = KeyCode.Q;
    [SerializeField] KeyCode interact = KeyCode.W;

    Light2D torchLight;

    bool isJumping = false;
    bool isFalling = false;

    protected override void Awake()
    {
        base.Awake();
        torchLight = GetComponentInChildren<Light2D>();
    }

    public override void Move()
    {
        float movementDirection = Input.GetAxis("Horizontal");

        if (movementDirection != 0)
        {
            float moveH = movementDirection * speed;

            rb.AddForce(new Vector2(moveH, 0), ForceMode2D.Impulse);
            rb.linearVelocity = new Vector2(Mathf.Clamp(rb.linearVelocity.x, -characterData.maxSpeed, characterData.maxSpeed), rb.linearVelocity.y);

            animator.SetBool("IsMoving", true);

            transform.rotation = Quaternion.Euler(0, movementDirection > 0 ? 0 : 180, 0);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            animator.SetBool("IsMoving", false);
        }
    }

    public override void Action()
    {

        if (Input.GetKeyDown(interact))
        {
            onPlayerAction?.Invoke(gameObject);
        }

        if (Input.GetKeyDown(torch))
        {
            torchLight.enabled = !torchLight.enabled;
        }

        if (Input.GetKeyDown(attack))
        {
            animator.SetTrigger("trAttack");
        }

        if (Input.GetKeyDown(jump) && !isJumping)
        {    
                isJumping = true;
                animator.SetTrigger("trJump");
                animator.SetBool("IsInAir", isJumping);

                rb.AddForce(Vector2.up * characterData.jumpForce, ForceMode2D.Impulse);
        }

        if (Input.GetAxis("Vertical") < 0 && !isJumping)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1, platformLayer);

            if (hit.collider != null)
            {
                animator.SetTrigger("trDown");
                StartCoroutine(DisableCollision(hit.collider, 0.5f));
            }
        }
    }

    public override void Checks()
    {
        if (IsFalling() && !isFalling)
        {
            isFalling = true;
            animator.SetTrigger("trFall");
        }
    }

    public override void DoDamage(int damage)
    {
        base.DoDamage(damage);
        if (!isInvicible)
        { 
            animator.SetTrigger("trHit");
        }
    }

    public override void Die()
    {
        animator.SetTrigger("trDeath");

        //stop game
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("wall") || collision.gameObject.CompareTag("platform"))
        {
            isJumping = false;
            animator.SetBool("IsInAir", isJumping);

            isFalling = false;
        }
    }

    private bool IsFalling()
    {
        bool fall = false;

        if (rb.linearVelocity.y < 0)
            fall = true;

        return fall;
    }

    private IEnumerator DisableCollision(Collider2D collider, float disableTime)
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collider, true);
        yield return new WaitForSeconds(disableTime);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collider, false);
    }
}
