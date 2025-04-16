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

    [SerializeField] protected CharacterData characterData;

    protected int health;
    protected float speed;

    protected bool isInvicible;

    protected virtual void Awake()
    {
        Setup();
    }

    private void Start()
    {
        onHealthChange?.Invoke(health, characterData.maxHealth);
    }

    private void Update()
    {
        Move();
        Action();
        Checks();
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
        health -= damage;

        onHealthChange?.Invoke(health, characterData.maxHealth);

        if (health <= 0)
        {
            Die();
        }
    }

    public virtual void Heal(int amount)
    {
        health += amount;
        if (health > characterData.maxHealth)
            health = characterData.maxHealth;

        onHealthChange?.Invoke(health, characterData.maxHealth);
    }

    public abstract void Die();
    public abstract void Move();
    public abstract void Action();
    public abstract void Checks();
}
