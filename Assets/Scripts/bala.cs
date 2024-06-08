using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // Velocidad de la bala
    public float lifetime = 5f; // Tiempo después del cual la bala se destruye
    public LayerMask targetLayer; // Capas que contienen al jugador

    void Start()
    {
        // Destruir la bala después del tiempo especificado
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Mover la bala hacia adelante
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Verificar si la bala golpea al jugador
        if ((targetLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            // Si es el jugador, destruirlo o hacer cualquier otra acción
            Debug.Log("Bala impactó al jugador.");
            Destroy(collision.gameObject); // Destruir al jugador
        }

        // Destruir la bala después de la colisión
        Destroy(gameObject);
    }
}
