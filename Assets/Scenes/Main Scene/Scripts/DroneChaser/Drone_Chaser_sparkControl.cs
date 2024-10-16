using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Drone_Chaser_sparkControl : MonoBehaviour
{
    // Start is called before the first frame update
    ParticleSystem _particleSystem;

    public Drone_Chaser_attack drone_Chaser_Attack;
    private int damage;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();

        damage = drone_Chaser_Attack.attackDamage;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other != null && other.gameObject.tag == "Player")
        {
            MovingMachine machine = other.GetComponent<MovingMachine>();
            machine.Damaged(damage);
        }
    }
}
