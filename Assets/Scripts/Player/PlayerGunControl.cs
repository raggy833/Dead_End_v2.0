using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;

public class PlayerGunControl : MonoBehaviour
{
    public GunDatabase gunDatabase;
    private GunDatabase curGunDatabase;
    public Gun primaryGun;
    public Gun secondaryGun;
    public int primaryWeaponId;
    public GameObject primaryGo;
    public int secondaryWeaponId;
    private GameObject secondaryGo;
    public Transform gunHoldPos;
    public GameObject shootEffect;
    private Transform shootEffectPos;
    private string primaryShootSound;
    private string primaryReloadSound;

    private Vector3 shootOffset;

    public bool aiming;
    private Camera cam;

    private float shootCooldown;
    private float shootTimer;
    [Header("Melee")]
    [SerializeField] public GameObject knifeBtn;
    [SerializeField] public GameObject knifeBtn2;
    [SerializeField] public GameObject knifeGo;
    [SerializeField] public float knifeDistance = 5f;
    [SerializeField] private float lastAttackTime = -Mathf.Infinity;
    [SerializeField] private float knifeAttackDelay = 0.5f;
    [SerializeField] private bool knifeAttacking = false;
    [SerializeField] private float remainingTime;
    private GameObject skullInRange;

    [Header("Character buff")]
    private bool handgunBuff;
    private bool shotgunBuff;
    private bool knifeBuff;

