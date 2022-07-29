using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMenu : MonoBehaviour
{

    public void Weapon1()
    {
        FindObjectOfType<Player>().switch2Weapon1();
        GameObject.Find("Robot").GetComponent<Animator>().SetTrigger("disable");
        GameObject.Find("Robot").GetComponent<Animator>().SetTrigger("w1");
        GameObject.Find("Robot").GetComponent<AudioSource>().Play();
    }

    public void Weapon2()
    {
        FindObjectOfType<Player>().switch2Weapon2();
        GameObject.Find("Robot").GetComponent<Animator>().SetTrigger("disable");
        GameObject.Find("Robot").GetComponent<Animator>().SetTrigger("w2");
        GameObject.Find("Robot").GetComponent<AudioSource>().Play();
    }

    public void Weapon3()
    {
        FindObjectOfType<Player>().switch2Weapon3();
        GameObject.Find("Robot").GetComponent<Animator>().SetTrigger("disable");
        GameObject.Find("Robot").GetComponent<Animator>().SetTrigger("w3");
        GameObject.Find("Robot").GetComponent<AudioSource>().Play();
    }
}
