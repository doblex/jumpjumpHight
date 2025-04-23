using System.Collections;
using UnityEngine;

public class Enemy : Character
{
    public enum AIState 
    { 
        idle,
        attack,
        dead
    }

    [Header("Stats")]
    [SerializeField] EnemyData enemyData;
        
    AIState currentState;
    GameObject playerObject;

    float positionTimer;

    Vector3 spawnpoint;
    Vector3 targetPosition;

    protected override void Awake()
    {
        base.Awake();

        spawnpoint = targetPosition = transform.position;

        positionTimer = enemyData.updatePosition;

        isFacingRight = false;

        playerObject = GameObject.FindWithTag("Player");
    }

    public override void Action()
    {
        if (Vector3.Distance(playerObject.transform.position, attackBoxOrigin.position) < attackBoxDimensions.x / 2)
        {
            Attack();
        }
    }

    public override void onAttackHit(Collider2D hitCollider)
    {
        hitCollider.gameObject.GetComponent<Player>().DoDamage(characterData.damage);
    }

    public override void Die()
    {
        animator.SetTrigger("trDead");
        StartCoroutine(Disable(animator.GetCurrentAnimatorStateInfo(0).length));
    }

    private IEnumerator Disable(float delay)
    {
        yield return new WaitForSeconds(delay);

        gameObject.SetActive(false);
    }

    public override void Move()
    {

        if (positionTimer <= 0)
        {
            switch (currentState)
            {
                case AIState.idle:
                    SetIdleTarget();
                    break;
                case AIState.attack:
                    SetAttackTarget();
                    break;
            }

            positionTimer = enemyData.updatePosition;
        }

        MoveToTarget();

        positionTimer -= Time.deltaTime;

    }

    private void MoveToTarget()
    {
        if (Vector3.Distance(targetPosition, transform.position) < 0.2f)
        { 
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("IsMoving", false);
        }
        else
        { 
            rb.AddForce((targetPosition - transform.position).normalized * characterData.speed);

            Vector3 linearVelocity = rb.linearVelocity;
            rb.linearVelocity = new Vector3(Mathf.Clamp(linearVelocity.x, -characterData.maxSpeed, characterData.maxSpeed), linearVelocity.y, linearVelocity.z);
        }
    }

    private void SetIdleTarget()
    {
        float randomDirection = Random.Range(-1, 2);
        float randomX = Random.Range(1, enemyData.idleDistance);

        if (randomDirection == 0)
        {
            animator.SetBool("IsMoving", false);
        }
        else
        { 
            animator.SetBool("IsMoving", true);

            Flip(randomDirection);
            targetPosition = new Vector3(transform.position.x + randomDirection * randomX, transform.position.y, transform.position.z);
        }

    }

    private void SetAttackTarget()
    {
        targetPosition = playerObject.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, characterData.speed * Time.deltaTime);
    }

    public override void Checks()
    {
        if (CheckPlayerInSight())
        {
            currentState = AIState.attack;
        }
        else
        {
            currentState = AIState.idle;
        }
    }

    private bool CheckPlayerInSight()
    {
       RaycastHit2D hit = Physics2D.Raycast(transform.position, (playerObject.transform.position - transform.position).normalized, enemyData.idleDistance, ~1 << gameObject.layer);

        if (hit && hit.collider.gameObject.CompareTag("Player"))
        {
            return true;
        }

        return false;
    }
}
