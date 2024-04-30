using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float radius=5f;
    [SerializeField] private float mecha =3f;
    [SerializeField] private float fuerzaExp = 5f;
    [SerializeField] private GameObject explosion;
    [SerializeField] private int damage;

    float currentTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > mecha)
        {
            Explota();
        }
    }

    public void Explota()
    {
        //Collider2D[] elementos = Physics2D.OverlapCircle(transform.position, radius, LayerMask.GetMask("Enemies"));
        Collider2D[] elementos = Physics2D.OverlapCircleAll(transform.position, radius);
        Debug.Log(elementos.Length);

        foreach (Collider2D elemento in elementos){
            Rigidbody2D rb = elemento.GetComponent<Rigidbody2D>();

            if (rb != null){
                Vector2 direccion = elemento.transform.position - transform.position;
                float distancia = 1 + direccion.magnitude;
                float fuerzaFinal = fuerzaExp / distancia;
                
                rb.AddForce(direccion * fuerzaFinal, ForceMode2D.Impulse);
                danioExplosion(elemento);
            }
        }
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
        Debug.Log("bombitaaaaaa");

    }

    private void danioExplosion(Collider2D collision)
    {
        if (collision != null && (collision.gameObject.layer == 8 || collision.gameObject.layer == 3))
        {
            collision.GetComponent<Character>().TakeDamage(damage);
        }
    }
}
