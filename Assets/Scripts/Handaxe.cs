using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handaxe : MonoBehaviour
{
    private float timeSinceThrown = 0;

    // Update is called once per frame
    void Update()
    {
        timeSinceThrown += Time.deltaTime;

        if(timeSinceThrown > 2f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.layer == 7)
            Destroy(gameObject);
        
    }
}
