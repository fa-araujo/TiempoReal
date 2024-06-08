using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    private float maxHealth = 100f;
    private float currentHealth = 100f;
    private Image healthBarImage;

    private void Start()
    {
        healthBarImage = GetComponent<Image>();

        float imageHeight = Camera.main.pixelHeight;
        float imageWidth = Camera.main.pixelWidth;

        float xPos = (imageWidth / 3f);
        float yPos = (imageHeight / 2f) / 1000;

        Camera mainCamera = Camera.main;
        Vector3 cameraBottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        transform.position = new Vector3(cameraBottomLeft.x + xPos, cameraBottomLeft.y + yPos*2, 0);
    }
    void Update()
    {
        float fillAmount = currentHealth / maxHealth;
        healthBarImage.fillAmount = fillAmount;
        transform.position = new Vector3(Camera.main.transform.position.x - 6.21f, transform.position.y, transform.position.z);
    }
    public void SetCurrentHealth(int health)
    { 
        currentHealth = (float)health;
    }
    public void IncreaseMaxHealth(int quantity)
    {
        maxHealth += (float)quantity;
        currentHealth = (float)quantity;
    }
}
