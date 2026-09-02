using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public int doorCost;
    public int doorId;
    public bool doorLocked = false;
    private bool doorOpen;
    public GameObject openDoorEffect;
    public Vector3 offset;
    private StageControl stageControl;
    [SerializeField] private List<Transform> spawnPos = new List<Transform>();
    [SerializeField] private List<Transform> spawnTargetPos = new List<Transform>();

    void Start()
    {
        stageControl = FindObjectOfType<StageControl>();
        this.GetComponent<BoxCollider>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void Interact()
    {
        // Door with lock
        if (doorLocked)
        {

        }
        // Door with no lock
        else
        {
            if (stageControl.OpenDoor(doorCost, spawnPos, spawnTargetPos))
            {
                // Enough points
                this.GetComponent<BoxCollider>().enabled = false;
                doorOpen = !doorOpen;
                this.GetComponent<Animator>().SetBool("IsOpen", doorOpen);
                this.gameObject.layer = LayerMask.NameToLayer("Default");
                AudioManager.instance.Play("chaching");
                Instantiate(openDoorEffect, this.transform.position + offset, Quaternion.identity);
                //stageControl.ExpandSpawnArea(doorId);
            }
            else
            {
                // Not enough points
            }
        }
    }
    private void DeleteDoor()
    {
        Destroy(gameObject);
    }
}
