using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    public NavMeshAgent Agent { get => agent; }

    public GameObject player;
    public Transform currentTarget;
    public bool dead;
    [SerializeField]
    private float cleanDeadTime;
    private float cleanDeadCounter;
    public StageControl stageControl;
    public float soundTimeMin;
    public float soundTimeMax;
    public float soundTime;
    public float soundTimeCounter;
    public int soundId;
    public float walkSpeed = 0.4f;
    private bool attacking;
    [SerializeField] public bool zombie;
    [SerializeField] public bool ghoul;
    [SerializeField] public bool lizardMonster;
    [SerializeField] public bool spider;
    private bool ghoulActionWait = false;
    public float roarCounter = 10f;
    private float roarLagMin = 12f;
    private float roarLagMax = 15f;
    private bool roarWait = false;
    private bool waitingState = true;
    [SerializeField] private GameObject minimapIcon;
    [SerializeField] private GameObject energyBallPrefab;
    [SerializeField] private Transform[] energyBallPos = new Transform[3];
    [SerializeField] private Transform energyBallSpawnPos;

    [Header("Damage")]
    [SerializeField] private float zombieAttackDamage = 15f;

    void Start()
    {
        minimapIcon.SetActive(true);
        cleanDeadCounter = 0;
        agent = GetComponent<NavMeshAgent>();
        soundTime = Random.Range(soundTimeMin, soundTimeMax);
        soundId = Random.Range(1, 3);
        attacking = false;
        player = FindObjectOfType<PlayerLook>().gameObject;
        stageControl = FindObjectOfType<StageControl>();
        agent.speed = walkSpeed;
    }
    void Update()
    {
        CheckDead();
    }
    private void CheckDead()
    {
        if (zombie || spider)
        {
            if (dead)
            {
                cleanDeadCounter += Time.deltaTime;
                if (cleanDeadCounter >= cleanDeadTime)
                {
                    Destroy(this.gameObject);
                }
            }
            else
            {
                if (currentTarget != null)
                {
                    IdleSound();
                    CheckState();
                }
                else
                {
                    currentTarget = player.transform;
                }
            }
        }
        else if (ghoul)
        {
            if (!dead)
            {
                // idle sound?
                GhoulFollowTarget();
            }
        }
        else if (lizardMonster)
        {
            if (!dead)
            {
                // idle sound?
                LizardMonsterCheckState();
            }
        }
    }
    private void GhoulFollowTarget()
    {
        float distance = Vector3.Distance(currentTarget.transform.position, agent.transform.position);
        agent.SetDestination(currentTarget.position);
        roarCounter -= Time.deltaTime;
        if (roarCounter < 0 && !attacking)
        {
            int temp = Random.Range(0, 2);
            if (temp == 0)
            {
                StartCoroutine(GhoulEnergyBall());
            }
            else if (temp == 1)
            {
                StartCoroutine(GhoulRoar());
            }
        }
        else if (distance >= 5f)
        {
            // FollowTarget();
        }
        else if (distance < 5f && !attacking)
        {
            StartCoroutine(GhoulAttack());
        }
    }
    private void LizardMonsterCheckState()
    {
        // TODO: Test

        currentTarget = player.transform;
        float distance = Vector3.Distance(currentTarget.transform.position, agent.transform.position);
        agent.SetDestination(player.transform.position);
        Debug.Log(agent.destination);
        // If Player gets too close, target will change to player
        float distanceToPlayer = Vector3.Distance(player.transform.position, agent.transform.position);
        Debug.Log(distanceToPlayer);
        if (currentTarget != player.transform && distanceToPlayer < 3)
        {
            currentTarget = player.transform;
        }
        else
        {

            agent.SetDestination(currentTarget.position);
            if (distance >= 1.5f)
            {
                FollowTarget();
            }
            else if (distance < 1.5f && !attacking)
            {
                StartCoroutine(AttackPlayer());
            }
            else
            {

            }
        }
    }
    private void CheckState()
    {
        // Target is prop 
        if (currentTarget.GetComponent<PropHealth>() != null)
        {
            // Check if broken
            if (currentTarget.GetComponent<PropHealth>().isBroken)
            {
                currentTarget = player.transform;
            }
        }
        float distance = Vector3.Distance(currentTarget.transform.position, agent.transform.position);
        // If Player gets too close, target will change to player
        float distanceToPlayer = Vector3.Distance(player.transform.position, agent.transform.position);
        if (currentTarget != player.transform && distanceToPlayer < 3)
        {
            currentTarget = player.transform;
        }
        else
        {

            agent.SetDestination(currentTarget.position);
            if (distance >= 1.5f)
            {
                FollowTarget();
            }
            else if (distance < 1.5f && !attacking)
            {
                StartCoroutine(AttackPlayer());
            }
            else
            {

            }
        }
    }
    IEnumerator GhoulAttack()
    {
        attacking = true;
        agent.speed = 0;
        this.GetComponent<Animator>().SetTrigger("attack1");
        yield return new WaitForSeconds(.5f);
        float distance = Vector3.Distance(currentTarget.transform.position, agent.transform.position);
        if (distance <= 5f)
        {
            currentTarget.gameObject.GetComponent<PlayerHealth>().TakeDamage(15);
            DI_System.CreateIndicator(this.transform);
            // if (!DI_System.CheckIfObjectInSight(this.transform))
            // {
            //     DI_System.CreateIndicator(this.transform);
            // }
        }
        yield return new WaitForSeconds(.5f);
        attacking = false;
        agent.speed = 1f;
    }
    IEnumerator GhoulRoar()
    {
        roarCounter = Random.Range(roarLagMin, roarLagMax);
        attacking = true;
        agent.speed = 0;
        AudioManager.instance.Play("ghoul_roar");
        this.GetComponent<Animator>().SetTrigger("roar");
        yield return new WaitForSeconds(1f);
        this.GetComponent<TraumaInducer>().CameraShake();
        yield return new WaitForSeconds(2.5f);
        stageControl.BossRoarSpawnZombie();
        yield return new WaitForSeconds(2f);
        attacking = false;
        agent.speed = 1f;

    }
    IEnumerator GhoulEnergyBall()
    {
        attacking = true;
        agent.speed = 0;
        this.GetComponent<Animator>().SetTrigger("energyBall");
        yield return new WaitForSeconds(2f);
        // Energy ball test
        for (int i = 0; i < 3; i++)
        {
            GameObject energyBallClone = Instantiate(energyBallPrefab, energyBallSpawnPos.position, Quaternion.identity) as GameObject;
            energyBallClone.GetComponent<BossEnergyBall>().FindMovePos(energyBallPos[i].transform.position);
            yield return new WaitForSeconds(.5f);
        }
        yield return new WaitForSeconds(2.5f);
        attacking = false;
        agent.speed = 1f;
    }
    private void FollowTarget()
    {
        if (zombie)
        {
            // Running
            if (this.walkSpeed > 1.5)
            {
                this.GetComponent<Animator>().SetBool("Running", true);
                this.GetComponent<Animator>().SetBool("Walking", false);
                this.GetComponent<Animator>().SetBool("Walking2", false);
                this.GetComponent<Animator>().SetBool("Walking3", false);
                // Walking
            }
            else if (this.walkSpeed > 0)
            {
                // Running animation off
                this.GetComponent<Animator>().SetBool("Running", false);
                // Random number for different walking animation
                int rand = Random.Range(0, 2);
                if (rand == 0)
                {
                    this.GetComponent<Animator>().SetBool("Walking", true);
                }
                else if (rand == 1)
                {
                    this.GetComponent<Animator>().SetBool("Walking2", true);
                }
                else if (rand == 2)
                {
                    this.GetComponent<Animator>().SetBool("Walking3", true);
                }
            }
            float distance = Vector3.Distance(currentTarget.transform.position, agent.transform.position);
            agent.SetDestination(currentTarget.position);
        }
        else if (ghoul)
        {
            //this.GetComponent<Animation>().Play("Walk");
        }
        else if (lizardMonster)
        {
            if (this.walkSpeed > 1)
            {
                this.GetComponent<Animator>().SetBool("Walking", true);
            }
            float distance = Vector3.Distance(currentTarget.transform.position, agent.transform.position);
            agent.SetDestination(currentTarget.position);
        }
        else if (spider)
        {
            this.GetComponent<Animator>().SetBool("Walking", true);
            float distance = Vector3.Distance(currentTarget.transform.position, agent.transform.position);
            agent.SetDestination(currentTarget.position);
        }
    }
    IEnumerator AttackPlayer()
    {
        if (zombie)
        {
            attacking = true;
            agent.speed = 0;
            this.GetComponent<Animator>().SetBool("Walking", false);
            this.GetComponent<Animator>().SetBool("Walking2", false);
            this.GetComponent<Animator>().SetBool("Walking3", false);
            this.GetComponent<Animator>().SetTrigger("Attack");
            yield return new WaitForSeconds(.5f);
            float distance = Vector3.Distance(currentTarget.transform.position, agent.transform.position);
            if (distance <= 1.6f)
            {
                if (currentTarget.gameObject.GetComponent<PlayerHealth>() != null)
                {
                    currentTarget.gameObject.GetComponent<PlayerHealth>().TakeDamage(zombieAttackDamage);
                    DI_System.CreateIndicator(this.transform);
                }
                else if (currentTarget.gameObject.GetComponent<PropHealth>() != null)
                {
                    // Decrease health from prop
                    currentTarget.gameObject.GetComponent<PropHealth>().ReceiveDamage(zombieAttackDamage);
                }
            }
            yield return new WaitForSeconds(.35f);
            // Check if dead during attack animation
            if (!dead)
            {
                agent.speed = walkSpeed;
            }
            attacking = false;
        }
        else if (lizardMonster)
        {
            attacking = true;
            agent.speed = 0;
            this.GetComponent<Animator>().SetBool("Walking", false);
            this.GetComponent<Animator>().SetBool("Walking2", false);
            this.GetComponent<Animator>().SetBool("Walking3", false);
            this.GetComponent<Animator>().SetTrigger("Attack");
            yield return new WaitForSeconds(.5f);
            float distance = Vector3.Distance(currentTarget.transform.position, agent.transform.position);
            if (distance <= 1.5f)
            {
                if (currentTarget.gameObject.GetComponent<PlayerHealth>() != null)
                {
                    currentTarget.gameObject.GetComponent<PlayerHealth>().TakeDamage(15);
                    DI_System.CreateIndicator(this.transform);
                }
                else if (currentTarget.gameObject.GetComponent<PropHealth>() != null)
                {
                    // Decrease health from prop
                    currentTarget.gameObject.GetComponent<PropHealth>().ReceiveDamage(15);
                }
            }
            yield return new WaitForSeconds(.35f);
            // Check if dead during attack animation
            if (!dead)
            {
                agent.speed = walkSpeed;
            }
            attacking = false;
        }
        else if (spider)
        {
            attacking = true;
            agent.speed = 0;
            this.GetComponent<Animator>().SetBool("Walking", false);
            this.GetComponent<Animator>().SetTrigger("Attack");
            yield return new WaitForSeconds(.5f);
            float distance = Vector3.Distance(currentTarget.transform.position, agent.transform.position);
            if (distance <= 1.6f)
            {
                if (currentTarget.gameObject.GetComponent<PlayerHealth>() != null)
                {
                    currentTarget.gameObject.GetComponent<PlayerHealth>().TakeDamage(zombieAttackDamage);
                    DI_System.CreateIndicator(this.transform);
                }
                else if (currentTarget.gameObject.GetComponent<PropHealth>() != null)
                {
                    // Decrease health from prop
                    currentTarget.gameObject.GetComponent<PropHealth>().ReceiveDamage(zombieAttackDamage);
                }
            }
            yield return new WaitForSeconds(.35f);
            // Check if dead during attack animation
            if (!dead)
            {
                agent.speed = walkSpeed;
            }
            attacking = false;
        }
    }
    private void IdleSound()
    {
        if (zombie)
        {
            // Change depending on enemy ID
            soundTimeCounter += Time.deltaTime;
            if (soundTimeCounter > soundTime)
            {
                this.GetComponent<AudioSource>().Play();
                soundTime = Random.Range(soundTimeMin, soundTimeMax);
                soundId = Random.Range(1, 3);
                soundTimeCounter = 0;
            }
        }
        else if (spider)
        {

        }

    }
    public void Dead(bool crit)
    {
        if (ghoul)
        {
            this.dead = true;
            Debug.Log("ghoul is dead");
            agent.speed = 0;
            this.GetComponent<CapsuleCollider>().isTrigger = true;
            this.GetComponent<CapsuleCollider>().enabled = false;
            //　agent.enabled = false;
            // this.GetComponent<Animation>().Stop("Walk");
            // this.GetComponent<Animation>().Play("Death");
            // this.GetComponent<Animation>()["Death"].speed = 0.25f;
            this.GetComponent<Animator>().SetTrigger("Dead");
            Time.timeScale = 0.5f;
            Invoke("ShowResultPanel", 2f);
        }
        else if (zombie)
        {
            agent.speed = 0;
            agent.enabled = false;
            minimapIcon.SetActive(false);
            stageControl.UpdateWaveEnemyNum(1, this.name);
            this.GetComponent<CapsuleCollider>().isTrigger = true;
            this.GetComponent<CapsuleCollider>().enabled = false;
            this.GetComponent<Animator>().SetBool("Walking", false);
            this.GetComponent<Animator>().SetBool("Walking2", false);
            this.GetComponent<Animator>().SetBool("Walking3", false);
            this.GetComponent<Animator>().SetBool("Dead", true);
            this.GetComponentInChildren<ZombieHead>().gameObject.SetActive(false);
            AudioManager.instance.Play("Zombie_Hurt3");
            this.dead = true;
        }
        else if (spider)
        {
            agent.speed = 0;
            agent.enabled = false;
            minimapIcon.SetActive(false);
            stageControl.UpdateWaveEnemyNum(1, this.name);
            this.GetComponent<CapsuleCollider>().isTrigger = true;
            this.GetComponent<CapsuleCollider>().enabled = false;
            this.GetComponent<Animator>().SetBool("Walking", false);
            this.GetComponent<Animator>().SetBool("Dead", true);
            // TODO change to spider sound
            AudioManager.instance.Play("Zombie_Hurt3");
            this.dead = true;
        }
    }
    public void ShowResultPanel()
    {
        Time.timeScale = 0f;
        stageControl.ShowResultPanel();
    }
}
