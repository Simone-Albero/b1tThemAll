using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.Text;

public class GameHandler : MonoBehaviour
{
    private float score = 0.0f;
    public TextMeshProUGUI scoreOutput;

    private Dictionary<Type,int> gameStats;

    private Spawn spawner;
    [SerializeField]
    private float spawnRate = 0.15f;

    public GameObject wrench;
    public GameObject policeCar;
    public GameObject helicopter;
    public GameObject militaryJeep;
    public GameObject playerBody;

    //GameOver Stuff
    private bool gameHasEnded = false;
    public GameObject gameOverMenu;

    private AudioManager audioManager;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        spawner = this.GetComponent<Spawn>();
        gameStats = new Dictionary<Type, int>();
        StartCoroutine(spawner.dropInsideMap(wrench, 10, 99, playerBody, 10));
        StartCoroutine(spawner.dropInsideMap(policeCar, 4, spawnRate, playerBody, PoliceCar.id));
        StartCoroutine(spawner.RandomDrop(helicopter, 2, spawnRate, playerBody, Helicopter.id));
        StartCoroutine(spawner.dropInsideMap(militaryJeep, 3, (spawnRate-0.05f), playerBody, MilitaryJeep.id));
    }

    void Update()
    {
        scoreOutput.text = score.ToString();
    }

    public void setScore(int value){ this.score = value; }
    public float getScore(){ return this.score; }
    public void updateScore(float value) { this.score += value; }

    public void gameOver()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            Time.timeScale = 0f;
            gameOverMenu.GetComponentsInChildren<TextMeshProUGUI>()[0].text = "Current Score: " + score.ToString();
            gameOverMenu.GetComponentsInChildren<TextMeshProUGUI>()[1].text = getFinalStats();
            gameOverMenu.SetActive(true);
            audioManager.play("GameOver");
            audioManager.decreaseVolume("Theme", 0.2f);
        }
    }

    public void updateStats(Type key)
    {
        if (gameStats.ContainsKey(key))
            gameStats[key] += 1;
        else
            gameStats.Add(key, 1);
    }

    private String getFinalStats()
    {
        StringBuilder result = new StringBuilder("Killed Units\n");

        foreach(Type t in gameStats.Keys)
        {
            result.Append("-" + t.Name + ": " + gameStats[t] + "\n");
        }

        return result.ToString();
    }

}
