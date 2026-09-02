using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    private void Start()
    {
        Destroy();
    }
    public void Destroy()
    {
        Destroy(gameObject, 1f);
    }
}
