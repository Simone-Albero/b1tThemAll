using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class PoliceCar : Spawner
{
    public static int id = 1;
    [SerializeField]
    private GameObject explosion;


    public PoliceCar() : base() 
    {
    }

    new
        void Start()
    {
        base.setId(id);
        base.Start();

    }

    new
        void Update()
    {
        base.Update();
    }

    private new void OnDestroy()
    {
        base.OnDestroy();

        if (getHealth() <= 0)
        {
            GameObject.Destroy(Instantiate(explosion, transform.position, Quaternion.identity), 0.5f);
            GameObject.FindObjectOfType<AudioManager>().play("Explosion");
            game.updateStats(this.GetType());
        }
    }

}
