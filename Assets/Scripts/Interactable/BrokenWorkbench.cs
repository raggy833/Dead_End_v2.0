using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenWorkbench : Interactable
{
    private StageControl stageControl;
    [SerializeField] private GameObject workbenchGO;
    [SerializeField] private GameObject workbenchPanel;

    [SerializeField] private GameObject brokenWorkbenchGO;
    [SerializeField] private int pointsForFix = 5000;

    private void Start()
    {
        stageControl = FindObjectOfType<StageControl>();
    }
    protected override void Interact()
    {
        if (stageControl.points >= pointsForFix)
        {
            workbenchGO.SetActive(true);
            workbenchGO.GetComponent<PropHealth>().ToFullHealth();
            brokenWorkbenchGO.SetActive(false);
        }
        else
        {
            // Not enough points
            AudioManager.instance.Play("boop");
        }
    }
}
