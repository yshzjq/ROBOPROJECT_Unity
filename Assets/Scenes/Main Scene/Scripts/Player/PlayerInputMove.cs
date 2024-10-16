using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WeaponControllScripts;

public class PlayerInputMove : MonoBehaviour // 플레이어의 움직임을 입력받아 이동 및 애니메이션 구현
{

    public float playerBaseSpeed;
    public VariableJoystick moveJoystick;
    public VariableJoystick attackJoystick;

    public Transform lookShootPoint;
    public float lookShootPointDis;

    private Rigidbody playerRigidbody;
    private Animator playerAnimator;

    private WeaponControllScripts weaponControll;

    public Vector3 move_direction = Vector3.zero;
    public Vector3 attack_direction = Vector3.zero;

    private bool dashPossible = true;
    private float dashMoveSpeed = 1f;

    // 플레이어 이동속도 적용 프로퍼티

    float player_speed {
        get
        {
            m_player_speed = playerBaseSpeed * (1 + (PlayerPrefs.GetInt("SpeedLevel") * 0.1f));
            return m_player_speed * dashMoveSpeed;
        }
    }
    float m_player_speed;


    public bool isMove
    {
        get {
            float checkValue = Mathf.Pow(moveJoystick.Vertical, 2) + Mathf.Pow(moveJoystick.Horizontal, 2);
            if (checkValue > 0.8f) return true;
            return false;
        }
    }
    public bool isAttack
    {
        get
        {
            float checkValue = Mathf.Pow(attackJoystick.Vertical, 2) + Mathf.Pow(attackJoystick.Horizontal, 2);
            if (checkValue > 0.8f)
            {
                weaponControll.weaponState = WeaponState.fire;
                return true; 
            }

            weaponControll.weaponState = WeaponState.ready;
            return false;
        }
    }

    public float MoveDirection(Vector3 dir1,Vector3 dir2)
    {
        float check = Vector3.Dot(dir2, dir1);

        if (isMove == true)
        {
            if (check > 0f) return 1f;
            else if (check < 0f) return -1f;
            return 1f;
        }
        else return 0f;
        
    }
    bool m_moveDirection;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        weaponControll = GetComponent<WeaponControllScripts>();
    }

    public void FixedUpdate()
    {
        if (GameManager.instance.isGameover)
        {
            return;
        }

        playerRigidbody.velocity = Vector3.zero;

        move_direction = Vector3.forward * moveJoystick.Vertical + Vector3.right * moveJoystick.Horizontal;         // move_JoyStick 조작을 나타내는 값
        attack_direction = Vector3.forward * attackJoystick.Vertical + Vector3.right * attackJoystick.Horizontal;   // attack_JoyStick 조작을 나타내는 값

        if (isMove == true) /// move_JoyStick 이 일정 값 이상 인식 될때 true
        {
            playerRigidbody.MovePosition(transform.position + (move_direction * Time.deltaTime * player_speed));                           // 플레이어가 움직인다.
            if (move_direction != Vector3.zero && isAttack == false) transform.rotation = Quaternion.LookRotation(move_direction);  // 움직이면 플레이어가 가는 방향을 향해 각도 설정

        }

        if (isAttack == true) // attack_JoyStick 이 일정 값 이상 인식 될때 true
        {
            if (attack_direction != Vector3.zero) transform.rotation = Quaternion.LookRotation(attack_direction.normalized); // 조준하는 애니메이션 
        }

        lookShootPoint.position = gameObject.transform.position + (attack_direction.normalized * lookShootPointDis); // 조준 시점 변경

        // 애니메이션 출력
        playerAnimator.SetFloat("move", MoveDirection(move_direction,attack_direction)); 
        playerAnimator.SetBool("isAttackReady",isAttack);


        
        //샷 포인트
        
    }

    public void InputDashButton()
    {
        if(dashPossible == true)
        {
            StartCoroutine(DashMove());
        }
    }

    public void DashReady()
    {
        dashPossible = true;
    }

    IEnumerator DashMove()
    {
        dashPossible = false;
        dashMoveSpeed = 1.5f;
        yield return new WaitForSeconds(1f);
        dashMoveSpeed = 1f;
    }
}
