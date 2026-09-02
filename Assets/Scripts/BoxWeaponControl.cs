using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxWeaponControl : MonoBehaviour
{
    public GameObject boxWeapon;
    public GameObject effect;
    public GunDatabase gunDatabase;
    public int gunId;

    void Start()
    {
        // Effect
        Instantiate(effect, this.transform.position, this.transform.rotation);
        // Instantiate weapon
        Invoke("SpawnWeapon", 3f);
    }
    private void SpawnWeapon()
    {
        gunId = Random.Range(0, gunDatabase.GetDatabaseLength());
        GameObject boxWeaponClone = Instantiate(boxWeapon, new Vector3(this.transform.position.x, this.transform.position.y + 1f, this.transform.position.z), this.transform.rotation) as GameObject;
        GameObject gunClone = Instantiate(gunDatabase.GetGun(gunId).gameObject, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        gunClone.transform.parent = boxWeaponClone.transform;
        gunClone.transform.position = new Vector3(0, 0, 0);
        gunClone.GetComponent<Animator>().enabled = false;
        boxWeaponClone.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
    }
}
