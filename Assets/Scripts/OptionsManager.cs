using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;

    private void Start()
    {
        volumeSlider.onValueChanged.AddListener(delegate { Save(); });

        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            volumeSlider.value = 1;
        }
        else
        {
            Load();
        }
    }
    public void SetVolume()
    {
        AudioListener.volume = volumeSlider.value;
    }
    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }
    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
}
