using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DroneBossrData", menuName = "ScriptableObject/DroneBossData")]
public class DroneBossData : ScriptableObject
{
    public int hp = 1000; // ü��
    public int attackDamage = 50; // ���ݷ�
    public float moveSpeed = 2f; // �̵� �ӵ�
}
