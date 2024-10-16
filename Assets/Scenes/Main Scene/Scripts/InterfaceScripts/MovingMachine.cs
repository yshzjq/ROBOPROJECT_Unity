using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingMachine : MonoBehaviour, DamageAct
{
    protected int start_HP; // 초기 체력
    public int current_HP { get; protected set; } // 현재 체력
    public int attackDamage { get; set; } // 공격력

    public float attackRange { get; protected set; } // 공격거리
    public float attackDistance { get; protected set; } // 공격 범위

    public bool isDead { get; protected set; } // 사망 여부

    public event Action onDeath; // 사망시 함수들을 발동할 이벤트

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
