using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneHitEffectControl : MonoBehaviour
{
    [SerializeField] private float destroyLag;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, destroyLag);
    }
}
