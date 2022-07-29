using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Shooter
{
    public static int id = 4;

    public Soldier() : base()
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
            game.updateStats(this.GetType());
        }
    }

}
