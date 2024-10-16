using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone_Boss_sparkControl : MonoBehaviour
{
    // Start is called before the first frame update
    ParticleSystem _particleSystem;

    public Drone_Boss_Controller drone_Boss;
    private int damage;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other != null && other.gameObject.tag == "Player")
        {
            MovingMachine machine = other.GetComponent<MovingMachine>();
            machine.Damaged(50);
        }
    }
}
