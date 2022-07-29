using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private GameObject buttons;

    [SerializeField]
    private GameObject options;

    [SerializeField]
    private TextMeshProUGUI title;

    public void Pause()
    {
        pauseMenu.SetActive(true);
        buttons.SetActive(true);
        options.SetActive(false);
        title.text = "Game Pause";
        GameObject.FindObjectOfType<AudioManager>().decreaseVolume("Theme", 0.2f);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameObject.FindObjectOfType<AudioManager>().increaseVolume("Theme", 0.2f);
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void Options()
    {
        buttons.SetActive(false);
        options.SetActive(true);
        title.text = "Game Options";
    }

    public void Back()
    {
        title.text = "Game Pause";
        buttons.SetActive(true);
        options.SetActive(false);
    }
}
