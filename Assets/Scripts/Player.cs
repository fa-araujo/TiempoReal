using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Player : Character
{

    private const KeyCode forwardKey = KeyCode.W;
    private const KeyCode leftKey = KeyCode.A;
    private const KeyCode backKey = KeyCode.S;
    private const KeyCode rightKey = KeyCode.D;

    [SerializeField] GameObject handAxePrefab;
    [SerializeField] GameObject lifeBar;
    private BoxCollider2D boxCollider;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector3 axeOffSet;
    private Vector2 throwDirection;
    private float spinDamageDelay = 2f;

    private void Start()
    {
        this.life = 100;
        this.attackDelay = 1f;
        this.lastAttack = 2f;
        this.speed = 5.0f;
        this.strength = 10;

        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
            this.Spin();
        else if (Input.GetKeyUp(KeyCode.Space))
            animator.SetBool("isSpinning", false);

        this.MoveTo(new Vector3());

        if (AnyArrowDown() && lastAttack > attackDelay)
        {
            this.Attack();
            animator.SetTrigger("Attack");
        }

        spinDamageDelay += Time.deltaTime;
        lastAttack += Time.deltaTime;
    }

    private bool AnyArrowDown() { return (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow)); }
   
    protected override void Attack()
    {
        throwDirection = new Vector2();
        axeOffSet = this.transform.position;
        this.spriteRenderer.flipX = false;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            throwDirection = Vector2.up * speed;
            axeOffSet.y += (this.boxCollider.size.y / 2);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            throwDirection = Vector2.left * speed;
            axeOffSet.x -= (this.boxCollider.size.x / 2);
            this.spriteRenderer.flipX = true;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            throwDirection = Vector2.down * speed;
            axeOffSet.y -= (this.boxCollider.size.y / 2);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            throwDirection = Vector2.right * speed;
            axeOffSet.x += (this.boxCollider.size.x / 2);
        }

        lastAttack = 0;
    }

    private void Spin()
    {
        animator.SetBool("isSpinning", true);

        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, spriteRenderer.size.x, LayerMask.GetMask("Enemies"));

        foreach (Collider2D enemyCollider in enemies)
        {
            Rigidbody2D enemyRigidbody = enemyCollider.attachedRigidbody;

            if (enemyRigidbody != null){
                Vector2 direction = (enemyCollider.transform.position - transform.position).normalized;

                enemyRigidbody.AddForce(direction, ForceMode2D.Impulse);

                if (spinDamageDelay > 2){
                    enemyCollider.gameObject.GetComponent<Character>().TakeDamage(strength);
                    spinDamageDelay = 0;
                }
            }
        }
    }

    private void ThrowAxe()
    {
        GameObject handAxe = Instantiate(handAxePrefab, axeOffSet, transform.rotation);
        if (Input.GetKey(KeyCode.LeftArrow))
                handAxe.GetComponent<SpriteRenderer>().flipX = true;

        handAxe.GetComponent<Handaxe>().SetDamage(this.strength);
        Rigidbody2D axeRb = handAxe.GetComponent<Rigidbody2D>();
        axeRb.velocity = throwDirection;
    }

    protected override void MoveTo(Vector3 direction)
    {
        direction = transform.position;

        if (Input.GetKey(forwardKey)) {
            direction += Vector3.up;
            this.spriteRenderer.flipX = false;
        }
        else if (Input.GetKey(leftKey)) {
            direction += Vector3.left;
            this.spriteRenderer.flipX = true;
        }
        else if (Input.GetKey(backKey)) {
            direction += Vector3.down;
            this.spriteRenderer.flipX = false;
        }
        else if (Input.GetKey(rightKey)) {
            direction += Vector3.right;
            this.spriteRenderer.flipX = false;
        }

        animator.SetBool("isRunning", direction != transform.position);
        transform.position = Vector2.MoveTowards(transform.position, direction, Time.deltaTime * speed);
    }

    public override void TakeDamage(int damage)
    {
        life -= damage;
        lifeBar.GetComponent<LifeBar>().SetCurrentHealth(life);

        if (life <= 0)
            Die();
    }

    private void Die()
    {
        animator.SetTrigger("Dead");
        this.GetComponent<Rigidbody2D>().Sleep();
        this.boxCollider.enabled = false;
        this.enabled = false;
    }








}
