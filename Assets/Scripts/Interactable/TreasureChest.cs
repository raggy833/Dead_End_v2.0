using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : Interactable
{
    public GameObject openEffect;
    private StageControl stageControl;
    private Vector3 effectPosOffset;
    void Start()
    {
        stageControl = FindObjectOfType<StageControl>();
    }

    protected override void Interact()
    {
        effectPosOffset = new Vector3(0, 0.5f, 0);
        Instantiate(openEffect, this.gameObject.transform.position + effectPosOffset, Quaternion.identity);
        stageControl.AddPoints(1000);
        Destroy(this.gameObject);
    }
}
