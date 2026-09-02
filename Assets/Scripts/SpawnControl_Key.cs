using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnControl_Key : MonoBehaviour
{

    [SerializeField] private GameObject keyPrefab;
    [SerializeField] private Transform[] keySpawnPos;

    void Start()
    {
        SpawnKey();
    }

    private void SpawnKey()
    {

    }
}
