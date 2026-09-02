using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    private StageControl stageControl;

    [SerializeField] private GameObject GunPos;
    [SerializeField] private GameObject KnifePos;

    public float health;
    private float lerpTimer;
    private bool isDead;
    [Space(20)]
    [Header("Health Bar")]
    public float maxHealth = 100f;
    public float chipSpeed = 2f;
    public Image frontHealthBar;
    public Image backHealthBar;
    [Space(20)]
    [Header("Shield Bar")]
    public Image shieldBar;
    public float curMaxShield;
    public bool shieldActive;

    [Space(20)]
    [Header("Damage Overlay")]
    public Image overlay;
    public float duration;
    public float fadeSpeed;

    private float durationTimer;

    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    private void Setup()
    {
        stageControl = FindObjectOfType<StageControl>();
        health = maxHealth;
        shieldActive = false;
        curMaxShield = 0f;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
        isDead = false;
    }

    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
        if (overlay.color.a > 0 & !isDead)
        {
            durationTimer += Time.deltaTime;
            if (durationTimer > duration)
            {
                // fade the image
                float tempAlpha = overlay.color.a;
                tempAlpha -= Time.deltaTime * fadeSpeed;
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);
            }
        }
    }
    public void UpdateHealthUI()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = health / maxHealth;
        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
        if (fillF < hFraction)
        {
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            // percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete);
        }
    }
    public void AddShield(float newAddAmount)
    {
        shieldActive = true;

        if (curMaxShield < newAddAmount)
        {
            // new shield is larger
            curMaxShield = newAddAmount;
            //shieldBar.fillAmount = newAddAmount;
        }
        else
        {
            // current shield is larger
            float addFraction = newAddAmount / curMaxShield;
            //shieldBar.fillAmount += Mathf.Round(addFraction * 100.0f) * 0.01f;
        }
    }
    public void TakeDamage(float damage)
    {
        if (isDead)
        {
            // Shield on
            // float minusFraction = damage / curMaxShield;
            // shieldBar.fillAmount -= Mathf.Round(minusFraction * 100.0f) * 0.01f;

            // if (shieldBar.fillAmount <= 0.02f)
            // {
            //     shieldBar.fillAmount = 0f;
            //     shieldActive = false;
            // }
        }
        else
        {

            health -= damage;

            //---------------------
            //Player health is 0 (Gameover)
            //---------------------
            if (health <= 0)
            {
                isDead = true;
                health = 0;
                this.GetComponent<Animator>().SetBool("isDead", true);
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0.2f);
                GunPos.SetActive(false);
                KnifePos.SetActive(false);
                AudioManager.instance.Play("male_death_voice");
                StartCoroutine(ShowGameOverWindow());
            }
            lerpTimer = 0f;
            durationTimer = 0;
            overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0.2f);
        }
    }
    private IEnumerator ShowGameOverWindow()
    {
        yield return new WaitForSeconds(2f);
        stageControl.GameOver();
    }

    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        lerpTimer = 0f;
    }
}
