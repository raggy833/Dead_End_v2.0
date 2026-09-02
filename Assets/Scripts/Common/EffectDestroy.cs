using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDestroy : MonoBehaviour
{
    [SerializeField] private float destroyWaitTime;

    private void Start()
    {
        Destroy();
    }
    public void Destroy()
    {
        Destroy(gameObject, destroyWaitTime);
    }
}
