using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int life = 100;

    private float gridSize = 1f;

    private float speed = 5.0f;

    private const KeyCode forwardKey = KeyCode.W;
    private const KeyCode leftKey = KeyCode.A;
    private const KeyCode backKey = KeyCode.S;
    private const KeyCode rightKey = KeyCode.D;

    [SerializeField] GameObject handAxePrefab;
    private BoxCollider2D boxCollider;
    private float attackDelay = 1f;
    private float lastAttack = 2f;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }
    void Update()
    {
        this.Move();

        if (AnyArrowDown() && lastAttack > attackDelay)
            this.ThrowAxe();

        lastAttack += Time.deltaTime;
    }

    private bool AnyArrowDown() { return (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow)); }
   
    private void ThrowAxe()
    {
        Vector2 throwDirection = new Vector2();
        Vector3 axeOffSet = this.transform.position;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            throwDirection = Vector2.up * gridSize * 3;
            axeOffSet.y += (this.boxCollider.size.y / 2);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            throwDirection = Vector2.left * gridSize * 3;
            axeOffSet.x -= (this.boxCollider.size.x / 2);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            throwDirection = Vector2.down * gridSize * 3;
            axeOffSet.y -= (this.boxCollider.size.y / 2);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            throwDirection = Vector2.right * gridSize * 3;
            axeOffSet.x += (this.boxCollider.size.x / 2);
        }


        GameObject handAxe = Instantiate(handAxePrefab, axeOffSet, transform.rotation);

        if(Input.GetKeyDown(KeyCode.LeftArrow))
            handAxe.GetComponent<SpriteRenderer>().flipX = true;

        Rigidbody2D axeRb = handAxe.GetComponent<Rigidbody2D>();
        axeRb.velocity = throwDirection;

        lastAttack = 0;
    }

    private void Move()
    {
        Vector2 direction = transform.position;

        if (Input.GetKey(forwardKey))
            direction += Vector2.up * gridSize;
        else if (Input.GetKey(leftKey))
            direction += Vector2.left * gridSize;
        else if (Input.GetKey(backKey))
            direction += Vector2.down * gridSize;
        else if (Input.GetKey(rightKey))
            direction += Vector2.right * gridSize;

        transform.position = Vector2.MoveTowards(transform.position, direction, Time.deltaTime * speed);
    }

    public void TakeDamage(int damage)
    {
        life -= damage;
    }

}
