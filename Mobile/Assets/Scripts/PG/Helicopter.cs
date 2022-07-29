using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter : Shooter
{
    public static int id = 3;
    [SerializeField]
    private GameObject explosion;

    public Helicopter() : base() //parametri da sistemare
    {
    }

    new

        // Start is called before the first frame update
        void Start()
    {
        base.setId(id);
        base.Start();
    }

    new

        // Update is called once per frame
        void Update()
    {
        base.Update();
    }

    private new void FixedUpdate()
    {
        base.FixedUpdate();
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
