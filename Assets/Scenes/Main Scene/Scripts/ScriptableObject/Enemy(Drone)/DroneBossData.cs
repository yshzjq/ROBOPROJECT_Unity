using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DroneBossrData", menuName = "ScriptableObject/DroneBossData")]
public class DroneBossData : ScriptableObject
{
    public int hp = 1000; // 체력
    public int attackDamage = 50; // 공격력
    public float moveSpeed = 2f; // 이동 속도
}
