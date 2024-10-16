using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getRotation : MonoBehaviour
{
    Transform player;


    private void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.rotation = new Quaternion(0f,0f,player.rotation.y,-1 * player.rotation.w);

    }
}
