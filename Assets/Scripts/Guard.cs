using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Guard : MonoBehaviour
{
    private int health = 50;
    private int strength = 10;
    private Vector3 patrolPosition;

    private float speed = 2f;
    private float attackDelay = 1.5f;
    private float range = 4f;
    private float attackRange = 1f;
    private float attackTimer = 0f;

    private bool stunned = false;

    private Rigidbody2D rb;
    private Vector2 throwDirection = Vector2.down;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        patrolPosition = this.transform.position;
    }

    private void Update()
    {
        Transform playerFound = this.SearchForPlayer();

        if (playerFound != null && !stunned)
            this.MoveToPlayer(playerFound);
        else if (!stunned && this.transform.position.x == patrolPosition.x)
            this.Patrol();
        else
            this.ReturnToPatrol();
        
        attackTimer += Time.deltaTime;
    }
    private Transform SearchForPlayer()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, range, LayerMask.GetMask("Player"));

        if (player)
            return player.transform;
        
        return null;
    }
    private void Attack(Player player)
    {
        player.TakeDamage(this.strength);
        attackTimer = 0f;
    }

    private void Patrol()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, throwDirection, 1f, LayerMask.GetMask("Objects"));

        if (hit)
            throwDirection *= -1;

        rb.velocity = throwDirection;
    }

    private void MoveToPlayer(Transform playerPos)
    {
        transform.position = Vector2.MoveTowards(transform.position, playerPos.position, speed * Time.deltaTime);

        if (Vector2.Distance(this.transform.position, playerPos.position) <= attackRange && attackTimer > attackDelay)
            this.Attack(playerPos.gameObject.GetComponent<Player>());
    }
    private void ReturnToPatrol()
    {
        transform.position = Vector2.MoveTowards(transform.position, patrolPosition, speed * Time.deltaTime);
    }

    private void TakeDamage(int damage)
    {
        this.StartCoroutine(Stun());
        health -= damage;
        
        if(health <= 0)
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
            collision.gameObject.GetComponent<Player>().TakeDamage(10);        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.tag == "Handaxe")
        {
            this.TakeDamage(10);
            Destroy(collision.gameObject);
        }
    }
}
