using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseBox : Interactable
{
    private StageControl stageControl;
    public bool isBroken;
    [SerializeField] private GameObject fuseboxGO;
    [SerializeField] private GameObject brokenFuseboxGO;
    [SerializeField] private PropHealth propHealth;

    [SerializeField] private GameObject smokeEffect;
    [SerializeField] private GameObject sparkEffect;
    [SerializeField] private GameObject sparkEffect2;
    [SerializeField] private GameObject playerSmokeEffect;
    private int fixPrice = 2000;

    private MsgPanel_System msgPanel_System;

    private void Start()
    {
        stageControl = FindObjectOfType<StageControl>();
        msgPanel_System = FindObjectOfType<MsgPanel_System>();
        isBroken = false;
        promptMessage = "Fix fuse box: 2000";
        useEvents = false;
    }

    protected override void Interact()
    {

        if (stageControl.points >= 2000)
        {
            // Fix fuse box
            stageControl.FuseboxFixed();
            smokeEffect.SetActive(false);
            sparkEffect.SetActive(false);
            sparkEffect2.SetActive(false);
            isBroken = false;
            propHealth.health = propHealth.maxHealth;
        }
        else
        {
            // Not enough points
            AudioManager.instance.Play("boop");
        }

    }
    public void FuseboxBroken()
    {
        useEvents = true;
        playerSmokeEffect.SetActive(true);
        playerSmokeEffect.GetComponent<ParticleSystem>().Play();
        smokeEffect.SetActive(true);
        smokeEffect.GetComponent<ParticleSystem>().Play();
        sparkEffect.SetActive(true);
        sparkEffect.GetComponent<ParticleSystem>().Play();
        sparkEffect2.SetActive(true);
        sparkEffect2.GetComponent<ParticleSystem>().Play();
        isBroken = true;
        promptMessage = "Fix fuse box :" + fixPrice;
        stageControl.FuseboxBroken();
    }
}
