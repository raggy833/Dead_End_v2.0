using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPoints : MonoBehaviour
{
    public float yRandNum;
    public float xRandNum;
    void Start()
    {
        Destroy(this.gameObject, 0.5f);
        xRandNum = Random.Range(-75, 75);
        yRandNum = Random.Range(125, 200);
    }
    private void Update()
    {
        gameObject.transform.position -= new Vector3(xRandNum * Time.deltaTime, yRandNum * Time.deltaTime, 0);
    }
}
