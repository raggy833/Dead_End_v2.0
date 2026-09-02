using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : Interactable
{
    public GameObject getItemEffect;
    private StageControl stageControl;
    private PlayerGunControl playerGunControl;
    private MsgPanel_System msgPanel_System;
    void Start()
    {
        stageControl = FindObjectOfType<StageControl>();
        playerGunControl = FindObjectOfType<PlayerGunControl>();
        msgPanel_System = FindObjectOfType<MsgPanel_System>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void Interact()
    {
        AudioManager.instance.Play("get_item");
        // Check total items 
        int totalItem = stageControl.FindItem();
        // Use returned value for prompt message
        msgPanel_System.OutputMsg("Aquired " + totalItem + "/ 3");
        Instantiate(getItemEffect, this.transform.position, this.transform.rotation);
        Destroy(gameObject);
    }
}
