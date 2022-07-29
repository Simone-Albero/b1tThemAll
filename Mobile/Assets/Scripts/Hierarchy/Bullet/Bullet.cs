using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //dopo quanti secondi viene distrutto in automatico
    [SerializeField]
    private float time = 3;
    protected float dmg = 30;
    public float bulletVelocity = 1f;
    private int id;
    protected Rigidbody2D rb;

    public Bullet(){}

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, time);
        GameObject.FindObjectOfType<AudioManager>().play(GetType().ToString());
        rb.AddForce(transform.up * bulletVelocity, ForceMode2D.Impulse);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (id != 0)
        {
            if (collision.gameObject.tag != "Enemy" && collision.gameObject.tag != "Bullet" )
                Destroy(gameObject);
        }
        else //id = 0
        {
            if (collision.gameObject.tag != "Robot" && collision.gameObject.tag != "Bullet" && collision.gameObject.tag != "Ability") 
                Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Ability" && id != 0) dmg = 0;
    }

    public void SetDmg(float value)
    {
        dmg = value;
    }
    public float GetDmg()
    {
        return dmg;
    }

    public void SetId(int value)
    {
        id = value;
    }
    public float GetId()
    {
        return id;
    }
}