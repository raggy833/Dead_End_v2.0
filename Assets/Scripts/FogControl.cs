using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FogControl : MonoBehaviour
{
    public float defaultTimerDuration = 300f; // 5 minutes in seconds (default value)
    public float timerDuration; // The current countdown timer duration
    public float fogIncreaseDuration = 10f; // The time it takes to fully increase fog amount
    public float fogDecreaseDuration = 10f; // The time it takes to fully decrease fog amount
    public float fogStartAmount = 4f; // The initial particle amount of fog
    public float fogTargetAmount = 8f; // The target particle amount of fog
    public ParticleSystem fogParticles; // Reference to the Particle System GameObject
    private Transform playerTransform;
    public TextMeshProUGUI fuseBoxCounter; // Reference to the TextMeshProGUI displaying countdown
    private Vector3 fogVelocity; // New variable to store the velocity of the fog particles
    private ParticleSystem.EmissionModule fogEmission; // Emission module of the Particle System
    [SerializeField] private GameObject fuseBoxTimerBackgroundPanel;
    private bool isTimerRunning = true;
    private float currentTimer;
    private bool isRedColor = false;
    public Color blackColor;
    public Color redColor;

    private bool isFogIncreasing = false; // Flag to track if fog is currently increasing
    private bool isFogDecreasing = false; // Flag to track if fog is currently decreasing

    public float followSpeed = 2f;

    private void Start()
    {
        isTimerRunning = true;
        playerTransform = FindObjectOfType<PlayerGunControl>().gameObject.transform;
        timerDuration = defaultTimerDuration;
        UpdateFuseBoxCounterText();

        // Get the emission module of the particle system
        fogEmission = fogParticles.emission;

        fogVelocity = Vector3.zero;
    }

    private void Update()
    {
        // Calculate a new target position with slight random movement
        Vector3 targetPosition = playerTransform.position;
        targetPosition += Random.insideUnitSphere * 0.5f; // Adjust the 0.5f value to control the amount of random movement

        // Move the fog gradually towards the new target position using Lerp
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref fogVelocity, followSpeed);

        if (isFogIncreasing)
        {
            IncreaseFog();
        }
        else if (isFogDecreasing)
        {
            DecreaseFog();
        }
        else
        {
            // Run countdown if neither increasing nor decreasing
            if (timerDuration > 0f)
            {
                isTimerRunning = true;
                timerDuration -= Time.deltaTime;
                UpdateFuseBoxCounterText();
            }
            else
            {
                // Called only once
                if (isTimerRunning)
                {
                    Debug.Log("Fusebox broken");
                    FindObjectOfType<FuseBox>().FuseboxBroken();
                }

                isTimerRunning = false;
                StartCoroutine(SwitchTimerPanelColor());

                if (fogEmission.rateOverTime.constant < fogTargetAmount)
                {

                    // Timer has run out, increase fog emission gradually to the target amount
                    float fogIncreaseRate = (fogTargetAmount - fogStartAmount) / fogIncreaseDuration;
                    float newFogAmount = fogEmission.rateOverTime.constant + fogIncreaseRate * Time.deltaTime;
                    fogEmission.rateOverTime = new ParticleSystem.MinMaxCurve(newFogAmount);
                }
                else
                {
                    // Timer has run out and fog emission reached the target amount
                    // Keep the fog emission at the target amount and stop updating the counter
                    fogEmission.rateOverTime = new ParticleSystem.MinMaxCurve(fogTargetAmount);
                }
            }

        }
    }

    private IEnumerator SwitchTimerPanelColor()
    {
        float colorSwitchDuration = 1f; // 1 second duration for each color transition
        float elapsedTime = 0f;

        Color startColor = blackColor;
        Color targetColor = redColor;

        while (!isTimerRunning)
        {
            if (elapsedTime < colorSwitchDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / colorSwitchDuration;
                SetFuseBoxTimerPanelColor(Color.Lerp(startColor, targetColor, t));
            }
            else
            {
                // Swap startColor and targetColor for the next transition
                Color tempColor = startColor;
                startColor = targetColor;
                targetColor = tempColor;

                elapsedTime = 0f; // Reset the elapsed time for the next transition
            }

            // Adjust the time delay as per your requirement
            yield return null;
        }

        // Ensure the timer panel color is back to the default color (black) when the timer is reset
        SetFuseBoxTimerPanelColor(blackColor);
    }

    private void SetFuseBoxTimerPanelColor(Color color)
    {
        Image timerBackgroundImage = fuseBoxTimerBackgroundPanel.GetComponent<Image>();
        timerBackgroundImage.color = color;
    }

    private void IncreaseFog()
    {
        if (fogEmission.rateOverTime.constant < fogTargetAmount)
        {
            float fogIncreaseRate = (fogTargetAmount - fogStartAmount) / fogIncreaseDuration;
            float newFogAmount = fogEmission.rateOverTime.constant + fogIncreaseRate * Time.deltaTime;
            fogEmission.rateOverTime = new ParticleSystem.MinMaxCurve(newFogAmount);
        }
        else
        {
            // Finished increasing fog, reset flags and timer
            isFogIncreasing = false;
            timerDuration = defaultTimerDuration;
            UpdateFuseBoxCounterText();
        }
    }

    private void DecreaseFog()
    {
        if (fogEmission.rateOverTime.constant > fogStartAmount)
        {
            float fogDecreaseRate = (fogTargetAmount - fogStartAmount) / fogDecreaseDuration;
            float newFogAmount = fogEmission.rateOverTime.constant - fogDecreaseRate * Time.deltaTime;
            fogEmission.rateOverTime = new ParticleSystem.MinMaxCurve(newFogAmount);
        }
        else
        {
            // Finished decreasing fog, reset flags and timer
            isFogDecreasing = false;
            timerDuration = defaultTimerDuration;
            UpdateFuseBoxCounterText();
        }
    }

    private void UpdateFuseBoxCounterText()
    {
        if (timerDuration <= 0)
        {
            fuseBoxCounter.text = string.Format("{0:00}:{1:00}", 0, 00);
        }
        else
        {
            int minutes = Mathf.FloorToInt(timerDuration / 60f);
            int seconds = Mathf.FloorToInt(timerDuration % 60f);
            fuseBoxCounter.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void StartFuseBoxCounter()
    {
        // Start the countdown timer from a different script
        timerDuration = defaultTimerDuration;
        UpdateFuseBoxCounterText();
    }

    public void FixFuseBox()
    {
        // Reset the countdown timer and start decreasing fog
        timerDuration = defaultTimerDuration;
        fogTargetAmount = fogStartAmount; // Set the target fog amount to the initial value
        isFogDecreasing = true;
        isTimerRunning = true;
    }
}
