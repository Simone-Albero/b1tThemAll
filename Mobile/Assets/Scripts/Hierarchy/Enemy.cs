using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
public class Enemy : Entity
{
    public Animator animator;

    //score parameter
    protected GameHandler game;
    [SerializeField]
    private float grantScore = 0.0f;

    //pathfinder parameter
    [SerializeField]
    protected float stopDistance = 10;
    private IAstarAI ai;
    protected GameObject player;
    private float distance;

    //spawn-shooting parameter
    protected Spawn spawn;
    protected bool isInScope;
    private int generationId = -1; //identifica l'unità che mi ha generato (-1 valore non valido)
    protected int progId = -1; //identifica l'ordine di generazione del'unità

    //healthbar
    public GameObject hpBar;
    private GameObject hpBarReference;
    private float verticalDistance = 1.7f;
    private float horizontalDistance = -0.5f;

    //laser
    private bool takingLaserDmg = false;

    public Enemy() : base()
    {
    }


    public new void Start()
    {
        base.Start();
        SpawnHealthBar();
        spawn = GameObject.Find("GameController").GetComponent<Spawn>();
        game = GameObject.Find("GameController").GetComponent<GameHandler>();
        ai = GetComponent<IAstarAI>();
        ai.maxSpeed = base.getSpeed();
        ai.onSearchPath += Update;
    }

    public void Update()
    {
        if (takingLaserDmg)
        {
            base.subHealth(3f);
            lerpTimerHealthBar = 0f;
        }

        UpdateHealthBar();

        if(getHealth() <= 0)
        { 
            Destroy(gameObject);
            game.updateScore(grantScore);
        }

        if (player.GetComponent<Transform>().position != null && ai != null) ai.destination = player.GetComponent<Transform>().position;
        distance = ai.remainingDistance;

        if (distance <= stopDistance)
        {
            isInScope = true;
            this.ai.isStopped = true;
        }

        if (distance > stopDistance)
        {
            this.ai.isStopped = false;
            this.isInScope = false;
        }

    }

    private void OnEnable()
    {
        ai = GetComponent<IAstarAI>();
        if (ai != null) ai.onSearchPath += Update;
        player = GameObject.Find("Robot");
    }

    void OnDisable()
    {
        if (ai != null) ai.onSearchPath -= Update;
    }

    public void OnDestroy()
    {
        Destroy(hpBarReference);
        //aggiorno le statistiche dello spawner
        if (generationId != -1)
            spawn.UpdetGeneratedEnemyTracer(generationId);
        else
        {
            spawn.UpdetEnemyTracer(getId());
            spawn.RemoveGenerationUnit(progId);
        }
    }

    private void SpawnHealthBar()
    {
        Vector2 spawnPos = gameObject.GetComponent<Transform>().position;
        spawnPos.x += horizontalDistance;
        spawnPos.y += verticalDistance;
        hpBarReference = Instantiate(hpBar, spawnPos, Quaternion.identity);
    }

    private void UpdateHealthBar()
    {
        Vector2 position = gameObject.GetComponent<Transform>().position;
        position.x += horizontalDistance;
        position.y += verticalDistance;

        Vector3 position3 = new Vector3(position.x, position.y, 0);
        hpBarReference.GetComponent<Transform>().position = position3;

        Vector3 scale = hpBarReference.GetComponentInChildren<Transform>().localScale;
        scale.x = getHealth() / getMaxHealth();
        hpBarReference.GetComponentInChildren<Transform>().localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //se coolide con un proiettile 
        if (collision.gameObject.tag == "Bullet" && base.getId() != collision.gameObject.GetComponent<Bullet>().GetId())
        {
            //danni proiettile
            float currDmg = collision.gameObject.GetComponent<Bullet>().GetDmg();
            base.subHealth(currDmg);
            lerpTimerHealthBar = 0f;
        }

        if (collision.gameObject.tag == "Laser")
        {
            takingLaserDmg = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Laser")
        {
            takingLaserDmg = false;
        }
    }

    public void setGenerationId (int value) {this.generationId = value; }

    public void setProgId(int value) { this.progId = value; }

    public void setDmg(float value) { this.dmg = value; }
    public float getDmg() { return this.dmg; }

    public void setFireRate(float value) { this.fireRate = value; }
    public float getFireRate() { return this.fireRate; }

}

