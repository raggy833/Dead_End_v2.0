using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnControl : MonoBehaviour
{
    [SerializeField] private bool isTriggerEnabled = true;
    [SerializeField] private Transform[] spawnArea;
    private StageControl stageControl;

    private void Start()
    {
        stageControl = FindObjectOfType<StageControl>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && isTriggerEnabled)
        {
            int tempNum = Random.Range(0, spawnArea.Length);
            stageControl.SpawnEnemyTrigger(spawnArea[tempNum]);
            Invoke("EnabledTrigger", 10f);
            isTriggerEnabled = false;
        }
    }
    private void EnabledTrigger()
    {
        isTriggerEnabled = true;
    }

}
