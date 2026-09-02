using UnityEngine;

public class FlashColor : MonoBehaviour
{
    private Renderer renderer;
    private ParticleSystem particles;
    private bool isFlashing = false;

    void Start()
    {
        // Get the renderer component of the object
        renderer = GetComponent<Renderer>();

        // Add a particle system to the object
        particles = gameObject.AddComponent<ParticleSystem>();
        var main = particles.main;
        main.duration = 0.5f;
        main.startLifetime = 0.5f;
        main.startSize = 0.5f;
        main.startColor = Color.yellow;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        var emission = particles.emission;
        emission.rateOverTime = 20f;
        particles.Stop();

        StartFlashing();
    }

    // Start flashing the color
    public void StartFlashing()
    {
        if (!isFlashing)
        {
            InvokeRepeating("Flash", 0f, 0.5f);
            particles.Play();
            isFlashing = true;
        }
    }

    // Stop flashing the color
    public void StopFlashing()
    {
        if (isFlashing)
        {
            CancelInvoke("Flash");
            renderer.material.color = Color.white;
            particles.Stop();
            isFlashing = false;
        }
    }

    // Change the color of the object
    void Flash()
    {
        if (renderer.material.color == Color.yellow)
        {
            renderer.material.color = Color.white;
        }
        else
        {
            renderer.material.color = Color.yellow;
        }
    }
}
