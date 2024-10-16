using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Drone_Shooter : MovingMachine
{
    public LayerMask Target; // 플레이어

    public HS_ProjectileMover bulletPrefab;

    public Transform targetLockOnPosition;
    public float targetDistance;

    private MovingMachine targetMachine;
    private NavMeshAgent navMeshAgent;

    public List<GameObject> ShootEffects;


    public AudioClip deathClip;
    public AudioClip attackClip;
    public AudioClip damagedClip;

    private Animator drone_Chaser_Animater;
    private AudioSource droneAudio;
    private Renderer droneRenderer;

    public float waitAppearTime;
    private bool isInDisATK;
    public float waitAttackTIme;
    private bool isAttack = false;
    public int damage = 20;
    public float timeBetAttack = 1f;
    private float lastTimeAttackTime;

    public float droneSpeed = 3.5f;

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



    private void Awake()
    {

        navMeshAgent = GetComponent<NavMeshAgent>();

        droneAudio = GetComponent<AudioSource>();

        droneRenderer = GetComponentInChildren<Renderer>();
        drone_Chaser_Animater = GetComponent<Animator>();
    }

    public void Setup(DroneShooterData droneShooterData)
    {
        // 체력 설정
        start_HP = droneShooterData.hp;
        current_HP = droneShooterData.hp;
        // 공격력 설정
        attackDamage = droneShooterData.attackDamage;
        // 내비메시 에이전트의 이동 속도 설정
        navMeshAgent.speed = droneShooterData.moveSpeed;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine("appearTime");
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
        drone_Chaser_Animater.SetBool("isTarget", isTarget);
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

            yield return new WaitForSeconds(0.25f);
        }

    }

    public override void Damaged(int value)
    {
        if (isDead == false)
        {
            droneAudio.PlayOneShot(damagedClip);
        }
        base.Damaged(value);
    }

    public override void Die()
    {
        base.Die();

        StopAllCoroutines();

        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;

        Collider[] Drone_Chaser_colliders = GetComponents<Collider>();
        for (int i = 0; i < Drone_Chaser_colliders.Length; i++)
        {
            Drone_Chaser_colliders[i].enabled = false;
        }

        drone_Chaser_Animater.SetTrigger("isDie");
        droneAudio.PlayOneShot(deathClip);

        Destroy(gameObject, 5f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (targetMachine.isDead == false && isDead == false)
        {
            if (other.tag == "Player")
            {
                RaycastHit hit;
                if (Physics.Raycast(targetLockOnPosition.position, (targetMachine.transform.position - targetLockOnPosition.transform.position).normalized, out hit, 100f) && isAttack == false)
                {
                    if (hit.collider.tag == "Player")
                    {
                        StartCoroutine("attacking");
                    }
                }
            }
        }
    }

    IEnumerator attacking()
    {
        transform.LookAt(new Vector3(targetMachine.transform.position.x, gameObject.transform.position.y, targetMachine.transform.position.z));
        navMeshAgent.isStopped = true;
        yield return new WaitForSeconds(waitAttackTIme);
        drone_Chaser_Animater.SetBool("isAttack", isAttack);
        isAttack = true;
        yield return new WaitForSeconds(waitAttackTIme + 2f);
        isAttack = false;
        drone_Chaser_Animater.SetBool("isAttack", isAttack);
        navMeshAgent.isStopped = false;
    }


    //private void OnTriggerExit(Collider other)
    //{


    //    if (targetMachine.isDead == false && isDead == false)
    //    {
    //        MovingMachine attackTarget = other.GetComponent<MovingMachine>();

    //        if (attackTarget != null && attackTarget == targetMachine)
    //        {
    //            isInDisATK = false;
    //        }

    //    }
    //}


    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null && collision.collider.tag == "Bullet" && isDead == false)
        {
            HS_ProjectileMover bullet = collision.gameObject.GetComponent<HS_ProjectileMover>();

            if (bullet != null)
            {
                Damaged(bullet.getDamage());
            }
        }
    }


    public void ShootEffectAppear(int num)
    {
        droneAudio.PlayOneShot(attackClip);
        HS_ProjectileMover bullet = Instantiate(bulletPrefab, ShootEffects[num].transform.position, ShootEffects[num].transform.rotation);
        bullet.setDamage(damage);
    }
}
