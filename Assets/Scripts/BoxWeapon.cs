using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxWeapon : MonoBehaviour
{
    private Vector3 targetPos;
    private float speed = 0.05f;

    private void Start()
    {
        this.GetComponentInChildren<GunControl>().gameObject.transform.position = new Vector3(0f, 0f, 0f);
        targetPos = new Vector3(this.transform.position.x, this.transform.position.y - 1, this.transform.position.z);
        Destroy(this.gameObject, 10f);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }
}
