using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    private void Start()
    {
        UpdateVolumeSlider();
        AudioManager.instance.ChangeMasterVolume(_slider.value);
        _slider.onValueChanged.AddListener(val => AudioManager.instance.ChangeMasterVolume(val));
    }
    private void UpdateVolumeSlider()
    {
        _slider.value = AudioManager.instance.volume;
    }
}
