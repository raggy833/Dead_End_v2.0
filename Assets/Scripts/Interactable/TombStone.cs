using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TombStone : Interactable
{
    private StageControl stageControl;
    private PlayerGunControl playerGunControl;
    private MsgPanel_System msgPanel_System;

    void Start()
    {
        stageControl = FindObjectOfType<StageControl>();
        playerGunControl = FindObjectOfType<PlayerGunControl>();
        msgPanel_System = FindObjectOfType<MsgPanel_System>();

        this.promptMessage = "Place skulls...";
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void UpdateInteractText(string txt)
    {
        this.promptMessage = txt;
    }

    protected override void Interact()
    {
        int curItemNum = stageControl.itemsAcquiredNum;
        if (curItemNum < 3)
        {
            msgPanel_System.OutputMsg("Not enough skulls");
        }
        else if (curItemNum == 3)
        {
            // AudioManager.instance.Play("get_item");
            stageControl.BossFightTrigger();
            Destroy(gameObject);
        }
        else
        {

        }
    }
}
