using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Bullet
{
    //homing stuff
    private Transform target;

    public Rocket() : base()
    {}

    private new void Start()
    {
        base.Start();
        target = FindTarget();
    }


    void FixedUpdate()
    {
        if (target == null)
        {
            target = FindTarget();
        }
        Vector2 point2target = (Vector2)transform.position - (Vector2)target.position;
        point2target.Normalize();
        float value = Vector3.Cross(point2target, transform.up).z;
        rb.angularVelocity = 200f * value;
        rb.velocity = transform.up * bulletVelocity;
    }

    private Transform FindTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemy");
        float minDist = Mathf.Infinity;
        float dist;
        Transform target = null;
        foreach (GameObject currTarget in targets)
        {
            dist = Vector3.Distance(transform.position, currTarget.GetComponent<Transform>().position);
            if (dist < minDist)
            {
                target = currTarget.GetComponent<Transform>();
                minDist = dist;
            }
        }

        return target;
    }

/* **in memoria delle due ore di tempo buttate dal nostro amico giordano detto tavernello**
private void HomeStorica()
{
    if (target == null)
    {
        target = FindTarget();
    }

    Vector2 direction = (Vector2)target.position - rb.position;
    direction.Normalize();

    lerpTimerRocket += Time.deltaTime;
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
    float angleOriginal = gameObject.GetComponent<Rigidbody2D>().rotation;
    float percent = lerpTimerRocket / 10f;
    if (lerpTimerRocket > 3f) percent = 1;
    if (angle != angleOriginal)
        gameObject.GetComponent<Rigidbody2D>().rotation = (1 - percent) * angleOriginal + (angle * percent);

    rb.velocity = transform.up * bulletVelocity;
}*/

}
