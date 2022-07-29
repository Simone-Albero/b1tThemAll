using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingButton : MonoBehaviour
{
    [SerializeField] private GameObject player;

    public void ForceField()
    {
        Ability abilityLoader = player.GetComponent<Ability>();
        abilityLoader.ability.SetActive(true);
        StartCoroutine(abilityLoader.onAbilityActivate());
    }
}
