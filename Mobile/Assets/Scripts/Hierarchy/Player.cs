using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Player : Entity
{
    public static int id = 0;

    //Joystick
    public Joystick movJoystick;
    private Vector2 movement;

    public Transform firePointDx;
    public Transform firePointSx;
    public Joystick aimJoystick;
    private readonly float distanzaJ = 0.5f; //distanza joystick dal centro per sparare

    //health bar stuff (float lerpTimer in Entity.cs)
    public float totalLerpTime = 2f;
    public Image frontHealthBar;
    public Image redBackHealthBar;
    public Image greenBackHealthBar;
    public TextMeshProUGUI healthBarText;

    //shield
    private bool shieldActivated;
    private float shieldValue;
    [SerializeField]
    private float maxShieldValue = 50f;
    public TextMeshProUGUI shielsBarText;

    //shield bar stuff
    public Image frontShieldBar;

    //shooting
    private Vector2 direction;
    private GameObject currBullet;
    private float lastShot = 0.0f;

    //parametri proiettile base
    private readonly float baseFireRate = 9f;
    private readonly float baseDamage = 10f;
    public GameObject simpleBullet;

    //parametri proiettile rocket
    public GameObject rocketBullet;

    //parametri proiettile laser
    private bool isWeaponLaser;
    public GameObject laserSx;
    public GameObject laserDx;

    public Player() : base() { }

    new void Start()
    {
        switch2Weapon1();
        base.Start();
        base.setId(id);
        shieldValue = maxShieldValue;
        shieldActivated = true;
    }

    void Update()
    {
        UpdateHealthUI();
        UpdateShieldUI();
        HealthPlusMinus();
        setHealth(Mathf.Clamp(getHealth(), 0, getMaxHealth()));
        healthBarText.text = base.getHealth().ToString() + "/" + base.getMaxHealth().ToString();
        shielsBarText.text = ((int)shieldValue).ToString() + "/" + ((int)maxShieldValue).ToString();

        direction.x = aimJoystick.Horizontal;
        direction.y = aimJoystick.Vertical;

        if (base.getHealth() <= 0)
        {
            FindObjectOfType<GameHandler>().gameOver();
        }

        CalcoloSpostamento();
    }


    private void FixedUpdate()
    {
        //movimento
        base.rigidBody.MovePosition(rigidBody.position + movement * base.getSpeed() * Time.fixedDeltaTime);
        //ricarica scudo
        RechargeShield(0.15f);
        //rotazione
        if (direction.x != 0 && direction.y != 0)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            gameObject.GetComponent<Player>().rigidBody.rotation = angle;
        }

        Shoot();
    }

    public void Shoot()
    {
        if ((direction.x < -distanzaJ || direction.x > distanzaJ || direction.y < -distanzaJ || direction.y > distanzaJ))
        {
            if (fireRate != 0 && (Time.time > (1f / fireRate) + lastShot) && !isWeaponLaser)
            {
                lastShot = Time.time;
                int idRobot = gameObject.GetComponent<Player>().getId();

                //sx
                GameObject bullet1 = Instantiate(currBullet, firePointSx.position, firePointSx.rotation);
                bullet1.GetComponent<Bullet>().SetDmg(dmg);
                bullet1.GetComponent<Bullet>().SetId(idRobot);

                //dx
                GameObject bullet2 = Instantiate(currBullet, firePointDx.position, firePointDx.rotation);
                bullet2.GetComponent<Bullet>().SetDmg(dmg);
                bullet2.GetComponent<Bullet>().SetId(idRobot);
            }
            if (isWeaponLaser)
            {
                laserSx.SetActive(true);
                laserDx.SetActive(true);
            }
        }
        else
        {
            laserSx.SetActive(false);
            laserDx.SetActive(false);
        }
    }

    private void CalcoloSpostamento()
    {
        float horizontalMove = 0f;
        float verticalMove = 0f;

        //valori spostamento orizzontale
        if (movJoystick.Horizontal > 0.2f)
            horizontalMove = this.getSpeed();
        else
            if (movJoystick.Horizontal < -0.2f)
            horizontalMove = -this.getSpeed();

        //valori spostamento verticale
        if (movJoystick.Vertical > 0.2f)
            verticalMove = this.getSpeed();
        else
            if (movJoystick.Vertical < -0.2f)
            verticalMove = -this.getSpeed();

        //valore movimento
        movement.x = horizontalMove;
        movement.y = verticalMove;
    }

    public void RestoreHp(float value) //value espressa in percentuale
    {
        float newHealt = base.getHealth() + base.getMaxHealth() * value;
        lerpTimerHealthBar = 0f;
        if (newHealt < base.getMaxHealth()) base.setHealth(newHealt);
        else base.setHealth(base.getMaxHealth());

    }

    public bool FullHealth()
    {
        return base.getHealth() == base.getMaxHealth();
    }

    private void UpdateHealthUI()
    {
        float fillFrontBar = frontHealthBar.fillAmount;
        float fillRedBar = redBackHealthBar.fillAmount;
        float healthFraction = getHealth() / getMaxHealth();

        if (fillRedBar > healthFraction) //se true significa che il player ha preso danno
        {
            frontHealthBar.fillAmount = healthFraction;
            greenBackHealthBar.fillAmount = healthFraction;

            float percentComplete = lerpTimerHealthBar / totalLerpTime;
            if (lerpTimerHealthBar < totalLerpTime)
            {
                lerpTimerHealthBar += Time.deltaTime;
                redBackHealthBar.fillAmount = Mathf.Lerp(fillRedBar, healthFraction, percentComplete);
            }
            else
            {
                redBackHealthBar.fillAmount = healthFraction;
            }
        }

        if (fillFrontBar < healthFraction) //se true significa che il player si è curato
        {
            greenBackHealthBar.fillAmount = healthFraction;

            float percentComplete = lerpTimerHealthBar / totalLerpTime;
            if (lerpTimerHealthBar + 0.1f < totalLerpTime)
            {
                lerpTimerHealthBar += Time.deltaTime;
                frontHealthBar.fillAmount = Mathf.Lerp(fillFrontBar, healthFraction, percentComplete);
            }
            else
            {
                frontHealthBar.fillAmount = healthFraction;
                redBackHealthBar.fillAmount = healthFraction;
            }
        }

    }

    private void HealthPlusMinus()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            subHealth(10f);
            lerpTimerHealthBar = 0f;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            subHealth(-10f);
            lerpTimerHealthBar = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //se coolide con un proiettile 
        if (collision.gameObject.tag == "Bullet" && base.getId() != collision.gameObject.GetComponent<Bullet>().GetId())
        {
            //danni proiettile
            float currDmg = collision.gameObject.GetComponent<Bullet>().GetDmg();
            if (shieldActivated)
            {
                shieldValue -= currDmg;
                if (shieldValue <= 0)
                {
                    shieldActivated = false;
                    shieldValue = 0;
                }
            }
            else
            {
                base.subHealth(currDmg);
                lerpTimerHealthBar = 0f;
            }
        }
    }

    private void UpdateShieldUI()
    {
        frontShieldBar.fillAmount = shieldValue / maxShieldValue;
    }

    private void RechargeShield(float valuePerFrame)
    {
        if (shieldActivated)
        {
            if (shieldValue < maxShieldValue)
            {
                shieldValue += valuePerFrame;
            }
        }
    }

    public void switch2Weapon1()
    {
        currBullet = simpleBullet;
        dmg = baseDamage;
        fireRate = baseFireRate;
        isWeaponLaser = false;
    }


    public void switch2Weapon2()
    {
        currBullet = rocketBullet;
        dmg = 18f;
        fireRate = 2.5f;
        isWeaponLaser = false;
    }


    public void switch2Weapon3()
    {
        isWeaponLaser = true;
    }

}
