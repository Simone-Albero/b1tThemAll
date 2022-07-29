using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    private float health;
    public float maxHealth = 100;
    public float chipSpeed = 2f;
    private float lerpTimer;
    public Image frontHealthBar;
    public Image backHealthBar;


    
    void Start()
    {
        health = maxHealth;
    }

    
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);

    }

    public void UpdateHealthUI()
    {
        float fillFrontBar = frontHealthBar.fillAmount;
        float fillBackBar = backHealthBar.fillAmount;
        float healthFraction = health / maxHealth;


        if (fillBackBar > healthFraction) //se true significa che il player ha preso danno
        {
            frontHealthBar.fillAmount = healthFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            backHealthBar.fillAmount = Mathf.Lerp(fillBackBar, healthFraction, percentComplete);
        }
    }
}
