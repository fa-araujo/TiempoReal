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

    void Update()
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

        Vector2 throwDirection = new Vector2();

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                throwDirection = Vector2.up * gridSize * 3;
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                throwDirection = Vector2.left * gridSize * 3;
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                throwDirection = Vector2.down * gridSize * 3;
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                throwDirection = Vector2.right * gridSize * 3;

            GameObject handAxe = Instantiate(handAxePrefab, this.transform.position + new Vector3(0.5f,-0.5f,0), transform.rotation);
            Rigidbody2D axeRb = handAxe.GetComponent<Rigidbody2D>();

            axeRb.velocity = throwDirection;
        }

        /* KeyCode keyCode = Event.current.keyCode;

        switch (keyCode) {
            case forwardKey:
                direction += Vector2.up * gridSize;
                break;

            case leftKey:
                direction += Vector2.left * gridSize;
                break;

            case backKey:
                direction += Vector2.down * gridSize;
                break;

            case rightKey:
                direction += Vector2.right * gridSize;
                break;
        }*/
    }

    public void TakeDamage(int damage)
    {
        life -= damage;
    }

}
