using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Enemy
{
    [SerializeField]
    private int maxSpawn = 0;
    [SerializeField]
    private float spawnRate = 0.08f;
    public GameObject spawnSubject;
    public bool flag = true; //serve per impedire la generazione di più di una routine di spawn
   


    public Spawner() : base()
    {
    }

    public new void Start()
    {
        base.Start();
    }

    public new void Update()
    {
        base.Update();

        if (base.isInScope && flag)
        {
            flag = false;
            StartCoroutine(spawn.SpawnNearEnemy(animator, this.gameObject, spawnSubject, maxSpawn, spawnRate, progId));
        }

        if (!base.isInScope)
        {
            flag = true;
            StopAllCoroutines();
        }


    }


}
