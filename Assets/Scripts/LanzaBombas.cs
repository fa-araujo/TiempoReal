using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanzaBombas : MonoBehaviour
{
    public GameObject bombaPrefab;
    public float fuerzaDeLanzamiento = 10f;
    [SerializeField] private int maxBombas = 5;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && GameObject.FindGameObjectsWithTag("Bomb").Length < maxBombas)
        {
            LanzarBomba();

            int cantidadDeBombas = GameObject.FindGameObjectsWithTag("Bomb").Length;

            // Mostrar la cantidad en la consola
            Debug.Log("Cantidad de bombas: " + cantidadDeBombas);
        }
    }


    void LanzarBomba()
    {
        // Obtener la posición del mouse en el mundo
        Vector2 posicionDelMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Crear la bomba en la posición del jugador
        GameObject bomba = Instantiate(bombaPrefab, transform.position, Quaternion.identity);

        // Calcular la dirección del lanzamiento
        Vector2 direccion = (posicionDelMouse - (Vector2)transform.position).normalized;

        // Aplicar fuerza a la bomba en la dirección calculada
        Rigidbody2D rb = bomba.GetComponent<Rigidbody2D>();
        if (rb != null){
            rb.AddForce(direccion * fuerzaDeLanzamiento, ForceMode2D.Impulse);
        }
    }
}
