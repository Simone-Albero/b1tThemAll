using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    //shooting
    public Transform firePointDx;
    public Transform firePointSx;
    public Joystick aimJoystick;

    private Vector2 direction;

    //curr weapon stuff
    private GameObject currBullet;
    private float fireRate;
    private float dmg;
    private float lastShot = 0.0f;

    private readonly float distanzaJ = 0.5f; //distanza joystick dal centro per sparare

    //parametri proiettile base
    private readonly float baseFireRate = 9f;
    private readonly float baseDamage = 10f;
    //parametri proiettile laser

    //parametri proiettile razzo


    private void Update()
    {
        //valore mira
        direction.x = aimJoystick.Horizontal;
        direction.y = aimJoystick.Vertical;
    }

    private void FixedUpdate()
    {
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
        if ((direction.x < -distanzaJ || direction.x > distanzaJ || direction.y < -distanzaJ || direction.y > distanzaJ) && fireRate != 0 && (Time.time > (1f / fireRate) + lastShot))
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
    }


    public void switch2Weapon1()
    {
        currBullet = GameObject.Find("Bullet");
        dmg = baseDamage;
        fireRate = baseFireRate;
    }


    public void switch2Weapon2()
    {
        currBullet = GameObject.Find("Roket");
        dmg = baseDamage;
        fireRate = baseFireRate;
    }


    public void switch2Weapon3()
    {
        currBullet = GameObject.Find("Bullet");
        dmg = baseDamage;
        fireRate = baseFireRate;
    }
}
