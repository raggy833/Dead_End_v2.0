using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeRangeControl : MonoBehaviour
{
    private PlayerGunControl playerGunControl;
    // Start is called before the first frame update
    void Start()
    {
        playerGunControl = GetComponentInParent<PlayerGunControl>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BreakableSkull"))
        {
            Debug.Log("Enter knife range");
            playerGunControl.OnKnifeRangeEnter(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BreakableSkull"))
        {
            Debug.Log("Exit knife range");
            playerGunControl.OnKnifeRangeExit();
        }
    }

}
