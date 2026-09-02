using UnityEngine;
using System.Collections;

public class Flicker : MonoBehaviour
{
    public float flickerTime = 0.5f;
    public float waitTime = 0.5f;
    public GameObject statusIcon;

    IEnumerator Start()
    {
        while (true)
        {
            statusIcon.SetActive(false);
            yield return new WaitForSeconds(flickerTime);
            statusIcon.SetActive(true);
            yield return new WaitForSeconds(flickerTime);
            statusIcon.SetActive(false);
            yield return new WaitForSeconds(flickerTime);
            statusIcon.SetActive(true);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
