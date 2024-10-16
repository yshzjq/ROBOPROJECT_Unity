using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public class Drone_Chaser_attack : MovingMachine
{
    public LayerMask Target; // 플레이어

    private MovingMachine targetMachine;
    private NavMeshAgent navMeshAgent;

    public GameObject attackEffect;


    // 오디오 소스들
    private AudioSource droneAudio;
    public AudioClip deathClip;
    public AudioClip attackClip;
    public AudioClip damagedClip;

    private Animator drone_Chaser_Animater;
    
    private Renderer droneRenderer;

    public float waitAppearTime;
    private bool isInDisATK;
    public float waitAttackTIme;
    private bool isAttack = false;

    public float timeBetAttack = 1f;
    private float lastTimeAttackTime;

    public float droneSpeed = 3.5f; // 드론의 스피드

    public Material eyecolor; // 드론의 눈동자 색

    public void Setup(DroneChaserData droneChaserData)
    {
        // 체력 설정
        start_HP = droneChaserData.hp;
        current_HP = droneChaserData.hp;
        // 공격력 설정
        attackDamage = droneChaserData.attackDamage;
        // 내비메시 에이전트의 이동 속도 설정
        navMeshAgent.speed = droneChaserData.moveSpeed;
    }

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
        drone_Chaser_Animater = GetComponent<Animator>();
        droneAudio = GetComponent<AudioSource>();

        droneRenderer = GetComponentInChildren<Renderer>();
        attackEffect.SetActive(false);
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
        drone_Chaser_Animater.SetBool("isTarget",isTarget);
    }

    private IEnumerator UpdateTargetPos()
    {

        while (isDead == false)
        {

            if (isTarget == true && isAttack == false && isInDisATK == false)
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
        
        Collider[] Drone_Chaser_colliders = GetComponentsInChildren<Collider>();
        for (int i = 0; i < Drone_Chaser_colliders.Length; i++)
        {
            Drone_Chaser_colliders[i].enabled = false;
        }

        navMeshAgent.enabled = false;

        drone_Chaser_Animater.SetTrigger("isDie");
        droneAudio.PlayOneShot(deathClip);

        Destroy(gameObject, 5f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (targetMachine.isDead == false && isDead == false)
        {
            MovingMachine attackTarget = other.GetComponent<MovingMachine>();

            if (attackTarget != null && attackTarget == targetMachine && isInDisATK == false)
            {
                isInDisATK = true;
                navMeshAgent.isStopped = true;
                
                StartCoroutine(attacking());
            }
        }
    }

    IEnumerator attacking()
    {
        while (true)
        {
            isAttack = true;

            eyecolor.color = Color.white;
            yield return new WaitForSeconds(waitAttackTIme);
            eyecolor.color = Color.red;
            drone_Chaser_Animater.SetBool("isAttack",isAttack);

            yield return new WaitForSeconds(waitAttackTIme + 1f);
            eyecolor.color = Color.white;
            isAttack = false;
            drone_Chaser_Animater.SetBool("isAttack", isAttack);
            if (isAttack == false && isInDisATK == false)
            {
                break;
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {

        if (targetMachine.isDead == false)
        {
            MovingMachine attackTarget = other.GetComponent<MovingMachine>();

            if (attackTarget != null && attackTarget == targetMachine)
            {
                isInDisATK = false;

            }
            
        }
    }

   public void sparkAttackStart()
    {
        attackEffect.SetActive(true);
    }

    public void sparkAttackStop()
    {
        attackEffect.SetActive(false);
    }

    public void sparkSoundPlay()
    {
        droneAudio.PlayOneShot(attackClip);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision != null && collision.collider.tag == "Bullet" && isDead == false)
        {
            HS_ProjectileMover bullet = collision.gameObject.GetComponent< HS_ProjectileMover>();

            if (bullet != null)
            {
                Damaged(bullet.getDamage());
            }
        }
    }



}
