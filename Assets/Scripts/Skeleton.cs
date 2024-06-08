using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Skeleton : Character
{
    private Vector3 patrolPosition;
    private Player player;

    private float visionRange = 5f;
    private float attackRange = 1.5f;

    private bool stunned = false;
    private bool alive = true;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 throwDirection = Vector2.down;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        patrolPosition = this.transform.position;
        speed = 2f;
        attackDelay = 3f;
        lastAttack = 0f;
        life = 50;
        strength = 10;
    }

    private void Update()
    {
        if (stunned || !alive)
            return;

        Transform playerFound = this.SearchForPlayer();

        if (playerFound != null)
        {
            if (Vector3.Distance(this.transform.position, playerFound.position) < attackRange)
                this.Attack();
            else
            {
                player = playerFound.gameObject.GetComponent<Player>();
                this.MoveTo(playerFound.position);
            }
        }
        else if (this.transform.position.x == patrolPosition.x)
            this.Patrol();
        else
            this.ReturnToPatrol();
        
        lastAttack += Time.deltaTime;
    }
    private Transform SearchForPlayer()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, visionRange, LayerMask.GetMask("Player"));

        if (player)
            return player.transform;
        
        return null;
    }
    protected override void Attack()
    {
        if (lastAttack > attackDelay)
        {
            animator.SetTrigger("Attack");
            player.TakeDamage(this.strength);
            lastAttack = 0f;
        }
    }

    private void Patrol()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, throwDirection, 1f, LayerMask.GetMask("Objects"));

        if (hit)
            throwDirection *= -1;

        rb.velocity = throwDirection;
    }

    protected override void MoveTo(Vector3 direction)
    {
        this.LookInDirection(direction);

        transform.position = Vector2.MoveTowards(transform.position, direction, speed * Time.deltaTime);
    }
    private void ReturnToPatrol()
    {
        this.LookInDirection(patrolPosition);

        transform.position = Vector2.MoveTowards(transform.position, patrolPosition, speed * Time.deltaTime);
    }

    private void LookInDirection(Vector3 direction)
    {
        if (this.transform.InverseTransformPoint(direction).x < 0f)
            this.GetComponent<SpriteRenderer>().flipX = true;
        else
            this.GetComponent<SpriteRenderer>().flipX = false;
    }

    public override void TakeDamage(int damage)
    {
        if (!alive)
            return;

        this.StartCoroutine(Stun());
        this.life -= damage;

        if (this.life <= 0)
        {
            alive = false;
            animator.SetTrigger("Dead");

            this.rb.Sleep();
            this.GetComponent<BoxCollider2D>().enabled = false;
            this.enabled = false;
        }
    }

    private IEnumerator Stun()
    {
        this.stunned = true;
        animator.SetTrigger("Hit");
        yield return new WaitForSeconds(0.1f);

        this.stunned = false;
    }

}
