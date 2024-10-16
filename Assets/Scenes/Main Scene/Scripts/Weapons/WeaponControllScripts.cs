using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControllScripts : MonoBehaviour
{
    public WeaponData[] weaponDatas;
    public HS_ProjectileMover bulletPrefab;
    
    public float bulletLifeTIme = 10f;

    private PlayerInputMove playerInputMove;
    private Animator playerAnimator;
    
    private float weaponBetTime;
    private float weaponFireLastTime;

    public string[] specialWeaponNames;
    public GameObject[] specialWeapons;
    public Transform[] shotPositions;
    int weaponIdx;

    AudioSource audio;
    AudioClip shootSound;

    // 플레이어 초기 능력치
    public float basePlayerATKSpeed;

    int finalATK
    {
        get
        {
            m_finalAtk = weaponDatas[weaponIdx].weaponAttack + (int)(weaponDatas[weaponIdx].weaponAttack * (PlayerPrefs.GetInt("AttackLevel") * 0.1f));
            return m_finalAtk;
        }
    }
    int m_finalAtk;

    float finalATKSpeed
    {
        get
        {
            m_finalATKSpeed = (1.0f - (PlayerPrefs.GetInt("ATKSpeedLevel") * 0.1f)) * weaponDatas[weaponIdx].weaponBetTime;
            return m_finalATKSpeed;
        }
    }
    float m_finalATKSpeed;


    public enum WeaponState
    {
        ready,
        fire,
        empty,
        Reloading
    }
    public WeaponState weaponState { get; set; }

    private void Awake()
    {
        playerInputMove = GetComponent<PlayerInputMove>();
        playerAnimator = GetComponent<Animator>();

        audio = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        
        weaponState = WeaponState.ready;

        weaponFireLastTime = 0f;

        string findWeapon = PlayerPrefs.GetString("UseWeapon");

        for(int i = 0;i<specialWeaponNames.Length;i++)
        {
            if(findWeapon.Equals(specialWeaponNames[i]))
            {

                specialWeapons[i].SetActive(true);
                weaponIdx = i;
                shootSound = weaponDatas[i].weaponShotSound;
            }
        }
    }


    private void FixedUpdate()
    {
        if (weaponState == WeaponState.fire)
        {
            attackShoot();
        }
    }

    public void attackShoot() // 총알을 발사
    {

        if(weaponFireLastTime < Time.time && GameManager.instance.isGameover == false)
        {

            if (weaponIdx == 0)
            {
                for(int i = 0;i<5;i++)
                {
                    audio.PlayOneShot(shootSound);
                    HS_ProjectileMover bullet_ShotGun = Instantiate(bulletPrefab, shotPositions[weaponIdx].position, shotPositions[weaponIdx].rotation * new Quaternion(0f,Random.Range(0.2f,-0.2f),0f,1f));
                    bullet_ShotGun.setDamage(finalATK);
                    Destroy(bullet_ShotGun, bulletLifeTIme);
                    playerAnimator.SetTrigger("isAttackShoot");
                }
            }
            else
            {
                audio.PlayOneShot(shootSound);
                HS_ProjectileMover bullet = Instantiate(bulletPrefab, shotPositions[weaponIdx].position, shotPositions[weaponIdx].rotation);
                bullet.setDamage(finalATK);
                Destroy(bullet, bulletLifeTIme);
                playerAnimator.SetTrigger("isAttackShoot");

            }

            weaponFireLastTime = Time.time + finalATKSpeed;

        }
    }

    
}
