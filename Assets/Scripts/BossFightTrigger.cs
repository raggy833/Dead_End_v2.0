using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightTrigger : MonoBehaviour
{
    private StageControl stageControl;

    private void Start()
    {
        stageControl = FindObjectOfType<StageControl>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            stageControl.BossFightTrigger();
            Destroy(gameObject);
        }
    }
}
