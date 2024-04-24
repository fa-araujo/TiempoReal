using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{

    private const KeyCode forwardKey = KeyCode.W;
    private const KeyCode leftKey = KeyCode.A;
    private const KeyCode backKey = KeyCode.S;
    private const KeyCode rightKey = KeyCode.D;

    [SerializeField] GameObject handAxePrefab;
    private BoxCollider2D boxCollider;

    private void Start()
    {
        this.life = 100;
        this.attackDelay = 1f;
        this.lastAttack = 2f;
        this.speed = 5.0f;
        this.strength = 10;
        boxCollider = GetComponent<BoxCollider2D>();
    }
    void Update()
    {
        this.MoveTo(new Vector3());

        if (AnyArrowDown() && lastAttack > attackDelay)
            this.Attack();

        lastAttack += Time.deltaTime;
    }

    private bool AnyArrowDown() { return (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow)); }
   
    protected override void Attack()
    {
        Vector2 throwDirection = new Vector2();
        Vector3 axeOffSet = this.transform.position;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            throwDirection = Vector2.up * speed;
            axeOffSet.y += (this.boxCollider.size.y / 2);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            throwDirection = Vector2.left * speed;
            axeOffSet.x -= (this.boxCollider.size.x / 2);
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
        
        GameObject handAxe = Instantiate(handAxePrefab, axeOffSet, transform.rotation);

        if(Input.GetKey(KeyCode.LeftArrow))
            handAxe.GetComponent<SpriteRenderer>().flipX = true;

        handAxe.GetComponent<Handaxe>().SetDamage(this.strength);
        Rigidbody2D axeRb = handAxe.GetComponent<Rigidbody2D>();
        axeRb.velocity = throwDirection;

        lastAttack = 0;
    }

    protected override void MoveTo(Vector3 direction)
    {
        direction = transform.position;

        if (Input.GetKey(forwardKey))
            direction += Vector3.up;
        else if (Input.GetKey(leftKey))
            direction += Vector3.left;
        else if (Input.GetKey(backKey))
            direction += Vector3.down;
        else if (Input.GetKey(rightKey))
            direction += Vector3.right;

        transform.position = Vector2.MoveTowards(transform.position, direction, Time.deltaTime * speed);
    }

    public override void TakeDamage(int damage)
    {
        life -= damage;
    }

}
