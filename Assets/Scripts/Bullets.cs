using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    public float blinkDelay = 1f; // The time in seconds before the bullet starts blinking
    public float blinkDuration = 3f; // The time in seconds the bullet blinks before disappearing
    public float rotationSpeed = 100f;

    private Renderer[] renderers;
    private bool isBlinking = false;

    private void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        StartCoroutine(Blink());
    }


    private void Update()
    {
        // Rotate the bullet on the Y-axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private IEnumerator Blink()
    {
        yield return new WaitForSeconds(blinkDelay);
        isBlinking = true;

        float blinkingTime = 0f;
        while (isBlinking && blinkingTime < blinkDuration)
        {
            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = !renderer.enabled;
            }
            blinkingTime += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerGunControl playerGunControl = other.GetComponent<PlayerGunControl>();
            playerGunControl.PickUpBulletsItem();
            Destroy(gameObject);
        }
    }
}
