using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject explosionPrefab;
    public float explosionRadius = 5f;
    public float explosionForce = 1000f;
    public float fuseDelay = 3f;
    public float grenadeDamage = 100f;

    private bool isExploded = false;

    private void Start()
    {
        Invoke("Explode", fuseDelay);
    }

    private void Explode()
    {
        if (isExploded)
            return;

        isExploded = true;

        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        AudioManager.instance.Play("MetalImpact5");

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.GetComponentInParent<EnemyHealth>().ReceiveDamage(grenadeDamage, false);
            }

            Rigidbody rb = collider.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }

        Destroy(gameObject);
    }

    public void Throw(Vector3 direction)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(direction * 10f, ForceMode.Impulse);

        Vector3 offset = direction * 1f + Vector3.up * 0.75f;
        transform.position = transform.position + offset;
    }
}
