using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampapinche : MonoBehaviour
{

    [SerializeField] private float velocidadRotacion = 45f; // Velocidad de rotación en grados por segundo
    [SerializeField] private float rango = 5f;
    [SerializeField] private float fuerzaDeMovimiento = 10f;
    [SerializeField] private int damage = 50;
    private Rigidbody2D rb;
    private Vector2 throwDirection = Vector2.right;
    private Vector2 direccionRaycast;
    private bool girar = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ActualizarDireccionRaycast();
    }

    // Update is called once per frame
    void Update()
    {

        if (girar)
        {
            transform.Rotate(Vector3.forward, velocidadRotacion * Time.deltaTime);

            ActualizarDireccionRaycast();

            checkForPlayer();

        } else
        {
            chasePlayer();
            girar = true;
        }
    }

    private void checkForPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direccionRaycast, rango, LayerMask.GetMask("Player"));

        if (hit)
        {
            girar = false;
        }

        //rb.velocity = direccionRaycast;
    }

    private void chasePlayer()
    {
        rb.AddForce(direccionRaycast * fuerzaDeMovimiento, ForceMode2D.Impulse);

    }


    void OnDrawGizmos()
    {
        // Dibujar el raycast en el editor
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, direccionRaycast * rango);
    }

    void ActualizarDireccionRaycast()
    {
        // Obtener la dirección hacia la derecha del objeto después de aplicar cualquier rotación
        direccionRaycast = transform.right;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null && (collision.gameObject.layer == 7 || collision.gameObject.layer == 6))
            Destroy(gameObject);

        if (collision != null && (collision.gameObject.layer == 8 || collision.gameObject.layer == 3))
        {
            collision.gameObject.GetComponent<Character>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
