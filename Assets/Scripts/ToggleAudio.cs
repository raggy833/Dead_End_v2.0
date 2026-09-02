using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleAudio : MonoBehaviour
{
    [SerializeField] private bool _toggleMusic;
    private bool muted = false;
    public Sprite audioIcon;
    public Sprite muteIcon;

    public void Toggle()
    {
        if (_toggleMusic)
        {
            AudioManager.instance.ToggleVolume();
            if (muted)
            {
                this.gameObject.GetComponent<Image>().sprite = audioIcon;
                muted = !muted;
            }
            else if (!muted)
            {
                this.gameObject.GetComponent<Image>().sprite = muteIcon;
                muted = !muted;
            }
        }
    }
}
