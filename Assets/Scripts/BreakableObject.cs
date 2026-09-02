using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    public GameObject breakEffectPrefab; // Prefab for the break effect
    public GameObject bulletPrefab; // Prefab for the bullet

    public void Break()
    {
        Debug.Log("Break called");
        // Instantiate the break effect at the current object's location
        Instantiate(breakEffectPrefab, transform.position, Quaternion.identity);

        // Instantiate a random chance for dropping a bullet
        float dropChance = 1f; // Set the drop chance as desired (e.g., 50%)
        if (Random.value <= dropChance)
        {
            // Calculate the offset position relative to the current object's transform
            Vector3 spawnPosition = new Vector3(transform.position.x, 1, transform.position.z);

            // Instantiate the bullet prefab at the offset position and with no rotation
            Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
        }

        // Destroy the object
        Destroy(gameObject);
    }
}
