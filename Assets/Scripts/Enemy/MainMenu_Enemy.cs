using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MainMenu_Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    public NavMeshAgent Agent { get => agent; }

    public Transform startPosition;
    public Transform target;
    [SerializeField]
    private float cleanDeadTime;
    private float cleanDeadCounter;
    public float soundTimeMin;
    public float soundTimeMax;
    public float soundTime;
    public float soundTimeCounter;
    public int soundId;
    public float walkSpeed = 0.4f;

    [SerializeField] private bool zombie;
    [SerializeField] private bool ghoul;
    void Start()
    {
        cleanDeadCounter = 0;
        agent = GetComponent<NavMeshAgent>();
        soundTime = Random.Range(soundTimeMin, soundTimeMax);
        soundId = Random.Range(1, 3);
        this.GetComponent<Animator>().SetBool("Walking", true);
        agent.speed = walkSpeed;
    }

    void Update()
    {
        CheckDead();
    }
    private void CheckDead()
    {

        IdleSound();
        CheckState();

    }
    private void CheckState()
    {
        FollowTarget();
    }
    private void FollowTarget()
    {
        if (zombie)
        {
            // Running
            if (this.walkSpeed > 0.5)
            {
                this.GetComponent<Animator>().SetBool("Running", true);
                this.GetComponent<Animator>().SetBool("Walking", false);
                // Walking
            }
            else if (this.walkSpeed > 0)
            {
                this.GetComponent<Animator>().SetBool("Running", false);
                this.GetComponent<Animator>().SetBool("Walking", true);
            }
            float distance = Vector3.Distance(target.transform.position, agent.transform.position);
            agent.SetDestination(target.position);
            if (distance < 1f)
            {
                this.gameObject.transform.position = startPosition.position;
            }
        }
    }
    private void IdleSound()
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
}
