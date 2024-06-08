using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoEnemigo : MonoBehaviour
{
    public Transform[] puntos; // Array de puntos por los que va a caminar el enemigo
    public float velocidad = 2.0f; // Velocidad de movimiento
    public float fuerzaEmpuje = 10f; // Fuerza de empuje al colisionar
    private int indicePunto = 0; // Índice del punto actual

    void Update()
    {
        // Movimiento entre puntos
        if (puntos.Length != 0)
        {
            Vector3 direccion = (puntos[indicePunto].position - transform.position).normalized;
            transform.Translate(direccion * velocidad * Time.deltaTime);

            if (Vector3.Distance(transform.position, puntos[indicePunto].position) < 0.1f)
            {
                indicePunto++;
                if (indicePunto >= puntos.Length)
                {
                    indicePunto = 0;
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Manejar la colisión con otros objetos
        Debug.Log("Colisión detectada con: " + collision.gameObject.name);

        // Aplicar fuerza de empuje al objeto con el que colisiona
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 direccion = collision.contacts[0].point - transform.position;
            rb.AddForce(direccion.normalized * fuerzaEmpuje, ForceMode.Impulse);
        }
    }
}










