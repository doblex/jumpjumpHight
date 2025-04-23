using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public abstract class Character : MonoBehaviour
{
    public delegate void OnHealthChange(int current, int maxHealth);
    public event OnHealthChange onHealthChange;

    protected Animator animator;
    protected Rigidbody2D rb;
    protected SpriteRenderer spriteRenderer;
    [Header("Stats")]
    [SerializeField] protected CharacterData characterData;

    [Header("Attack")]
    [SerializeField] protected Transform attackBoxOrigin;
    [SerializeField] protected Vector2 attackBoxDimensions;
    [SerializeField] protected LayerMask AttackLayer;


    protected float attackRecoil;

    protected int health;
    protected float speed;

    protected bool isInvicible;

    protected bool isFacingRight = true;

    protected virtual void Awake()
    {
        Setup();
    }

    private void Start()
    {
        onHealthChange?.Invoke(health, characterData.maxHealth);

        attackRecoil = characterData.attackDuration;
    }

    private void Update()
    {
        Move();
        Action();
        Checks();

        ResetAttack();
    }

    private void ResetAttack()
    {
        if (attackRecoil > 0)
        {
            attackRecoil -= Time.deltaTime;
        }
    }

    private void Setup() 
    {
        if (characterData == null)
        {
            Debug.LogError("CharacterData is not assigned.");
            return;
        }

        health = characterData.maxHealth;
        speed = characterData.speed;

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void DoDamage(int damage)
    {
        if (isInvicible)
            return;

        isInvicible = true;
        StartCoroutine(SetInvicible(false, characterData.immunityFrames));
        health -= damage;

        onHealthChange?.Invoke(health, characterData.maxHealth);

        if (health <= 0)
        {
            Die();
        }
    }

    private IEnumerator SetInvicible(bool isBecomingInvincible, float delay)
    {
        yield return new WaitForSeconds(delay);

        isInvicible = isBecomingInvincible;
    }

    public virtual void Heal(int amount)
    {
        health += amount;
        if (health > characterData.maxHealth)
            health = characterData.maxHealth;

        onHealthChange?.Invoke(health, characterData.maxHealth);
    }

    protected void Flip(float HorizontalInput)
    {
        if (isFacingRight && HorizontalInput < 0 || !isFacingRight && HorizontalInput > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }

    protected void Attack()
    {
        if (attackRecoil <= 0)
        { 
            attackRecoil = characterData.attackDuration;
            animator.SetTrigger("trAttack");

            Collider2D hitCollider = Physics2D.OverlapBox(attackBoxOrigin.position, attackBoxDimensions, 0f, AttackLayer);

            if (hitCollider != null)
            {
               onAttackHit(hitCollider);
            }

        }
    }


    public abstract void onAttackHit(Collider2D hitCollider);
    public abstract void Die();
    public abstract void Move();
    public abstract void Action();
    public abstract void Checks();
}
