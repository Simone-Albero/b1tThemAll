using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceOfficer : Shooter
{
    public static int id = 2;
    public GameObject deadBody;

    public PoliceOfficer() : base()
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
            //GameObject.Destroy(Instantiate(deadBody, transform.position, Quaternion.identity), 2f);
            game.updateStats(this.GetType());
        }
            
    }

}
