using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeControl : MonoBehaviour
{
    private PlayerGunControl playerGunControl;
    // Start is called before the first frame update
    void Start()
    {
        playerGunControl = GetComponentInParent<PlayerGunControl>();
    }
    private void DamageEnemyInKnifeRange()
    {
        playerGunControl.DamageEnemyInKnifeRange();
    }
}
