using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PouchItemSpawnControl : MonoBehaviour
{
    [SerializeField] private GameObject gearPrefab;
    [SerializeField] private Transform[] allGearSpawnPos;

    [SerializeField] private Transform[] currentGearPos;
    private int[] spawnedIndices;

    private void Start()
    {
        InitGearSpawn();
    }

    private void InitGearSpawn()
    {
        currentGearPos = new Transform[2];
        spawnedIndices = new int[allGearSpawnPos.Length];

        for (int i = 0; i < 2; i++)
        {
            int spawnIndex = GetRandomUnspawnedIndex();
            spawnedIndices[spawnIndex] = 1;

            GameObject gearObj = Instantiate(gearPrefab, allGearSpawnPos[spawnIndex]);
            currentGearPos[i] = allGearSpawnPos[spawnIndex];
            gearObj.transform.SetParent(allGearSpawnPos[spawnIndex]);
        }
    }

    private int GetRandomUnspawnedIndex()
    {
        List<int> unspawnedIndices = new List<int>();
        for (int i = 0; i < spawnedIndices.Length; i++)
        {
            if (spawnedIndices[i] == 0)
            {
                unspawnedIndices.Add(i);
            }
        }

        if (unspawnedIndices.Count == 0)
        {
            for (int i = 0; i < spawnedIndices.Length; i++)
            {
                spawnedIndices[i] = 0;
            }

            unspawnedIndices.AddRange(spawnedIndices);
        }

        int randomIndex = Random.Range(0, unspawnedIndices.Count);
        return unspawnedIndices[randomIndex];
    }

    public void RespawnGear(Transform gearPos)
    {
        // Find a gear position that doesn't have a child
        List<int> unoccupiedIndices = new List<int>();
        for (int i = 0; i < allGearSpawnPos.Length; i++)
        {
            if (allGearSpawnPos[i].childCount == 0)
            {
                unoccupiedIndices.Add(i);
            }
        }

        if (unoccupiedIndices.Count == 0)
        {
            Debug.LogWarning("No unoccupied gear positions found!");
            return;
        }

        int respawnIndex = unoccupiedIndices[Random.Range(0, unoccupiedIndices.Count)];
        spawnedIndices[respawnIndex] = 1;
        Instantiate(gearPrefab, allGearSpawnPos[respawnIndex]).transform.SetParent(allGearSpawnPos[respawnIndex]);
    }
}
