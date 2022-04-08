using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] AudioSource buttonClick = null;
    [SerializeField] Image popUpImage = null;
    
    [SerializeField] Sprite[] images;

    public void ShowStory()
    {
        buttonClick.Play();
        popUpImage.sprite = images[0];
        popUpImage.gameObject.SetActive(true);
    }

    public void ShowHowToPlay()
    {
        buttonClick.Play();
        popUpImage.sprite = images[1];
        popUpImage.gameObject.SetActive(true);
    }

    public void ShowCredits()
    {
        buttonClick.Play();
        popUpImage.sprite = images[2];
        popUpImage.gameObject.SetActive(true);
    }

    public void Close()
    {
        buttonClick.Play();
        popUpImage.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
