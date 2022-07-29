using Firebase.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public GameObject buttonsController;
    public GameObject options;
    public GameObject credits;
    public GameObject social;
    public GameObject login;
    public GameObject register;

    public GameObject outputView;
    public TMP_Text feedbackText;

    public void PlayButton()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void OptionButton()
    {
        buttonsController.SetActive(false);
        options.SetActive(true);
    }

    public void CreditsButton()
    {
        buttonsController.SetActive(false);
        credits.SetActive(true);
    }

    public void SocialButton()
    {
        buttonsController.SetActive(false);
        social.SetActive(true);
    }

    public void RegisterButton()
    {
        login.SetActive(false);
        register.SetActive(true);
    }

    public void BackFromOptions()
    {
        buttonsController.SetActive(true);
        options.SetActive(false);
    }

    public void BackFromCredits()
    {
        buttonsController.SetActive(true);
        credits.SetActive(false);
    }

    public void BackFromSocial()
    {
        buttonsController.SetActive(true);
        social.SetActive(false);
    }

    public void BackFromRegister()
    {
        register.SetActive(false);
        login.SetActive(true);
    }

    internal void awaitVerification(bool isSent, string email, string output)
    {
        register.SetActive(false);
        login.SetActive(false);
        outputView.SetActive(true);

        if (isSent)
        {
            feedbackText.text = $"Sent Email!\nPlease Verify {email}";
        }
        else
        {
            feedbackText.text = $"Email Not Sent: {output}\nPlease Verify {email}";
        }
    }

    public void BackFromVerify()
    {
        outputView.SetActive(false);
        login.SetActive(true);
    }

    internal void userProfile(FirebaseUser user)
    {
        login.SetActive(false);
        outputView.SetActive(true);

        feedbackText.text = $"Hi {user.DisplayName}\nHere's your email: {user.Email.ToString()}\nThanks for joining us!";

    }
}
