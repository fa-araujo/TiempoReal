using UnityEngine;

public class CajaMovil : MonoBehaviour
{
    public float fuerzaEmpuje = 10f;
    public float tiempoDetener = 1f;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificar si el objeto que colisionó es el jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            // Obtener la dirección del empuje (de la caja hacia el jugador)
            //Vector2 direccionEmpuje = (collision.transform.position - transform.position).normalized;
            Vector2 direccionEmpuje = (transform.position - collision.transform.position).normalized;

            // Obtener el Rigidbody2D de la caja
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Aplicar una fuerza de empuje a la caja en la dirección del jugador
                rb.AddForce(direccionEmpuje * fuerzaEmpuje, ForceMode2D.Impulse);

                Invoke("DetenerCaja", tiempoDetener);
            }

        }
    }

    private void DetenerCaja()
    {
        // Obtener el Rigidbody2D de la caja
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Detener el movimiento de la caja
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }
}
