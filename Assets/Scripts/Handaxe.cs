using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handaxe : MonoBehaviour
{
    private float timeSinceThrown = 0;
    private int _damage;

    void Update()
    {
        timeSinceThrown += Time.deltaTime;

        if(timeSinceThrown > 1f)
            Destroy(gameObject);


        transform.Rotate(Vector3.back * Time.deltaTime * 360f);
    }

    public void SetDamage(int damage) { _damage = damage; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.layer == 7)
            Destroy(gameObject);

        if (collision != null && collision.gameObject.layer == 8)
        {
            collision.GetComponent<Character>().TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}
