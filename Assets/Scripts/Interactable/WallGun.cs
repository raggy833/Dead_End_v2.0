using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGun : Interactable
{
    public GunDatabase gunDatabase;
    public int gunId;
    private bool doorOpen;
    public GameObject buyWeaponEffect;
    private StageControl stageControl;
    private PlayerGunControl playerGunControl;
    private MsgPanel_System msgPanel_System;

    void Start()
    {
        AudioManager.instance.Play("bgm_game");

        stageControl = FindObjectOfType<StageControl>();
        playerGunControl = FindObjectOfType<PlayerGunControl>();
        msgPanel_System = FindObjectOfType<MsgPanel_System>();

        this.promptMessage = gunDatabase.GetGun(gunId).name + " : " + gunDatabase.GetGun(gunId).price;
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void Interact()
    {
        // // Enough points
        // if (stageControl.WallGunCost(gunDatabase.GetGun(gunId).price))
        // {
        //     AudioManager.instance.Play("chaching");
        //     this.gameObject.layer = LayerMask.NameToLayer("Default");
        //     playerGunControl.BuyWallGun(gunId);
        //     Instantiate(buyWeaponEffect, this.transform.position, Quaternion.identity);
        // }
        // // Not enough points
        // else
        // {
        //     msgPanel_System.OutputMsg("Not enough points");
        // }

    }
}
