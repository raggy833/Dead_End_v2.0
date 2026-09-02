using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnergyBall : MonoBehaviour
{
    [SerializeField] private Vector3 target = Vector3.zero;
    [SerializeField] private float toPosSpeed;
    [SerializeField] private float toPlayerSpeed;
    [SerializeField] private bool movingToTarget = false;
    [SerializeField] private bool movingToPlayer = false;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    private void Update()
    {
        if (movingToTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, toPosSpeed);
            if (transform.position == target)
            {
                Debug.Log("Energy ball reached target");
                movingToTarget = false;
                movingToPlayer = true;
                target = FindObjectOfType<PlayerGunControl>().transform.position;
            }
        }
        if (movingToPlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, toPlayerSpeed);
            if (transform.position == target)
            {
                Debug.Log("Energy ball reached target");
                movingToTarget = false;
                movingToPlayer = false;
                Destroy(this.gameObject);
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Hit player!");
            Destroy(this.gameObject);
        }
    }
    public void FindMovePos(Vector3 newTarget)
    {
        Debug.Log("Energy ball found new target");
        target = newTarget;
        movingToTarget = true;
        movingToPlayer = false;
    }
}
