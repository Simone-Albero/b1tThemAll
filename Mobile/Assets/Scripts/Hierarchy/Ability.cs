using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour
{
    [SerializeField] public GameObject ability;
    [SerializeField] private Button loadingButton;
    [SerializeField] private float countdown = 0;
    [SerializeField] private float shutdown = 0;
    private LoadingCircle loadingCircle;

    void Start()
    {
        loadingCircle = this.GetComponent<LoadingCircle>();
        loadingCircle.progress = 0;
        StartCoroutine(Countdown());
    }

    void Update()
    {

    }

    private IEnumerator Countdown()
    {
        float normalizedTime = 0;
        while (normalizedTime <= 1f)
        {
            loadingCircle.progress = normalizedTime + 0.01f;
            normalizedTime += Time.deltaTime / countdown;
            yield return null;
        }

        loadingButton.interactable = true;
    }

    public IEnumerator onAbilityActivate()
    {
        loadingButton.interactable = false;
        loadingCircle.progress = 0;
        yield return new WaitForSeconds(shutdown);
        ability.SetActive(false);
        yield return Countdown();
    }
}
