using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControl : MonoBehaviour
{
    public PlayerGunControl playerGunControl;
    void Start()
    {
        playerGunControl = FindObjectOfType<PlayerGunControl>();

    }
    public void Reload()
    {
        playerGunControl.ReloadAmmo();
    }
}
