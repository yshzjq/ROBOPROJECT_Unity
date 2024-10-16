using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DroneData",menuName = "ScriptableObject/DroneData")]
public class DronesData : ScriptableObject
{
    public int HP;
    public int ATK;
    public float ATKBetTime;
    public float ATKDistance;

    public AudioClip DieSound;
    public AudioClip AttackSound;
}
