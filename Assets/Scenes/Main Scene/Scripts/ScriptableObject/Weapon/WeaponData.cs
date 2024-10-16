using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObject/WeaponData")]
public class WeaponData :ScriptableObject
{
    public string useWeapon;

    public int weaponAttack = 20; // 무기 공격력

    public float weaponBetTime = 2f;

    public AudioClip weaponShotSound;
}
