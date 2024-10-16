using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingMachine : MonoBehaviour, DamageAct
{
    protected int start_HP; // �ʱ� ü��
    public int current_HP { get; protected set; } // ���� ü��
    public int attackDamage { get; set; } // ���ݷ�

    public float attackRange { get; protected set; } // ���ݰŸ�
    public float attackDistance { get; protected set; } // ���� ����

    public bool isDead { get; protected set; } // ��� ����

    public event Action onDeath; // ����� �Լ����� �ߵ��� �̺�Ʈ

    public virtual void OnEnable()
    {
        isDead = false;
        current_HP = start_HP;
    }
    public virtual void Damaged(int value)
    {
        current_HP -= value;

        if (current_HP <= 0)
        {
            Die();
        }
    }
    public virtual void Die()
    {
        if(onDeath != null)
        {
            onDeath();
        }
        isDead = true;
    }
    public virtual void Attack()
    {
        return;
    }
}
