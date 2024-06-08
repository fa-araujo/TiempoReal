using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanzaBombas : MonoBehaviour
{
    public GameObject bombaPrefab;
    public float fuerzaDeLanzamiento = 10f;
    [SerializeField] private int maxBombas = 5;
    [SerializeField] private float distancia = 2f;



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

        }
    }


    void LanzarBomba()
    {
        // Obtener la posici�n del mouse en el mundo
        Vector2 posicionDelMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direccionAlMouse = (posicionDelMouse - (Vector2)transform.position).normalized;

        Vector2 posicionNuevoElemento = (Vector2)transform.position + direccionAlMouse * distancia;



        // Crear la bomba en la posici�n del jugador
        GameObject bomba = Instantiate(bombaPrefab, posicionNuevoElemento, Quaternion.identity);

        // Calcular la direcci�n del lanzamiento
        Vector2 direccion = (posicionDelMouse - (Vector2)transform.position).normalized;

        // Aplicar fuerza a la bomba en la direcci�n calculada
        Rigidbody2D rb = bomba.GetComponent<Rigidbody2D>();
        if (rb != null){
            rb.AddForce(direccion * fuerzaDeLanzamiento, ForceMode2D.Impulse);
        }
    }
}
