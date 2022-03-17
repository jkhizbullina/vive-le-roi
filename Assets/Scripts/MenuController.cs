using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject continueButton;
    public GameObject startButton;
    public GameObject restartButton;
    public Slider volumeSlider;
    public AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("health") && PlayerPrefs.HasKey("screen"))
        {
            continueButton.GetComponent<Button>().interactable = true;
            startButton.SetActive(false);
            restartButton.SetActive(true);
        }
        if (PlayerPrefs.HasKey("volume"))
        {
            audio.volume = volumeSlider.value = PlayerPrefs.GetFloat("volume");
        }
    }

    // Update is called once per frame
    public void Restart()
    {
        PlayerPrefs.DeleteKey("health");
        PlayerPrefs.DeleteKey("screen");
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
    }

    public void ChangeVolume()
    {
        audio.volume = volumeSlider.value;
    }

    public void Quit()
    {
        Save();
        PlayerPrefs.Save();
        Application.Quit();
    }
}
