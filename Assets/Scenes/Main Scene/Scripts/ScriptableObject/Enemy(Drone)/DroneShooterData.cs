using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DroneShooterData", menuName = "ScriptableObject/DroneShooterData")]
public class DroneShooterData : ScriptableObject
{
    public int hp = 100; // ü��
    public int attackDamage = 20; // ���ݷ�
    public float moveSpeed = 2f; // �̵� �ӵ�
}
