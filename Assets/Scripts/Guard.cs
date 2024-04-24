using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Guard : Character
{
    private Vector3 patrolPosition;
    private Player player;

    private float range = 5f;
    private float attackRange = 1f;

    private bool stunned = false;

    private Rigidbody2D rb;
    private Vector2 throwDirection = Vector2.down;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        patrolPosition = this.transform.position;
        speed = 2f;
        attackDelay = 1.5f;
        lastAttack = 0f;
        life = 50;
        strength = 10;
    }

    private void Update()
    {
        if (stunned)
            return;

        Transform playerFound = this.SearchForPlayer();

        if (playerFound != null)
        {
            player = playerFound.gameObject.GetComponent<Player>();
            this.MoveTo(playerFound.position);
        }
        else if (this.transform.position.x == patrolPosition.x)
            this.Patrol();
        else
            this.ReturnToPatrol();
        
        lastAttack += Time.deltaTime;
    }
    private Transform SearchForPlayer()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, range, LayerMask.GetMask("Player"));

        if (player)
            return player.transform;
        
        return null;
    }
    protected override void Attack()
    {
        player.TakeDamage(this.strength);
        lastAttack = 0f;
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
        transform.position = Vector2.MoveTowards(transform.position, direction, speed * Time.deltaTime);
        
        if (Vector2.Distance(this.transform.position, direction) <= attackRange && lastAttack > attackDelay)
            this.Attack();
    }
    private void ReturnToPatrol()
    {
        transform.position = Vector2.MoveTowards(transform.position, patrolPosition, speed * Time.deltaTime);
    }

    public override void TakeDamage(int damage)
    {
        this.StartCoroutine(Stun());
        this.life -= damage;
        
        if(this.life <= 0)
            Destroy(gameObject);
    }

    private IEnumerator Stun()
    {
        this.stunned = true;
        //visual feedback de cuando esta stunned
        yield return new WaitForSeconds(0.1f);

        this.stunned = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject.layer == 3)
            collision.gameObject.GetComponent<Player>().TakeDamage(this.strength);        
    }


}
