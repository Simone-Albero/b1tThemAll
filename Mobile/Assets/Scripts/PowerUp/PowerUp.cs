using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 0.6f;

    protected string soundName;

    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 0, rotationSpeed));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Robot")
        {
            //GameObject robot = collision.gameObject;

            //if (!robot.GetComponent<Player>().FullHealth())
            //{

            //GameObject.FindObjectOfType<AudioManager>().play(GetType().ToString());
            interaction(collision);
            Destroy(gameObject);
            //}
        }
    }

    public abstract void interaction(Collider2D collision);

}
