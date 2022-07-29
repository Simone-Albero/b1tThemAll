using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingCircle : MonoBehaviour
{
    [SerializeField] private Image circleImg;
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI txtProgress;

    [SerializeField] [Range(0,1)] public float progress = 0f;


    void Update()
    {
        circleImg.fillAmount = progress;
        background.fillAmount = progress;
        txtProgress.text = Mathf.Floor(progress * 100).ToString();
    }
}
