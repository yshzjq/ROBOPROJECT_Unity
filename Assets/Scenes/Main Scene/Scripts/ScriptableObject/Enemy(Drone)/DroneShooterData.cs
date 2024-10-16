using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DroneShooterData", menuName = "ScriptableObject/DroneShooterData")]
public class DroneShooterData : ScriptableObject
{
    public int hp = 100; // 체력
    public int attackDamage = 20; // 공격력
    public float moveSpeed = 2f; // 이동 속도
}
