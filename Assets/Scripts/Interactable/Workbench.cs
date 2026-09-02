using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workbench : Interactable
{
    private StageControl stageControl;
    [SerializeField] private GameObject workbenchGO;
    [SerializeField] private GameObject workbenchPanel;

    [SerializeField] private GameObject brokenWorkbenchGO;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    protected override void Interact()
    {
        Debug.Log("Start workbench interact");
        workbenchPanel.SetActive(true);
        workbenchPanel.GetComponent<WorkbenchControl>().UpdateWeaponList();
        // Pause time
        Time.timeScale = 0;
    }
    public void WorkbenchBroken()
    {
        workbenchGO.SetActive(false);
        brokenWorkbenchGO.SetActive(true);
    }
}
