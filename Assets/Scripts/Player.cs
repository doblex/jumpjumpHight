using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Player : Character
{
    public delegate void OnPlayerAction(GameObject player);
    public event OnPlayerAction onPlayerAction;


    [Header("Jump")]
    [SerializeField] KeyCode jump = KeyCode.Space;
    [SerializeField] LayerMask WallLayer;
    [SerializeField] Transform groundCheck;

    [Header("Attack")]
    [SerializeField] KeyCode attack = KeyCode.E;

    [Header("actions")]
    [SerializeField] KeyCode torch = KeyCode.Q;
    Light2D torchLight;

    [SerializeField] KeyCode interact = KeyCode.W;

    [Header("Crouch")]
    [SerializeField] LayerMask platformLayer;


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

            Flip(movementDirection);
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
            Attack();
        }

        if (Input.GetKeyDown(jump) && IsGrounded())
        {    
                isJumping = true;
                animator.SetTrigger("trJump");
                animator.SetBool("IsInAir", isJumping);

            //rb.AddForce(Vector2.up * characterData.jumpForce, ForceMode2D.Impulse);

            rb.linearVelocity = new Vector2(rb.linearVelocityX, characterData.jumpForce);
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

    public override void onAttackHit(Collider2D hitCollider)
    {
        hitCollider.gameObject.GetComponent<Enemy>().DoDamage(characterData.damage);
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

    public bool IsGrounded() 
    {
        bool check = Physics2D.OverlapCircle(groundCheck.position, 0.2f, WallLayer);
        return check; 
    }

    private IEnumerator DisableCollision(Collider2D collider, float disableTime)
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collider, true);
        yield return new WaitForSeconds(disableTime);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collider, false);
    }
}
