using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    public float destroyTime = 0.5f;
    private void Start()
    {
        Destroy();
    }
    public void Destroy()
    {
        Destroy(gameObject, destroyTime);
    }
}
