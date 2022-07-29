using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public abstract class Shooter : Enemy
{

    protected Transform targetPosition;
    private Vector2 direction;
    private float angle;
    public List<Transform> firePoints;
    public GameObject bulletPrefab;

    private float lastShot;

    public Shooter() : base ()
    {
    }


    public new void Start()
    {
        targetPosition = base.player.GetComponent<Transform>();
        lastShot = Time.time;
        base.Start();
    }

    public new void Update()
    {
        
        base.Update();
        
        if (base.isInScope)
        {
            setXDirection(getTargetPosition().position.x - base.getBody().position.x);
            setYDirection(getTargetPosition().position.y - base.getBody().position.y);
            setAngle((Mathf.Atan2(getDirection().y, getDirection().x) * Mathf.Rad2Deg)-90);
        }

        
    }

    public void FixedUpdate()
    {  
        if (base.isInScope)
        {
            base.getBody().rotation = getAngle();

            if (Time.time > (1f / base.getFireRate()) + getLastShoot())
            {
                foreach (Transform firePoint in this.firePoints)
                    Shoot(firePoint);
            }
        }

    }

    public void setXDirection(float value) { this.direction.x = value;}
    public void setYDirection(float value) { this.direction.y = value; }
    public Vector2 getDirection() { return this.direction; }

    public float getLastShoot() { return this.lastShot; }

    public void setAngle(float value) { this.angle = value; }
    public float getAngle() { return this.angle; }

    public Transform getTargetPosition() { return this.targetPosition;}


    protected void Shoot(Transform firePoint)
    {
        animator.SetTrigger("Shoot");
        lastShot = Time.time;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        //setting danno proiettile
        bullet.GetComponent<Bullet>().SetDmg(base.getDmg()); //proprietaria della classe bullet
        bullet.GetComponent<Bullet>().SetId(base.getId());
    }
}
