using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WeaponControllScripts;

public class PlayerInputMove : MonoBehaviour // �÷��̾��� �������� �Է¹޾� �̵� �� �ִϸ��̼� ����
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

    // �÷��̾� �̵��ӵ� ���� ������Ƽ

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

        move_direction = Vector3.forward * moveJoystick.Vertical + Vector3.right * moveJoystick.Horizontal;         // move_JoyStick ������ ��Ÿ���� ��
        attack_direction = Vector3.forward * attackJoystick.Vertical + Vector3.right * attackJoystick.Horizontal;   // attack_JoyStick ������ ��Ÿ���� ��

        if (isMove == true) /// move_JoyStick �� ���� �� �̻� �ν� �ɶ� true
        {
            playerRigidbody.MovePosition(transform.position + (move_direction * Time.deltaTime * player_speed));                           // �÷��̾ �����δ�.
            if (move_direction != Vector3.zero && isAttack == false) transform.rotation = Quaternion.LookRotation(move_direction);  // �����̸� �÷��̾ ���� ������ ���� ���� ����

        }

        if (isAttack == true) // attack_JoyStick �� ���� �� �̻� �ν� �ɶ� true
        {
            if (attack_direction != Vector3.zero) transform.rotation = Quaternion.LookRotation(attack_direction.normalized); // �����ϴ� �ִϸ��̼� 
        }

        lookShootPoint.position = gameObject.transform.position + (attack_direction.normalized * lookShootPointDis); // ���� ���� ����

        // �ִϸ��̼� ���
        playerAnimator.SetFloat("move", MoveDirection(move_direction,attack_direction)); 
        playerAnimator.SetBool("isAttackReady",isAttack);


        
        //�� ����Ʈ
        
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