    [Header("Grenade")]
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] public int grenade_in_bag;
    [SerializeField] public TextMeshProUGUI grenade_num_UI;
    [Header("Buff")]
    public float damageBuff;
    [Header("Primary")]
    public int primary_mag_current;
    public int primary_mag_current_max;
    public int primary_ammoTotal_current;
    public int primary_ammoTotal_current_max;
    [Header("Secondary")]
    public int secondary_mag_current;
    public int secondary_mag_current_max;
    public int secondary_ammoTotal_current;
    public int secondary_ammoTotal_current_max;
    public Image secondaryImage;
    public Color secondaryImageColor = new Color32(255, 255, 255, 255);
    [Header("Crosshair")]
    [SerializeField] private GameObject crosshair_inrange;
    public float crosshairHitTimer = 0.7f;
    private float crosshairHitTime = 0f;
    [Header("Sensitivity")]
    private float xSensitivityHip = 10f;
    private float ySensitivityHip = 5f;
    private float xSensitivityAim = 3f;
    private float ySensitivityAim = 1f;

    public TextMeshProUGUI gunAmmoText;
    public GameObject hitEffect;
    public GameObject creatureHitEffect;
    [SerializeField] private GameObject stoneHitEffect;

    [SerializeField]
    private float btnCooldown;
    [SerializeField]
    private bool canPressBtn;
    [SerializeField]
    private float shootBtnCooldown;
    private bool canShoot;
    private StageControl stageControl;

    public float distance;
    [SerializeField]
    private LayerMask mask;
    [SerializeField]
    private GameObject hipCrosshair;
    [SerializeField]
    private GameObject aimCrosshair;
    private int hipLookSpeed;
    private int aimLookSpeed;
    private Recoil recoil_script;

    void Start()
    {
        Setup();
    }
    public void Setup()
    {
        damageBuff = 1f;

        primaryWeaponId = 0;
        secondaryWeaponId = 99; // default is 99
        //knifeBtn.SetActive(false);

        primaryGun = gunDatabase.GetGun(primaryWeaponId);
        UpdatePrimaryGun(primaryGun);

        //---------------------
        // Init gun stats
        // Update current value and max value of mag and ammo for primary gun
        //---------------------
        // update current max value
        primaryGun.mag_current_max_InGame = gunDatabase.GetGun(primaryWeaponId).mag_lv1_value + ((gunDatabase.GetGun(primaryWeaponId).mag_currentLv - 1) * gunDatabase.GetGun(primaryWeaponId).mag_IncreasePerUpgrade);
        primaryGun.ammoTotal_current_max_InGame = gunDatabase.GetGun(primaryWeaponId).ammoTotal_lv1_value + ((gunDatabase.GetGun(primaryWeaponId).ammoTotal_currentLv - 1) * gunDatabase.GetGun(primaryWeaponId).ammoTotal_IncreasePerUpgrade);
        primary_mag_current_max = primaryGun.mag_current_max_InGame;
        primary_ammoTotal_current_max = primaryGun.ammoTotal_current_max_InGame;
        // update current value
        primaryGun.mag_current_value_InGame = primaryGun.mag_current_max_InGame;
        primaryGun.ammoTotal_current_value_InGame = primaryGun.ammoTotal_current_max_InGame;
        primary_mag_current = primaryGun.mag_current_value_InGame;
        primary_ammoTotal_current = primaryGun.ammoTotal_current_value_InGame;

        stageControl = FindObjectOfType<StageControl>();
        recoil_script = transform.Find("CameraRecoil").GetComponent<Recoil>();

        // TODO: fix
        //secondaryGun = gunDatabase.GetGun(secondaryWeaponId);

        cam = GetComponent<PlayerLook>().cam;
        aiming = false;
        canPressBtn = true;

        // Update buffs by character id
        UpdateCharacterBuffs();

        UpdateGunMagAmmoMaxValues();

        UpdateAmmoUI();
    }
    private void UpdateCharacterBuffs()
    {
        int currentCharacterId = stageControl.GetCharacterId();
        if (currentCharacterId == 1)
        {
            Debug.Log("Character id is 1");
        }
        else if (currentCharacterId == 2)
        {
            Debug.Log("Character id is 2");
        }
        else if (currentCharacterId == 3)
        {
            Debug.Log("Character id is 3");
        }
        else if (currentCharacterId == 4)
        {
            Debug.Log("Character id is 4");
        }
    }

    //==============================================
    //-----UpdateGunMagAmmoMaxValues-----
    //Description: Update the max value of primary and secondary guns
    //
    //----Parameters-----------------
    // None
    //----Return---------------------
    // None
    //==============================================
    public void UpdateGunMagAmmoMaxValues()
    {
        // Update primary gun after upgrade
        primaryGun.mag_current_max_InGame = gunDatabase.GetGun(primaryWeaponId).mag_lv1_value + ((gunDatabase.GetGun(primaryWeaponId).mag_currentLv - 1) * gunDatabase.GetGun(primaryWeaponId).mag_IncreasePerUpgrade);
        primaryGun.ammoTotal_current_max_InGame = gunDatabase.GetGun(primaryWeaponId).ammoTotal_lv1_value + ((gunDatabase.GetGun(primaryWeaponId).ammoTotal_currentLv - 1) * gunDatabase.GetGun(primaryWeaponId).ammoTotal_IncreasePerUpgrade);
        primary_mag_current_max = primaryGun.mag_current_max_InGame;
        primary_ammoTotal_current_max = primaryGun.ammoTotal_current_max_InGame;
        if (secondaryWeaponId != 99)
        {
            // Update secondaray gun after upgrade
            secondaryGun.mag_current_max_InGame = gunDatabase.GetGun(secondaryWeaponId).mag_lv1_value + (gunDatabase.GetGun(secondaryWeaponId).mag_currentLv * gunDatabase.GetGun(secondaryWeaponId).mag_IncreasePerUpgrade);
            secondaryGun.ammoTotal_current_max_InGame = gunDatabase.GetGun(secondaryWeaponId).ammoTotal_lv1_value + (gunDatabase.GetGun(secondaryWeaponId).ammoTotal_currentLv * gunDatabase.GetGun(secondaryWeaponId).ammoTotal_IncreasePerUpgrade);
        }
    }
    public void UpdateDatabaseMagAmmoCurrentValues()
    {
        gunDatabase.GetGun(primaryWeaponId).mag_current_value_InGame = primary_mag_current;
        gunDatabase.GetGun(primaryWeaponId).ammoTotal_current_value_InGame = primary_ammoTotal_current;

        // if (secondaryWeaponId != 99)
        // {
        //     // Update secondaray gun after upgrade
        //     secondaryGun.mag_current_max_InGame = gunDatabase.GetGun(secondaryWeaponId).mag_lv1_value + (gunDatabase.GetGun(secondaryWeaponId).mag_currentLv * gunDatabase.GetGun(secondaryWeaponId).mag_IncreasePerUpgrade);
        //     secondaryGun.ammoTotal_current_max_InGame = gunDatabase.GetGun(secondaryWeaponId).ammoTotal_lv1_value + (gunDatabase.GetGun(secondaryWeaponId).ammoTotal_currentLv * gunDatabase.GetGun(secondaryWeaponId).ammoTotal_IncreasePerUpgrade);
        // }
    }
    void Update()
    {
        KnifeStatusCheck();

        // Crosshair effect
        CheckCrosshair();
        if (shootTimer >= 0)
        {
            shootTimer -= Time.deltaTime;
        }

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.green);
        RaycastHit hitInfo; // variable to store our collision information
        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            // Debug.Log(hitInfo.collider.name);

            if (hitInfo.collider.GetComponent<Enemy>() != null)
            {
                // Display red crosshair
                CrosshairHitRed();

                // Enemy is in melee range
                if (Vector3.Distance(transform.position, hitInfo.transform.position) < knifeDistance)
                {
                    Debug.Log("In knife distance");
                    knifeBtn.SetActive(true);
                }
                else
                {
                    knifeBtn.SetActive(false);
                }
            }
            else
            {
                CrosshairHitWhite();
                knifeBtn.SetActive(false);
            }
        }
    }
    public void KnifeStatusCheck()
    {
        if (knifeAttacking)
        {
            float timeSinceAttack = Time.time - lastAttackTime;
            if (timeSinceAttack >= knifeAttackDelay + remainingTime)
            {
                knifeAttacking = false;
                knifeGo.SetActive(false);
                ShowGunGo();
            }
        }
    }
    public void KnifeAnimationEnd()
    {
        lastAttackTime = 0;
        KnifeStatusCheck();
    }
    public void HideGunGo()
    {
        primaryGo.SetActive(false);
    }
    public void ShowGunGo()
    {
        primaryGo.SetActive(true);
    }
    public void KnifeAttack()
    {
        HideGunGo();
        knifeGo.SetActive(true);

        if (!knifeAttacking)
        {
            // If this is the first attack, play the KnifeAttack1 animation
            knifeAttacking = true;
            lastAttackTime = Time.time;
            knifeGo.GetComponent<Animator>().SetTrigger("KnifeAttack1");
        }
    }
    public void DamageEnemyInKnifeRange()
    {
        AudioManager.instance.Play("swoosh");
        // Find all EnemyHealth objects in scene
        EnemyHealth[] enemies = FindObjectsOfType<EnemyHealth>();
        if (skullInRange != null)
        {
            skullInRange.GetComponent<BreakableObject>().Break();
            knifeBtn2.SetActive(false);
            return;
        }
        // Find the closest enemy
        EnemyHealth closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        foreach (EnemyHealth enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestEnemy = enemy;
                closestDistance = distance;
            }
        }
        Debug.Log(closestEnemy.gameObject.name);
        // Check if closest enemy is within range, and call function on its script
        if (closestEnemy != null && closestDistance <= knifeDistance)
        {
            closestEnemy.ReceiveDamage(10 * damageBuff, false);
        }
    }
    private bool IsInView(Transform transform)
    {
        Vector3 point = cam.WorldToViewportPoint(transform.position);
        return point.z > 0 && point.x > 0 && point.x < 1 && point.y > 0 && point.y < 1;
    }
    public void Shoot()
    {
        if (shootTimer >= 0)
        {
            return;
        }

        if (canPressBtn && !knifeAttacking)
        {
            shootTimer += shootCooldown;
            // No in magazine ammo
            if (!CheckAmmo())
            {
                AudioManager.instance.Play("empty_click");
                return;
            }
            primary_mag_current -= 1;
            UpdateAmmoUI();
            AudioManager.instance.Play(primaryShootSound);

            if (aiming)
            {
                shootOffset = new Vector3(0f, 0f, 0f);
                primaryGo.GetComponent<Animator>().SetTrigger("AimingShoot");
                // Call recoil function
                recoil_script.RecoilFire();
            }
            else
            {
                float tempX = UnityEngine.Random.Range(-0.01f, 0.01f);
                float tempY = UnityEngine.Random.Range(-0.01f, 0.01f);
                shootOffset = new Vector3(tempX, tempY, 0);
                primaryGo.GetComponent<Animator>().SetTrigger("Shoot");
                // Call recoil function
                recoil_script.RecoilFire();
            }

            GameObject explosion = Instantiate(shootEffect, shootEffectPos.position, Quaternion.identity, shootEffectPos) as GameObject;
            explosion.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            // If shotgun
            if (primaryGun.gunType == Gun.GunType.ShotGun)
            {
                for (int i = 0; i < primaryGun.bullets_per_shot; i++)
                {
                    float tempX = UnityEngine.Random.Range(-0.05f, 0.05f);
                    float tempY = UnityEngine.Random.Range(-0.05f, 0.05f);
                    shootOffset = new Vector3(tempX, tempY, 0);

                    Ray ray = new Ray(cam.transform.position, cam.transform.forward + shootOffset);
                    Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);
                    RaycastHit hitInfo; // variable to store our collision information
                                        // Get raycast hit info
                    if (Physics.Raycast(ray, out hitInfo, distance, mask))
                    {
                        // If hit breakable object
                        if (hitInfo.collider.GetComponent<BreakableObject>() != null)
                        {
                            Debug.Log("bullet hit skull");
                            hitInfo.collider.GetComponent<BreakableObject>().Break();
                        }
                        // If hit enemy
                        else if (hitInfo.collider.GetComponent<Enemy>() != null || hitInfo.collider.GetComponentInParent<Enemy>() != null)
                        {
                            // Check not dead
                            if (!hitInfo.collider.GetComponentInParent<Enemy>().dead)
                            {
                                if (hitInfo.collider.name == "CriticalArea")
                                {
                                    AudioManager.instance.Play("bullet_hit_critical");
                                    hitInfo.collider.GetComponentInParent<EnemyHealth>().ReceiveDamage((primaryGun.damage_lv1_value + primaryGun.damage_currentLv * primaryGun.damage_IncreasePerUpgrade) * damageBuff * 3, true);
                                    // If creature, use creatureHitEffect
                                    if (hitInfo.collider.gameObject.GetComponent<Enemy>().spider)
                                    {
                                        GameObject hitEffectClone = Instantiate(creatureHitEffect, hitInfo.point, hitInfo.transform.rotation) as GameObject;
                                    }
                                    else
                                    {
                                        GameObject hitEffectClone = Instantiate(hitEffect, hitInfo.point, hitInfo.transform.rotation) as GameObject;
                                    }
                                    crosshair_inrange.SetActive(true);
                                    crosshairHitTime = crosshairHitTimer;
                                }
                                else
                                {
                                    AudioManager.instance.Play("bullet_hit_normal");
                                    hitInfo.collider.GetComponent<EnemyHealth>().ReceiveDamage((primaryGun.damage_lv1_value + primaryGun.damage_currentLv * primaryGun.damage_IncreasePerUpgrade) * damageBuff, false);
                                    // If creature, use creatureHitEffect
                                    if (hitInfo.collider.gameObject.GetComponent<Enemy>().spider)
                                    {
                                        GameObject hitEffectClone = Instantiate(creatureHitEffect, hitInfo.point, hitInfo.transform.rotation) as GameObject;
                                    }
                                    else
                                    {
                                        GameObject hitEffectClone = Instantiate(hitEffect, hitInfo.point, hitInfo.transform.rotation) as GameObject;
                                    }
                                    crosshair_inrange.SetActive(true);
                                    crosshairHitTime = crosshairHitTimer;
                                }
                            }
                        }
                        else
                        {
                            // Hit non enemy
                            GameObject hitEffectClone = Instantiate(stoneHitEffect, hitInfo.point, hitInfo.transform.rotation) as GameObject;
                        }
                    }
                }
            }
            // If other gun
            else
            {
                Ray ray = new Ray(cam.transform.position, cam.transform.forward + shootOffset);
                Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);
                RaycastHit hitInfo; // variable to store our collision information
                                    // Get raycast hit info
                if (Physics.Raycast(ray, out hitInfo, distance, mask))
                {
                    // If hit breakable object
                    if (hitInfo.collider.GetComponent<BreakableObject>() != null)
                    {
                        Debug.Log("bullet hit skull");
                        hitInfo.collider.GetComponent<BreakableObject>().Break();
                    }
                    // If hit enemy
                    else if (hitInfo.collider.GetComponent<Enemy>() != null || hitInfo.collider.GetComponentInParent<Enemy>() != null)
                    {
                        // Check not dead
                        if (!hitInfo.collider.GetComponentInParent<Enemy>().dead)
                        {
                            if (hitInfo.collider.name == "CriticalArea")
                            {
                                AudioManager.instance.Play("bullet_hit_critical");
                                hitInfo.collider.GetComponentInParent<EnemyHealth>().ReceiveDamage((primaryGun.damage_lv1_value + primaryGun.damage_currentLv * primaryGun.damage_IncreasePerUpgrade) * damageBuff * 3, true);
                                GameObject hitEffectClone = Instantiate(hitEffect, hitInfo.point, hitInfo.transform.rotation) as GameObject;
                                crosshair_inrange.SetActive(true);
                                crosshairHitTime = crosshairHitTimer;
                            }
                            else
                            {
                                AudioManager.instance.Play("bullet_hit_normal");
                                hitInfo.collider.GetComponent<EnemyHealth>().ReceiveDamage((primaryGun.damage_lv1_value + primaryGun.damage_currentLv * primaryGun.damage_IncreasePerUpgrade) * damageBuff, false);
                                // If creature, use creatureHitEffect
                                if (hitInfo.collider.gameObject.GetComponent<Enemy>().spider)
                                {
                                    Debug.Log("Hit creature");
                                    GameObject hitEffectClone = Instantiate(creatureHitEffect, hitInfo.point, hitInfo.transform.rotation) as GameObject;
                                }
                                else
                                {
                                    GameObject hitEffectClone = Instantiate(hitEffect, hitInfo.point, hitInfo.transform.rotation) as GameObject;
                                }
                                crosshair_inrange.SetActive(true);
                                crosshairHitTime = crosshairHitTimer;
                            }
                        }
                    }
                    else
                    {
                        // Hit non enemy
                        GameObject hitEffectClone = Instantiate(stoneHitEffect, hitInfo.point, hitInfo.transform.rotation) as GameObject;
                    }
                }
            }
        }
    }
    public void AimButton()
    {
        if (canPressBtn)
        {
            canPressBtn = false;
            Invoke("ResetBtnCooldown", 0.25f);
            aiming = !aiming;
            primaryGo.GetComponent<Animator>().SetBool("Aiming", aiming);
            this.GetComponent<Animator>().SetBool("Aiming", aiming);
            hipCrosshair.SetActive(!aiming);
            aimCrosshair.SetActive(aiming);
            if (aiming)
            {
                // Aiming
                this.GetComponent<PlayerLook>().xSensitivity = xSensitivityAim;
                this.GetComponent<PlayerLook>().ySensitivity = ySensitivityAim;
            }
            else
            {
                // Not aiming
                this.GetComponent<PlayerLook>().xSensitivity = xSensitivityHip;
                this.GetComponent<PlayerLook>().ySensitivity = ySensitivityHip;
            }
        }
    }

    //==============================================
    //-----PickUpBulletsItem-----
    //Description: Update primary and secondary gun ammo to max values
    //
    //----Parameters-----------------
    // None
    //----Return---------------------
    // None
    //==============================================
    public void PickUpBulletsItem()
    {
        // Update primary gun mag/ammo to max values
        primary_mag_current = primary_mag_current_max;
        primary_ammoTotal_current = primary_ammoTotal_current_max;
        if (secondaryWeaponId != 99)
        {
            // Update secondaray gun mag/ammo to max values
            secondaryGun.mag_current_value_InGame = gunDatabase.GetGun(secondaryWeaponId).mag_lv1_value + (gunDatabase.GetGun(secondaryWeaponId).mag_currentLv * gunDatabase.GetGun(secondaryWeaponId).mag_IncreasePerUpgrade);
            secondaryGun.ammoTotal_current_value_InGame = gunDatabase.GetGun(secondaryWeaponId).ammoTotal_lv1_value + (gunDatabase.GetGun(secondaryWeaponId).ammoTotal_currentLv * gunDatabase.GetGun(secondaryWeaponId).ammoTotal_IncreasePerUpgrade);
        }

        UpdateAmmoUI();
    }
    public void ReloadButton()
    {
        // skip if full mag
        if (primary_mag_current == primary_mag_current_max)
        {
            Debug.Log("Full mag");
            return;
        }
        if (primary_ammoTotal_current > 0)
        {
            canPressBtn = false;
            Invoke("ResetBtnCooldown", 1.4f);
            primaryGo.GetComponent<Animator>().SetTrigger("Reload");
        }
        else
        {
            Debug.Log("Total ammo is 0");
        }
        // }

    }
    public void ThrowGrenade()
    {
        if (grenade_in_bag > 0)
        {
            grenade_in_bag -= 1;
            grenade_num_UI.text = grenade_in_bag.ToString();
            GameObject grenade = Instantiate(grenadePrefab, this.transform.position, Quaternion.identity);

            // Get the exact direction the player is looking using the main camera's forward direction.
            Vector3 playerLookDirection = Camera.main.transform.forward;
            grenade.GetComponent<Grenade>().Throw(playerLookDirection);
        }
    }
    public void WaveClear_RefillGrenade()
    {
        grenade_in_bag = 2;
        grenade_num_UI.text = grenade_in_bag.ToString();
    }
    private void UpdatePrimaryGun(Gun newPrimary)
    {
        primaryGun = gunDatabase.GetGun(primaryWeaponId);
        // Update secondary weapon image
        Debug.Log("update secondary image");
        secondaryImage.sprite = primaryGun.sprite_white;

        primaryGo = Instantiate(newPrimary.gameObject, newPrimary.holdPos.transform.position, newPrimary.holdPos.transform.rotation) as GameObject;
        primaryGo.name = newPrimary.name;
        primaryGo.transform.parent = gunHoldPos.transform;

        primaryShootSound = newPrimary.shootSound;
        shootEffectPos = primaryGo.transform.Find("ShootEffectPos").gameObject.transform;
        primary_mag_current = gunDatabase.GetGun(newPrimary.id).mag_current_value_InGame;
        primary_mag_current_max = gunDatabase.GetGun(newPrimary.id).mag_current_max_InGame;
        primary_ammoTotal_current = gunDatabase.GetGun(newPrimary.id).ammoTotal_current_value_InGame;
        primary_ammoTotal_current_max = gunDatabase.GetGun(newPrimary.id).ammoTotal_current_max_InGame;
        shootCooldown = newPrimary.shootLag;
        UpdateAmmoUI();
    }
    public void SwitchWeaponButton()
    {
        Debug.Log(secondaryGun);
        // Switch primary and secondary weapon
        // if not secondary gun
        if (secondaryWeaponId == 99)
        {
            Debug.Log("No secondary gun");
            return;
        }
        Debug.Log("Switch gun");

        // Save current ammo in gun object
        // UpdateGunMagAmmoMaxValues();
        UpdateDatabaseMagAmmoCurrentValues();

        Gun tempGun = primaryGun;
        Destroy(primaryGo);

        UpdatePrimaryGun(secondaryGun);
        primaryGun = secondaryGun;
        primaryWeaponId = primaryGun.id;
        secondaryGun = tempGun;
        secondaryWeaponId = secondaryGun.id;

        secondary_mag_current = gunDatabase.GetGun(secondaryWeaponId).mag_current_value_InGame;
        secondary_ammoTotal_current = gunDatabase.GetGun(secondaryWeaponId).ammoTotal_current_value_InGame;

        UpdateAmmoUI();
    }
    public void NewGunToPrimary(Gun newPrimary)
    {
        Destroy(primaryGo);
        primaryGun = gunDatabase.GetGun(newPrimary.id);
        primaryWeaponId = primaryGun.id;

        primaryGo = Instantiate(newPrimary.gameObject, newPrimary.holdPos.transform.position, newPrimary.holdPos.transform.rotation) as GameObject;
        primaryGo.name = newPrimary.name;
        primaryGo.transform.parent = gunHoldPos.transform;

        primaryShootSound = newPrimary.shootSound;
        shootEffectPos = primaryGo.transform.Find("ShootEffectPos").gameObject.transform;
        primary_mag_current = gunDatabase.GetGun(primaryWeaponId).mag_current_value_InGame;
        primary_mag_current_max = gunDatabase.GetGun(primaryWeaponId).mag_current_max_InGame;
        primary_ammoTotal_current = gunDatabase.GetGun(primaryWeaponId).ammoTotal_current_value_InGame;
        primary_ammoTotal_current_max = gunDatabase.GetGun(primaryWeaponId).ammoTotal_current_max_InGame;
        shootCooldown = newPrimary.shootLag;
        UpdateAmmoUI();
    }
    public void NewGunToSecondary(Gun newSecondary)
    {
        secondaryGun = gunDatabase.GetGun(newSecondary.id);
        secondaryWeaponId = secondaryGun.id;

        secondary_mag_current = gunDatabase.GetGun(secondaryWeaponId).mag_current_value_InGame;
        secondary_ammoTotal_current = gunDatabase.GetGun(secondaryWeaponId).ammoTotal_current_value_InGame;

        // Change the color of the sprite so that it is visible
        Debug.Log("Update secondaryImage color");
        secondaryImage.color = secondaryImageColor;
        secondaryImage.sprite = primaryGun.sprite_white;
        secondaryImage.sprite = gunDatabase.GetGun(secondaryGun.id).sprite_white;
    }

    private void ResetBtnCooldown()
    {
        canPressBtn = true;
    }

    // After reload animation
    public void ReloadAmmo()
    {
        // Total ammo is less than mag size
        Debug.Log("Reload Ammo execute");
        if (primary_ammoTotal_current < primary_mag_current_max)
        {
            Debug.Log("");
            primary_mag_current = primary_ammoTotal_current;
            primary_ammoTotal_current = 0;
        }
        else
        // Total ammo is more than mag size
        {
            // Get ammo difference
            int tempAmmo = primary_mag_current_max - primary_mag_current;
            // Decrease difference from total
            primary_ammoTotal_current -= tempAmmo;
            // Add to mag
            primary_mag_current = primary_mag_current_max;
        }
        UpdateAmmoUI();
    }

    private bool CheckAmmo()
    {
        // Bullets left in mag
        if (primary_mag_current > 0)
        {
            return true;
        }
        // Mag empty but total ammo left -> Auto reload mag
        else if (primary_ammoTotal_current > 0)
        {
            ReloadButton();
            return false;
        }
        // Mag and total ammo empty
        else
        {
            // Play emply mag SE
            Debug.Log("No ammo");
            return false;
        }
    }

    private void UpdateAmmoUI()
    {
        gunAmmoText.text = primary_mag_current + " / " + primary_ammoTotal_current;
    }

    // public void BuyWallGun(int wallWeaponId)
    // {
    //     secondaryImage.color = secondaryImageColor;
    //     // primary and wall is same -> bullets

    //     // if no secondary weapon
    //     if (secondaryWeaponId == 99)
    //     {
    //         AudioManager.instance.Play("get_gun");
    //         secondaryWeaponId = wallWeaponId;
    //         secondaryGun = gunDatabase.GetGun(wallWeaponId);
    //         SwitchWeaponButton();
    //     }
    //     else if (primaryWeaponId == wallWeaponId)
    //     {
    //         primaryGun.curMag = gunDatabase.GetGun(primaryWeaponId).mag;
    //         primaryGun.primary_ammoTotal_current = gunDatabase.GetGun(primaryWeaponId).primary_ammoTotal_current_max;

    //         primary_mag_current = primaryGun.curMag;
    //         primary_ammoTotal_current = primaryGun.primary_ammoTotal_current;

    //         UpdateAmmoUI();
    //     }
    //     else if (secondaryWeaponId == wallWeaponId)
    //     {
    //         secondaryGun.curMag = gunDatabase.GetGun(secondaryWeaponId).mag;
    //         secondaryGun.primary_ammoTotal_current = gunDatabase.GetGun(secondaryWeaponId).primary_ammoTotal_current_max;

    //         UpdateAmmoUI();
    //     }
    //     // Replace primary weapon with wall gun
    //     else if (primaryWeaponId != wallWeaponId && secondaryWeaponId != wallWeaponId)
    //     {
    //         Debug.Log("Replacing current gun with new gun");
    //         Debug.Log("Get gun : " + gunDatabase.GetGun(wallWeaponId).name);
    //         NewGunToPrimary(gunDatabase.GetGun(wallWeaponId));
    //         canPressBtn = false;
    //         Invoke("ResetBtnCooldown", 1f);

    //     }
    //     else
    //     {
    //         Debug.Log("ERROR: gun id unknown");
    //     }
    // }

    // public void BuyBulletPack()
    // {
    //     primaryGun.curMag = gunDatabase.GetGun(primaryWeaponId).mag;
    //     primaryGun.primary_ammoTotal_current = gunDatabase.GetGun(primaryWeaponId).primary_ammoTotal_current_max;

    //     primary_mag_current = primaryGun.curMag;
    //     primary_ammoTotal_current = primaryGun.primary_ammoTotal_current;

    //     UpdateAmmoUI();
    // }

    // public bool PrimaryGunAmmoIsMax()
    // {
    //     if ((primary_mag_current == primary_mag_current_max) && (primary_ammoTotal_current == primary_ammoTotal_current_max))
    //     {
    //         Debug.Log("primary gun ammo is max");
    //         return true;
    //     }
    //     primary_mag_current = primaryGun.mag;
    //     primary_ammoTotal_current = primaryGun.primary_ammoTotal_current_max;
    //     UpdateAmmoUI();
    //     return false;
    // }
    // public bool SecondaryGunAmmoIsMax()
    // {
    //     if ((secondary_mag_current == secondary_mag_current_max) && (secondary_ammoTotal_current == secondary_ammoTotal_current_max))
    //     {
    //         Debug.Log("secondary gun ammo is max");
    //         return true;
    //     }
    //     secondary_mag_current = secondaryGun.mag;
    //     secondary_ammoTotal_current = secondaryGun.primary_ammoTotal_current_max;
    //     UpdateAmmoUI();
    //     return false;
    // }

    public void CheckCrosshair()
    {
        if (crosshairHitTime <= 0)
        {
            crosshair_inrange.SetActive(false);
            return;
        }
        crosshairHitTime -= Time.deltaTime;

    }
    public void CrosshairHitRed()
    {
        for (int i = 0; i < 4; i++)
        {
            hipCrosshair.GetComponentsInChildren<Image>()[i].color = Color.red;
        }
        aimCrosshair.GetComponentInChildren<Image>().color = Color.red;
        crosshairHitTime = crosshairHitTimer;
    }
    public void CrosshairHitWhite()
    {
        for (int i = 0; i < 4; i++)
        {
            hipCrosshair.GetComponentsInChildren<Image>()[i].color = Color.white;
        }
        aimCrosshair.GetComponentInChildren<Image>().color = Color.white;
    }

    public void OnKnifeRangeEnter(GameObject skull)
    {
        Debug.Log("Knife btn active");
        skullInRange = skull;
        knifeBtn2.SetActive(true);
    }
    public void OnKnifeRangeExit()
    {
        skullInRange = null;
        knifeBtn2.SetActive(false);
    }
}