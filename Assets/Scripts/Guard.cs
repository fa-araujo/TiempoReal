using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Guard : MonoBehaviour
{
    private int health = 50;
    private int strength = 10;

    private float speed = 2f;
    private float attackDelay = 1.5f;
    private float range = 0.0002f;
    private float attackRange = 1f;
    private float attackTimer = 0f;

    private bool stunned = false;
    private bool shouldMoveToPlayer = false;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Transform playerPos = this.SearchForPlayer();

        if (shouldMoveToPlayer && !stunned)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerPos.position, speed * Time.deltaTime);

            if (Vector2.Distance(this.transform.position, playerPos.position) <= attackRange && attackTimer > attackDelay)
                this.Attack(playerPos.gameObject.GetComponent<Player>());
        }
        else if(!stunned)
        {
            Vector2 direction = Vector2.down;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.2f , LayerMask.GetMask("Objects"));

            if(hit)
                direction = Vector2.up;

            rb.velocity = direction;
        }
        attackTimer += Time.deltaTime;
    }
    private Transform SearchForPlayer()
    {

        Collider2D player = Physics2D.OverlapCircle(transform.position, range, LayerMask.GetMask("Player"));

        if (player)
        {
            shouldMoveToPlayer = true;
            return player.transform;
        }
        
        return null;
    }
    private void Attack(Player player)
    {
        player.TakeDamage(this.strength);
        attackTimer = 0f;
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
        if (collision != null && collision.gameObject.tag == "Handaxe")
        {
            this.TakeDamage(10);
            Destroy(collision.gameObject);
        }

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
