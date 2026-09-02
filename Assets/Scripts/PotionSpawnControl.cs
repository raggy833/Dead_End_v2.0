using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionSpawnControl : MonoBehaviour
{
    [SerializeField] private ItemDatabase itemDatabase;
    [SerializeField] private Transform[] allPotionSpawnPos;

    [SerializeField] private Transform[] currentPotionPos;
    private int[] spawnedIndices;

    private void Start()
    {
        InitPotionSpawn();
    }

    private void InitPotionSpawn()
    {
        currentPotionPos = new Transform[2];
        spawnedIndices = new int[allPotionSpawnPos.Length];

        for (int i = 0; i < 2; i++)
        {
            int spawnIndex = GetRandomUnspawnedIndex();
            spawnedIndices[spawnIndex] = 1;

            // Random index for random potion
            int randIndex = Random.Range(0, itemDatabase.GetDatabaseLength());
            // Instantiate random potion
            GameObject potionObj = Instantiate(itemDatabase.GetItem(randIndex).item_prefab, allPotionSpawnPos[spawnIndex]);
            currentPotionPos[i] = allPotionSpawnPos[spawnIndex];
            potionObj.transform.SetParent(allPotionSpawnPos[spawnIndex]);
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

    public void RespawnPotion()
    {
        // Find a gear position that doesn't have a child
        List<int> unoccupiedIndices = new List<int>();
        for (int i = 0; i < allPotionSpawnPos.Length; i++)
        {
            if (allPotionSpawnPos[i].childCount == 0)
            {
                unoccupiedIndices.Add(i);
            }
        }

        if (unoccupiedIndices.Count == 0)
        {
            Debug.LogWarning("No unoccupied potion positions found!");
            return;
        }

        int respawnIndex = unoccupiedIndices[Random.Range(0, unoccupiedIndices.Count)];
        spawnedIndices[respawnIndex] = 1;
        // Random index for random potion
        int randIndex = Random.Range(0, itemDatabase.GetDatabaseLength());
        // Instantiate random potion
        Instantiate(itemDatabase.GetItem(randIndex).item_prefab, allPotionSpawnPos[respawnIndex]).transform.SetParent(allPotionSpawnPos[respawnIndex]);
    }
}
