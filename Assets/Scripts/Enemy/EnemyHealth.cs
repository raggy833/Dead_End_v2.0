using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float zombieBaseHealth = 50f;
    public float spiderBaseHealth = 35f;
    public Slider health;
    public float maxHealth;
    public float curHealth;
    private StageControl stageControl;
    [SerializeField] private bool zombie;
    [SerializeField] private bool ghoul;
    [SerializeField] private bool lizardMonster;
    [SerializeField] private bool spider;
    private int criticalKillPoints = 100;
    private int normalKillPoints = 50;
    private int criticalHitPoints = 10;
    private int normalHitPoints = 5;

    // Start is called before the first frame update
    void Start()
    {
        if (ghoul)
        {
            health = GameObject.Find("BossHealth").GetComponent<Slider>();
            health.gameObject.SetActive(true);
        }
        else
        {
            health.gameObject.SetActive(false);
        }
        health.maxValue = maxHealth;
        health.value = maxHealth;
        curHealth = maxHealth;
        stageControl = FindObjectOfType<StageControl>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ReceiveDamage(float damage, bool critical)
    {
        health.gameObject.SetActive(true);
        curHealth -= damage;
        health.value = curHealth;
        if (curHealth <= 0)
        {
            if (critical)
            {
                stageControl.AddPoints(criticalKillPoints);
                AudioManager.instance.Play("bloodyDeath");

            }
            else
            {
                stageControl.AddPoints(normalKillPoints);
            }
            health.gameObject.SetActive(false);
            this.GetComponent<Enemy>().Dead(critical);

        }
        else
        {
            if (critical)
            {
                stageControl.AddPoints(criticalHitPoints);
            }
            else
            {
                stageControl.AddPoints(normalHitPoints);
            }
        }
    }
}
