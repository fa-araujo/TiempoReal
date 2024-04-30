using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEditor.Rendering.FilterWindow;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float duracionExp = 1f;
    float currentTime;

    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Log("animar explosion");
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > duracionExp){
            DestroyObject();
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
        Debug.Log("destruir explosion");
    }
}
