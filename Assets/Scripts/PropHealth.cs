using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PropHealth : MonoBehaviour
{
    public bool fuseBox;
    public bool workbench;
    public bool pumpkin;

    public bool isBroken;

    public float health;
    private float lerpTimer;

    public float maxHealth = 100f;
    public float chipSpeed = 2f;
    public Image frontHealthBar;
    public Image backHealthBar;
    public TextMeshProUGUI healthNum;
    private StageControl stageControl;
    private Workbench workbenchScript;
    private FuseBox fuseBoxScript;

    // Start is called before the first frame update
    void Start()
    {
        workbenchScript = FindObjectOfType<Workbench>();
        fuseBoxScript = FindObjectOfType<FuseBox>();
        isBroken = false;
        health = maxHealth;
    }
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
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
    public void ReceiveDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            Broken();
        }
        lerpTimer = 0f;
        if (health <= 0 && pumpkin)
        {
            // Broken and gameover
            // stageControl.GameOver();
        }
    }
    public void Broken()
    {
        isBroken = true;
        if (workbench)
        {
            workbenchScript.WorkbenchBroken();
        }
        else if (fuseBox)
        {
            fuseBoxScript.FuseboxBroken();
        }
    }
    public void ToFullHealth()
    {
        AudioManager.instance.Play("hammer_fix_prop");
        isBroken = false;
        health = maxHealth;
    }
}
