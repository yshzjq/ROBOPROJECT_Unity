using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drone_Boss_Controller : MovingMachine
{
    public LayerMask Target; // �÷��̾�

    public HS_ProjectileMover bulletPrefab;

    public Transform targetLockOnPosition;
    public float targetDistance;

    public DronesData DronesData;

    private MovingMachine targetMachine;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;

    public List<GameObject> ShootEffects;


    public AudioClip deathClip;
    public AudioClip attackClip;
    public AudioClip damagedClip;

    private Animator drone_Boss_Animater;
    private AudioSource droneAudio;
    private Renderer droneRenderer;

    public float waitAppearTime;
    private bool isInDisATK;
    public float waitAttackTIme;
    private bool isAttack = false;
    public int damage = 20;
    public float timeBetAttack = 5f;
    private float lastTimeAttackTime;

    public float droneSpeed = 3.5f;

    private int baseAttackCount = 0;

    public float bossUppingDelayTime = 0.1f;


    // special Attack 

    public GameObject chargingSpark;
    public GameObject emissionSpark;

    public GameObject lightningRangePrefab;

    private Slider boss_HP;

    private bool isTarget
    {
        get
        {
            if (targetMachine != null && targetMachine.isDead == false)
            {
                return true;
            }

            return false;
        }
    }

    public void sparkAttackStop()
    {
        chargingSpark.SetActive(false);
        emissionSpark.SetActive(false);
        lightningRangePrefab.SetActive(false);
    }


    public void Setup(DroneBossData droneBossData)
    {
        // 체력 설정
        start_HP = droneBossData.hp;
        current_HP = droneBossData.hp;
        // 공격력 설정
        attackDamage = droneBossData.attackDamage;
        // 내비메시 에이전트의 이동 속도 설정
        navMeshAgent.speed = droneBossData.moveSpeed;
    }


    private void Awake()
    {

        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        drone_Boss_Animater = GetComponent<Animator>();
        droneAudio = GetComponent<AudioSource>();

        droneRenderer = GetComponentInChildren<Renderer>();

    }

    public override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine("appearTime");
        lastTimeAttackTime = 0f;

        boss_HP = UIManagerScripts.instance.BossHPUIAppear();
    }

    public void SetMachine(DronesData droneData)
    {
        start_HP = droneData.HP;
        current_HP = start_HP;
    }

    IEnumerator appearTime()
    {
        yield return new WaitForSeconds(waitAppearTime);

        StartCoroutine("UpdateTargetPos");
    }

    private void Update()
    {
        drone_Boss_Animater.SetBool("isTarget", isTarget);

        
    }
    
    private IEnumerator SpecialAttack() // 보스 특수 공격
    {
        navMeshAgent.speed = 0.01f;
        drone_Boss_Animater.SetTrigger("ReadySpecialAttack");

        yield return new WaitForSeconds(3f); 

        transform.position = new Vector3(targetMachine.transform.position.x,transform.position.y,targetMachine.transform.position.z);
         
        //애니메이션 설정2 (차징)

        yield return new WaitForSeconds(5f);

        //애니메이션 설정3 (방출)
        yield return new WaitForSeconds(7f);

        

        yield return null;
    }

    private IEnumerator UpdateTargetPos()
    {

        while (isDead == false)
        {

            if (isTarget == true && isAttack == false)
            {
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(targetMachine.transform.position);
                Collider[] colliders = GetComponents<Collider>();

                for (int i = 0; i < colliders.Length; i++)
                {
                    {
                        colliders[i].enabled = true;
                    }
                }

            }
            else if (isTarget == false && isAttack == false)
            {
                navMeshAgent.isStopped = true;

                Collider[] colliders = Physics.OverlapSphere(transform.position, 100f, Target);

                for (int i = 0; i < colliders.Length; i++)
                {
                    MovingMachine movingMachine = colliders[i].GetComponent<MovingMachine>();

                    if (movingMachine != null && movingMachine.isDead == false)
                    {
                        targetMachine = movingMachine;
                        break;
                    }
                }
            }
            else if (isTarget == true && isAttack == true)
            {
                navMeshAgent.SetDestination(targetMachine.transform.position);
                Collider[] colliders = GetComponents<Collider>();

                for (int i = 0; i < colliders.Length; i++)
                {
                    {
                        colliders[i].enabled = true;
                    }
                }
            }

            yield return new WaitForSeconds(0.25f);
        }

    }

    public override void Damaged(int value)
    {
        if (isDead == false)
        {
            Debug.Log(current_HP + "  " + start_HP);
            boss_HP.value = ((float)current_HP / (float)start_HP);
            Debug.Log(boss_HP.value);
            //droneAudio.PlayOneShot(damagedClip);
        }
        base.Damaged(value);
    }

    public override void Die()
    {
        base.Die();

        StopAllCoroutines();

        Collider[] Drone_Chaser_colliders = GetComponents<Collider>();
        for (int i = 0; i < Drone_Chaser_colliders.Length; i++)
        {
            Drone_Chaser_colliders[i].enabled = false;
        }

        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;


        drone_Boss_Animater.SetTrigger("isDie");
        droneAudio.PlayOneShot(deathClip);

        Destroy(gameObject, 5f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (targetMachine.isDead == false && isDead == false && baseAttackCount < 3 && isAttack == false)
        {
            if (other.tag == "Player")
            {
                RaycastHit hit;
                if (Physics.Raycast(targetLockOnPosition.position, (targetMachine.transform.position - targetLockOnPosition.transform.position).normalized, out hit, 100f) && isAttack == false && lastTimeAttackTime < Time.time)
                {
                    lastTimeAttackTime = Time.time + timeBetAttack;
                    if (hit.collider.tag == "Player")
                    {
                        isAttack = true;
                        StartCoroutine("attacking");
                    }

                }
            }
        }
    }

    IEnumerator attacking()
    {


        transform.LookAt(new Vector3(targetMachine.transform.position.x, gameObject.transform.position.y, targetMachine.transform.position.z));
        navMeshAgent.speed = 0.01f;

        isAttack = true;
        drone_Boss_Animater.SetBool("isAttack", isAttack);
        
        
        yield return new WaitForSeconds(waitAttackTIme);


        drone_Boss_Animater.SetBool("isAttack", !isAttack);

        baseAttackCount++;

        if (baseAttackCount >= 3)
        {
            yield return new WaitForSeconds(waitAttackTIme);
            yield return StartCoroutine("SpecialAttack");
            baseAttackCount = 0;
        }
        isAttack = false;
        navMeshAgent.speed = 3.5f;

        
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null && collision.collider.tag == "Bullet" && isDead == false)
        {
            HS_ProjectileMover bullet = collision.gameObject.GetComponent<HS_ProjectileMover>();

            if (bullet != null)
            {
                Damaged(bullet.getDamage());
                Debug.Log(current_HP);
            }
        }
    }


    public void ShootEffectAppear(int num)
    {
        droneAudio.PlayOneShot(attackClip);
        HS_ProjectileMover bullet = Instantiate(bulletPrefab, ShootEffects[num].transform.position, ShootEffects[num].transform.rotation);
        bullet.setDamage(damage);
    }

    public void ChargingSparkAppear()
    {
        chargingSpark.SetActive(true);

        Instantiate(lightningRangePrefab, transform.position - new Vector3(0f, 1.2f, 0f), new Quaternion(1f,0f,0f,1f));
    }

    public void EmissionSpartAppear()
    {
        chargingSpark.SetActive(false);
        emissionSpark.SetActive(true);
    }

    public void EmissionSpartDisAppear()
    {
        emissionSpark.SetActive(false);
    }

    
}
